using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Engine
{
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

        public bool Convert(string xmlPath, string outputPdfPath, IProgressReporter progress, ILogger logger)
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
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = chromePath,
                    Arguments = string.Join(" ", arguments),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    WorkingDirectory = Path.GetDirectoryName(chromePath),
                };

                progress.Report(30, "Lancement de Chrome...");
                logger?.Log("Chrome command: " + startInfo.FileName + " " + startInfo.Arguments, LogLevel.Debug);

                using (var process = Process.Start(startInfo))
                {
                    Task<string> errorTask = process.StandardError.ReadToEndAsync();
                    Task<string> outputTask = process.StandardOutput.ReadToEndAsync();

                    progress.Report(40, "Conversion en cours...");

                    // Progression plus fluide pendant l'attente
                    int elapsed = 0;
                    const int checkInterval = 300; // Vérifier toutes les 300ms
                    const int maxWaitTime = 600000;

                    while (!process.WaitForExit(checkInterval) && elapsed < maxWaitTime)
                    {
                        elapsed += checkInterval;
                        // Progression de 40% à 70% de manière fluide
                        int progressPercent = Math.Min(70, 40 + (elapsed * 30 / maxWaitTime));
                        progress.Report(progressPercent, $"Conversion en cours... ({elapsed / 1000}s)");
                    }

                    progress.Report(75, "Finalisation de la conversion...");

                    string stderr = errorTask.Result;
                    if (!string.IsNullOrWhiteSpace(stderr))
                    {
                        logger.Log("Chrome stderr complet :", LogLevel.Debug);
                        logger.Log(stderr, LogLevel.Debug);

                        var errors = stderr.Split('\n')
                            .Where(line => !string.IsNullOrWhiteSpace(line) &&
                                           !line.ToLower().Contains("devtools") &&
                                           !line.ToLower().Contains("extension") &&
                                           !line.ToLower().Contains("renderer"))
                            .Take(5)
                            .ToArray();

                        if (errors.Any())
                        {
                            logger.Log("Erreurs Chrome détectées : " + string.Join(" | ", errors), LogLevel.Warning);
                        }
                    }

                    progress.Report(85, "Vérification du fichier PDF...");
                    logger?.Log("Vérification du fichier PDF à : " + pdfPath, LogLevel.Debug);

                    if (!File.Exists(pdfPath))
                    {
                        logger.Log("Fichier PDF non généré", LogLevel.Error);
                        return false;
                    }

                    progress.Report(95, "Validation du fichier PDF...");
                    FileInfo fi = new FileInfo(pdfPath);
                    if (fi.Length == 0)
                    {
                        logger.Log("PDF généré mais vide", LogLevel.Error);
                        return false;
                    }

                    stopwatch.Stop();
                    logger.Log($"PDF généré en {stopwatch.ElapsedMilliseconds}ms ({fi.Length} octets)", LogLevel.Info);
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
                // Nettoyage du profil Chrome temporaire
                try
                {
                    if (Directory.Exists(chromeProfile))
                        Directory.Delete(chromeProfile, true);
                }
                catch { }

                // Nettoyage du XML temporaire seulement
                try
                {
                    if (File.Exists(xmlPath) && Path.GetFileName(xmlPath).StartsWith("preprocessed_"))
                    {
                        File.Delete(xmlPath);
                    }
                }
                catch
                {

                }
            }
        }
    }
}
