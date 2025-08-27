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

                    _logger.Log($"✅ Conversion réussie en {FormatDuration(stopwatch.Elapsed)} ({fileInfo.Length} octets)", LogLevel.Info);
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

                        // Reporter le progrès toutes les X minutes
                        if ((DateTime.Now - lastProgressReport).TotalMinutes >= _appConfig.Logging.ProgressLogIntervalMinutes)
                        {
                            lastProgressReport = DateTime.Now;
                            var elapsed = progressStopwatch.Elapsed;
                           
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

        private async Task<bool> WaitForPdfCreationAsync(
            string pdfPath,
            IProgress<ConversionProgress> progress,
            Stopwatch totalStopwatch)
        {
            var waitStopwatch = Stopwatch.StartNew();
            var maxWaitTime = TimeSpan.FromMinutes(_appConfig.Conversion.MaxWaitTimeMinutes);
            var stabilityCheck = TimeSpan.FromSeconds(_appConfig.Conversion.FileStabilityCheckSeconds);

            long lastSize = -1;
            int stableCount = 0;
            var lastProgressUpdate = DateTime.MinValue;

            while (waitStopwatch.Elapsed < maxWaitTime)
            {
                if (File.Exists(pdfPath))
                {
                    var currentSize = new FileInfo(pdfPath).Length;

                    if (currentSize > 0)
                    {
                        if (currentSize == lastSize)
                        {
                            stableCount++;
                            if (stableCount >= _appConfig.Conversion.FileStabilityCheckSeconds)
                            {
                                _logger.Log($"✓ PDF stabilisé ({currentSize} octets)", LogLevel.Debug);
                                return true;
                            }
                        }
                        else
                        {
                            stableCount = 0;
                            lastSize = currentSize;
                            _logger.Log($"📄 PDF en croissance... ({currentSize} octets)", LogLevel.Debug);
                        }
                    }
                }

                // Progress update
                if ((DateTime.Now - lastProgressUpdate).TotalSeconds >= 5)
                {
                    lastProgressUpdate = DateTime.Now;
                    var progressPercent = 75 + Math.Min(20, (int)((waitStopwatch.Elapsed.TotalMinutes / maxWaitTime.TotalMinutes) * 20));

                    progress?.Report(new ConversionProgress
                    {
                        Percentage = progressPercent,
                        CurrentStep = $"Attente PDF... ({FormatDuration(waitStopwatch.Elapsed)})",
                        Elapsed = totalStopwatch.Elapsed
                    });
                }

                await Task.Delay(1000);
            }

            _logger.Log($"❌ Timeout: PDF non créé après {maxWaitTime.TotalMinutes} minutes", LogLevel.Error);
            return false;
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