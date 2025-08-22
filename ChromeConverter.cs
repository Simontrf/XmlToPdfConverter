using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Engine
{
    [System.Obsolete("Utilisez ChromeConversionService à la place. Cette classe sera supprimée dans une version future.")]
    public class ChromeConverter : IXmlToPdfConverter
    {
        private readonly string chromePath;
        private readonly string chromeProfile;

        public ChromeConverter(string chromePath, string chromeProfile)
        {
            this.chromePath = chromePath;
            this.chromeProfile = chromeProfile;

            if (!Directory.Exists(chromeProfile))
                Directory.CreateDirectory(chromeProfile);
        }

        public async Task<bool> Convert(string xmlPath, string outputPdfPath, IProgressReporter progress, ILogger logger)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                progress.Report(5, "Préparation des chemins...");
                if (!File.Exists(xmlPath))
                {
                    logger.Log("Fichier XML introuvable", LogLevel.Error);
                    return false;
                }

                string xmlDirectory = Path.GetDirectoryName(xmlPath);
                string tempXmlDirectory = Path.GetDirectoryName(xmlPath); // Dossier du XML temporaire
                string xmlFileName = Path.GetFileName(xmlPath);
                string xmlUrl = new Uri(Path.GetFullPath(xmlPath)).AbsoluteUri;

                progress.Report(10, "Génération du chemin de sortie...");

                string pdfPath = outputPdfPath; // Utilise directement le chemin fourni

                string outputDir = Path.GetDirectoryName(pdfPath);
                if (!string.IsNullOrWhiteSpace(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                progress.Report(15, "Préparation des arguments Chrome...");
                var arguments = ChromeArguments.GetChromeArguments(
                    Path.GetFullPath(pdfPath),
                    xmlUrl,
                    chromeProfile
                );

                logger?.Log("Chemin PDF absolu : " + Path.GetFullPath(pdfPath), LogLevel.Debug);

                progress.Report(25, "Configuration du processus Chrome...");

                string chromeDir = Path.GetDirectoryName(chromePath);

                // Logs stdout et stderr dans le dossier de Chrome portable
                string logFile = Path.Combine(chromeDir, "chrome-out.log");
                string errorFile = Path.Combine(chromeDir, "chrome-err.log");

                // Écraser anciens logs
                if (File.Exists(logFile)) File.Delete(logFile);
                if (File.Exists(errorFile)) File.Delete(errorFile);

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = chromePath,
                    Arguments = string.Join(" ", arguments),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    WorkingDirectory = chromeDir
                };

                progress.Report(30, "Lancement de Chrome...");
                logger?.Log("Chrome command: " + startInfo.FileName + " " + startInfo.Arguments, LogLevel.Debug);

                logger?.Log($"Espace disque disponible: {new DriveInfo(Path.GetPathRoot(pdfPath)).AvailableFreeSpace / (1024 * 1024 * 1024)} GB", LogLevel.Debug);
                logger?.Log($"Taille du fichier XML: {new FileInfo(xmlPath).Length / (1024 * 1024)} MB", LogLevel.Debug);
                logger?.Log($"Profil Chrome: {chromeProfile}", LogLevel.Debug);

                using (var process = Process.Start(startInfo))
                {
                    var outputTask = Task.Run(async () =>
                    {
                        using (var outputWriter = new StreamWriter(logFile, false))
                        {
                            string line;
                            while ((line = await process.StandardOutput.ReadLineAsync()) != null)
                            {
                                await outputWriter.WriteLineAsync($"[{DateTime.Now:HH:mm:ss}] OUT: {line}");
                                outputWriter.Flush();
                            }
                        }
                    });

                    var errorTask = Task.Run(async () =>
                    {
                        using (var errorWriter = new StreamWriter(errorFile, false))
                        {
                            string line;
                            while ((line = await process.StandardError.ReadLineAsync()) != null)
                            {
                                await errorWriter.WriteLineAsync($"[{DateTime.Now:HH:mm:ss}] ERR: {line}");
                                errorWriter.Flush();
                            }
                        }
                    });

                    progress.Report(40, "Conversion en cours...");

                    // NOUVEAU : Surveiller les crashes
                    bool processExitedNormally = false;

                    // Progression plus fluide pendant l'attente
                    int elapsed = 0;
                    const int checkInterval = 300; // Vérifier toutes les 300ms
                    const int maxWaitTime = 3600000;

                    try
                    {
                        while (!process.WaitForExit(checkInterval) && elapsed < maxWaitTime)
                        {
                            elapsed += checkInterval;
                            // Progression de 40% à 70% de manière fluide
                            int progressPercent = Math.Min(65, 40 + (elapsed * 30 / maxWaitTime));
                            progress.Report(progressPercent, $"Conversion en cours... ({elapsed / 1000}s)");
                        }
                        processExitedNormally = true;
                    }
                    catch (Exception ex)
                    {
                        logger?.Log($"❌ Exception durant l'attente Chrome: {ex.Message}", LogLevel.Error);
                    }

                    process.WaitForExit();
                    await Task.WhenAll(outputTask, errorTask);

                    progress.Report(70, "Vérification du statut Chrome...");

                    int exitCode = process.ExitCode;
                    logger?.Log($"Chrome terminé avec le code : {exitCode}", LogLevel.Debug);

                    // Vérification immédiate si PDF créé
                    if (!File.Exists(pdfPath))
                    {
                        logger.Log("❌ Chrome terminé normalement mais aucun PDF créé", LogLevel.Error);
                        logger.Log("Possible: Chrome a échoué silencieusement durant la transformation", LogLevel.Error);
                        return false;
                    }

                    progress.Report(73, "Vérification immédiate post-Chrome...");

                    // Vérification immédiate si le PDF a été créé
                    if (File.Exists(pdfPath))
                    {
                        FileInfo immediateCheck = new FileInfo(pdfPath);
                        if (immediateCheck.Length > 0)
                        {
                            logger?.Log($"✓ PDF créé immédiatement par Chrome ({immediateCheck.Length} octets)", LogLevel.Info);
                        }
                        else
                        {
                            logger?.Log("⚠ PDF créé mais vide immédiatement après Chrome", LogLevel.Warning);
                        }
                    }
                    else
                    {
                        logger?.Log("❌ PROBLÈME: Aucun PDF créé par Chrome", LogLevel.Error);
                        logger?.Log("Chrome semble avoir échoué silencieusement", LogLevel.Error);
                        return false; // Arrêter ici au lieu de continuer
                    }

                    progress.Report(75, "Finalisation de la conversion...");

                    progress.Report(85, "Attente de la création du PDF...");
                    logger?.Log("Vérification du fichier PDF à : " + pdfPath, LogLevel.Debug);

                    // Attendre que le fichier PDF soit créé (max 60 secondes)
                    bool pdfFound = false;
                    for (int i = 0; i < 60; i++)
                    {
                        if (File.Exists(pdfPath))
                        {
                            pdfFound = true;
                            break;
                        }

                        if (i % 5 == 0) // Log toutes les 5 secondes
                        {
                            progress.Report(85 + i / 6, $"Attente PDF... ({i}s)");
                            logger?.Log($"Attente PDF... ({i}s)", LogLevel.Debug);
                        }

                        Thread.Sleep(1000);
                    }

                    if (!pdfFound)
                    {
                        logger.Log("Fichier PDF non généré après 60 secondes d'attente", LogLevel.Error);
                        return false;
                    }

                    progress.Report(95, "Validation et stabilisation du fichier PDF...");

                    // Attendre que le fichier soit complètement écrit et stable
                    long lastSize = 0;
                    int stableCount = 0;

                    for (int i = 0; i < 15; i++) // Max 15 secondes
                    {
                        FileInfo fi = new FileInfo(pdfPath);
                        long currentSize = fi.Length;

                        if (currentSize == 0)
                        {
                            logger.Log("PDF généré mais vide", LogLevel.Error);
                            return false;
                        }

                        if (currentSize == lastSize && currentSize > 0)
                        {
                            stableCount++;
                            if (stableCount >= 3) // Stable pendant 3 secondes
                            {
                                logger.Log($"PDF stabilisé ({currentSize} octets)", LogLevel.Debug);
                                break;
                            }
                        }
                        else
                        {
                            stableCount = 0;
                            logger.Log($"PDF en cours d'écriture... ({currentSize} octets)", LogLevel.Debug);
                        }

                        lastSize = currentSize;
                        Thread.Sleep(1000);
                    }

                    FileInfo finalInfo = new FileInfo(pdfPath);

                    stopwatch.Stop();
                    logger.Log($"PDF généré en {stopwatch.ElapsedMilliseconds}ms ({finalInfo.Length} octets)", LogLevel.Info);
                    logger.Log($"Fichier PDF sauvegardé : {pdfPath}", LogLevel.Info);
                    progress.Report(100, "Conversion terminée avec succès !");
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Log("Exception : " + ex.Message, LogLevel.Error);
                progress.Report(0, "Erreur lors de la conversion");
                return false;
            }
            finally
            {
                // Attendre un peu plus si le PDF existe pour s'assurer qu'il est complètement écrit
                try
                {
                    if (File.Exists(outputPdfPath) && new FileInfo(outputPdfPath).Length > 0)
                    {
                        Thread.Sleep(2000); // 2 secondes supplémentaires
                        logger?.Log("Attente sécuritaire terminée", LogLevel.Debug);
                    }
                }
                catch { }

                // Nettoyage du profil Chrome temporaire
                try
                {
                    if (Directory.Exists(chromeProfile))
                    {
                        Directory.Delete(chromeProfile, true);
                        logger?.Log("Profil Chrome nettoyé", LogLevel.Debug);
                    }
                }
                catch (Exception ex)
                {
                    logger?.Log($"Erreur nettoyage profil: {ex.Message}", LogLevel.Warning);
                }

                // Nettoyage du XML temporaire seulement
                try
                {
                    if (File.Exists(xmlPath) && Path.GetFileName(xmlPath).StartsWith("preprocessed_"))
                    {
                        File.Delete(xmlPath);
                        logger?.Log("XML temporaire nettoyé", LogLevel.Debug);
                    }
                }
                catch (Exception ex)
                {
                    logger?.Log($"Erreur nettoyage XML: {ex.Message}", LogLevel.Warning);
                }
            }
        }
    }
}
