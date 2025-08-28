using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using XmlToPdfConverter.Core.Configuration;
using XmlToPdfConverter.Core.Engine;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Services
{
    public class ChromeConversionService : IConversionService, IDisposable
    {
        private readonly ChromeConfiguration _chromeConfig;
        private readonly AppConfiguration _appConfig;
        private readonly IResourceManager _resourceManager;
        private readonly ILogger _logger;
        private readonly IXmlPreprocessor _preprocessor;

        public bool IsAvailable => _chromeConfig.IsAvailable;

        public ChromeConversionService(
            ILogger logger,
            AppConfiguration appConfig = null,
            IResourceManager resourceManager = null,
            IXmlPreprocessor preprocessor = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appConfig = appConfig ?? new AppConfiguration();
            _resourceManager = resourceManager ?? new ResourceManager(logger);
            _preprocessor = preprocessor ?? new XmlPreprocessor();
            _chromeConfig = new ChromeConfiguration(logger, _resourceManager, _appConfig);
        }

        public async Task<ConversionResult> ConvertAsync(
            string xmlPath,
            string xslPath,
            string outputPath,
            IProgress<ConversionProgress> progress = null)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new ConversionResult();

            try
            {
                // Validation des entrées
                if (!ValidateInputs(xmlPath, xslPath, outputPath))
                {
                    result.ErrorMessage = "Validation des entrées échouée";
                    return result;
                }                

                // Prétraitement XML
                string preprocessedXml = _preprocessor.Preprocess(xmlPath, xslPath, _logger);
                _resourceManager.RegisterTemporaryFile(preprocessedXml);                

                // Conversion Chrome
                bool success = await ConvertWithChromeAsync(
                    preprocessedXml,
                    outputPath,
                    progress,
                    stopwatch);

                if (success && File.Exists(outputPath))
                {
                    var fileInfo = new FileInfo(outputPath);
                    result.Success = true;
                    result.OutputPath = outputPath;
                    result.FileSizeBytes = fileInfo.Length;
                    result.Duration = stopwatch.Elapsed;
                }
                else
                {
                    result.ErrorMessage = "Chrome n'a pas généré le fichier PDF";
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                _logger.Log($"❌ Erreur conversion: {ex.Message}", LogLevel.Error);
            }
            finally
            {
                stopwatch.Stop();
                result.Duration = stopwatch.Elapsed;

                // Nettoyer les fichiers temporaires
                try
                {
                    _resourceManager.CleanupAll();
                }
                catch (Exception ex)
                {
                    _logger.Log($"⚠ Erreur nettoyage fichiers temporaires: {ex.Message}", LogLevel.Warning);
                }                
            }
            return result;
        }

        private async Task<bool> ConvertWithChromeAsync(
            string xmlPath,
            string outputPath,
            IProgress<ConversionProgress> progress,
            Stopwatch totalStopwatch)
        {
            try
            {
                string xmlUrl = new Uri(Path.GetFullPath(xmlPath)).AbsoluteUri;

                // Créer le répertoire de sortie si nécessaire
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = _chromeConfig.ChromePath,
                    Arguments = string.Join(" ", _appConfig.Chrome.GetArguments(
                        outputPath, xmlUrl, _chromeConfig.ProfilePath)),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                _logger.Log($"🚀 Lancement Chrome: {startInfo.FileName}", LogLevel.Debug);

                using (var process = Process.Start(startInfo))
                {
                    _resourceManager.RegisterProcess(process.Id);

                    var progressStopwatch = Stopwatch.StartNew();
                    var lastProgressReport = DateTime.MinValue;

                    // Surveillance du processus avec progress
                    while (!process.HasExited)
                    {
                        if (process.WaitForExit(_appConfig.Conversion.ProgressUpdateIntervalMs))
                        {
                            break;
                        }

                        if ((DateTime.Now - lastProgressReport).TotalSeconds >= 15)
                        {
                            lastProgressReport = DateTime.Now;
                            var elapsed = progressStopwatch.Elapsed;

                            int chromeProgress = Math.Min(60, 15 + (int)(elapsed.TotalSeconds * 2));
                            progress?.Report(new ConversionProgress
                            {
                                Percentage = chromeProgress,
                                CurrentStep = "Chrome traite le document...",
                                Elapsed = totalStopwatch.Elapsed
                            });

                            _logger.Log($"⏳ Chrome en cours... ({FormatDuration(elapsed)})", LogLevel.Info);
                        }
                    }

                    // Vérifier le code de sortie
                    var exitCode = process.ExitCode;
                    if (exitCode != 0)
                    {
                        _logger.Log($"❌ Chrome terminé avec erreur (code: {exitCode})", LogLevel.Error);
                        return false;
                    }

                    progress?.Report(new ConversionProgress
                    {
                        Percentage = 75,
                        CurrentStep = "Vérification du PDF...",
                        Elapsed = totalStopwatch.Elapsed
                    });

                    // Attendre la création du fichier PDF
                    return await WaitForPdfCreationAsync(outputPath, progress, totalStopwatch);
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"💥 Erreur lors de l'exécution Chrome: {ex.Message}", LogLevel.Error);
                return false;
            }
        }

        private async Task<bool> WaitForPdfCreationAsync(string pdfPath, IProgress<ConversionProgress> progress, Stopwatch totalStopwatch)
        {
            var waitStopwatch = Stopwatch.StartNew();
            var maxWaitTime = TimeSpan.FromMinutes(_appConfig.Conversion.MaxWaitTimeMinutes);
            var stabilityCheck = TimeSpan.FromSeconds(_appConfig.Conversion.FileStabilityCheckSeconds);

            long lastSize = -1;
            int stableCount = 0;
            var lastProgressUpdate = DateTime.MinValue;

            // ✅ AJOUTER: Variables pour détecter la fin réelle
            bool pdfDetected = false;
            DateTime? firstDetectionTime = null;

            while (waitStopwatch.Elapsed < maxWaitTime)
            {
                if (File.Exists(pdfPath))
                {
                    var currentSize = new FileInfo(pdfPath).Length;

                    if (currentSize > 0)
                    {
                        // ✅ MODIFIER: Marquer la première détection
                        if (!pdfDetected)
                        {
                            pdfDetected = true;
                            firstDetectionTime = DateTime.Now;
                            _logger.Log($"🔍 PDF détecté ({currentSize} octets)", LogLevel.Debug);
                        }

                        if (currentSize == lastSize)
                        {
                            stableCount++;

                            // ✅ MODIFIER: Condition de stabilité plus flexible
                            int requiredStableChecks = Math.Max(2, _appConfig.Conversion.FileStabilityCheckSeconds);

                            if (stableCount >= requiredStableChecks)
                            {                              
                                progress?.Report(new ConversionProgress
                                {
                                    Percentage = 95,
                                    CurrentStep = "PDF généré, validation finale...",
                                    Elapsed = totalStopwatch.Elapsed
                                });

                                return await ValidatePdfFile(pdfPath, currentSize);
                            }
                        }                        
                    }
                    else
                    {
                        // ✅ AJOUTER: Gérer les fichiers de taille 0
                        _logger.Log("⚠ PDF détecté mais vide, attente...", LogLevel.Debug);
                    }
                }
                else if (pdfDetected)
                {
                    // ✅ AJOUTER: Si le PDF était détecté mais n'existe plus
                    _logger.Log("⚠ PDF disparu, attente de régénération...", LogLevel.Warning);
                    pdfDetected = false;
                    firstDetectionTime = null;
                }

                // Progress update avec estimation plus précise
                if ((DateTime.Now - lastProgressUpdate).TotalSeconds >= 5) 
                {
                    lastProgressUpdate = DateTime.Now;

                    // ✅ MODIFIER: Progression plus intelligente
                    int progressPercent = CalculateWaitProgress(waitStopwatch.Elapsed, maxWaitTime, pdfDetected, currentSize: lastSize);

                    string statusMessage = pdfDetected && lastSize > 0
                        ? $"Stabilisation PDF... ({lastSize:N0} octets, {stableCount}/{_appConfig.Conversion.FileStabilityCheckSeconds}s)"
                        : "Attente génération PDF...";

                    progress?.Report(new ConversionProgress
                    {
                        Percentage = progressPercent,
                        CurrentStep = statusMessage,
                        Elapsed = totalStopwatch.Elapsed
                    });
                }
                await Task.Delay(1000);
            }

            _logger.Log($"❌ Timeout: PDF non stabilisé après {maxWaitTime.TotalMinutes} minutes", LogLevel.Error);
            return false;
        }

        // ✅ AJOUTER: Nouvelle méthode pour calculer la progression d'attente
        private int CalculateWaitProgress(TimeSpan elapsed, TimeSpan maxWait, bool pdfDetected, long currentSize)
        {
            // ✅ SIMPLIFIER: Base 75% + progression linéaire
            int baseProgress = 75;

            if (pdfDetected)
            {
                baseProgress = 85; // PDF détecté = 85%
                if (currentSize > 1024)
                {
                    baseProgress = 90; // PDF avec contenu = 90%
                }
            }

            // Progression temporelle simple (max 10% supplémentaires)
            int timeProgress = (int)((elapsed.TotalSeconds / 30.0) * 10); // 10% sur 30 secondes

            return Math.Min(95, baseProgress + timeProgress);
        }

        // ✅ AJOUTER: Nouvelle méthode de validation finale
        private async Task<bool> ValidatePdfFile(string pdfPath, long expectedSize)
        {
            try
            {
                // Attendre un peu pour s'assurer que le fichier est complètement écrit
                await Task.Delay(500);

                if (!File.Exists(pdfPath))
                {
                    _logger.Log("❌ PDF disparu lors de la validation finale", LogLevel.Error);
                    return false;
                }

                var finalInfo = new FileInfo(pdfPath);

                // Vérification de taille minimale (un PDF valide fait au moins quelques KB)
                if (finalInfo.Length < 1024)
                {
                    _logger.Log($"❌ PDF trop petit ({finalInfo.Length} octets), probablement corrompu", LogLevel.Error);
                    return false;
                }

                // Vérification que la taille n'a pas changé (stabilité confirmée)
                if (finalInfo.Length != expectedSize)
                {
                    _logger.Log($"⚠ Taille PDF changée pendant validation ({expectedSize} → {finalInfo.Length})", LogLevel.Warning);
                    // On accepte quand même si la différence est minime
                    if (Math.Abs(finalInfo.Length - expectedSize) > 1024)
                    {
                        return false;
                    }
                }

                _logger.Log($"✅ PDF validé: {finalInfo.Length:N0} octets", LogLevel.Info);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log($"❌ Erreur validation PDF: {ex.Message}", LogLevel.Error);
                return false;
            }
        }

        private bool ValidateInputs(string xmlPath, string xslPath, string outputPath)
        {
            if (string.IsNullOrEmpty(xmlPath) || !File.Exists(xmlPath))
            {
                _logger.Log("❌ Fichier XML manquant ou inexistant", LogLevel.Error);
                return false;
            }

            if (string.IsNullOrEmpty(xslPath) || !File.Exists(xslPath))
            {
                _logger.Log("❌ Fichier XSL manquant ou inexistant", LogLevel.Error);
                return false;
            }

            if (string.IsNullOrEmpty(outputPath))
            {
                _logger.Log("❌ Chemin de sortie non spécifié", LogLevel.Error);
                return false;
            }

            if (!IsAvailable)
            {
                _logger.Log("❌ Chrome non disponible", LogLevel.Error);
                return false;
            }

            return true;
        }

        public void Cleanup()
        {
            _resourceManager?.CleanupAll();
            _chromeConfig?.CleanupProfile();
        }

        public void Dispose()
        {
            Cleanup();
            _resourceManager?.Dispose();
            _chromeConfig?.Dispose();
        }
        private static string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalMinutes < 1)
                return $"{duration.Seconds}s";
            else if (duration.TotalHours < 1)
                return $"{duration.Minutes}min {duration.Seconds}s";
            else
                return $"{duration.Hours}h {duration.Minutes}min {duration.Seconds}s";
        }
    }
}