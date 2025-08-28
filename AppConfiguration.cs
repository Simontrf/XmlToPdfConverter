using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Configuration
{
    public class AppConfiguration
    {
        // Chrome Settings
        public ChromeSettings Chrome { get; set; } = new ChromeSettings();

        // Conversion Settings
        public ConversionSettings Conversion { get; set; } = new ConversionSettings();

        // Logging Settings
        public LoggingSettings Logging { get; set; } = new LoggingSettings();
    }

    public class ChromeSettings
    {
        public int VirtualTimeBudget { get; set; } = 30000;
        public int ProcessTimeout { get; set; } = 60000;
        public int MaxMemoryMB { get; set; } = 8192;
        public bool UseTemporaryProfile { get; set; } = true;
        public string CustomProfilePath { get; set; }

        public string[] GetArguments(string pdfPath, string xmlUrl, string profilePath)
        {
            return new[]
            {
                "--headless",
                "--no-sandbox",
                "--disable-dev-shm-usage",
        
                // ✅ RENDU COMPLET FORCÉ
                "--disable-partial-raster",
                "--disable-threaded-compositing",
                "--disable-checker-imaging",
                "--run-all-compositor-stages-before-draw",
                "--disable-background-timer-throttling",
                "--disable-renderer-backgrounding",
                "--disable-backgrounding-occluded-windows",
        
                // ✅ POLICES ET TEXTE
                "--disable-font-subpixel-positioning",
                "--enable-font-antialiasing",
                "--force-device-scale-factor=1.0",
        
                // ✅ TIMEOUT ÉTENDU
                "--virtual-time-budget=30000", // 30 secondes pour le rendu
                "--timeout=60000",
        
                // Mémoire
                "--js-flags=--max-old-space-size=8192",
                "--memory-pressure-off",
        
                // Sécurité et permissions
                "--disable-web-security",
                "--allow-file-access-from-files",
                "--allow-running-insecure-content",
        
                // PDF et couleurs
                "--print-backgrounds",
        
                // Configuration
                $"--user-data-dir=\"{profilePath}\"",
                $"--print-to-pdf=\"{pdfPath}\"",
                xmlUrl
            };
        }
    }

    public class ConversionSettings
    {
        public int FileStabilityCheckSeconds { get; set; } = 2;
        public int MaxWaitTimeMinutes { get; set; } = 60;
        public int ProgressUpdateIntervalMs { get; set; } = 300;
        public bool OpenResultAfterConversion { get; set; } = false;
        public bool CleanupTemporaryFiles { get; set; } = true;
        public string TempFilePrefix { get; set; } = "preprocessed_";
        public string ProfilePrefix { get; set; } = "chrome-profile-";
        public int PdfValidationDelayMs { get; set; } = 500;
        public long MinPdfSizeBytes { get; set; } = 1024;
        public int WaitProgressUpdateIntervalSeconds { get; set; } = 2;
        public bool UseProgressEstimation { get; set; } = true;
        public int MinEstimatedDurationSeconds { get; set; } = 5;
        public int MaxEstimatedDurationSeconds { get; set; } = 300;
        public double XmlSizeProgressFactor { get; set; } = 0.001;
        public double XslComplexityFactor { get; set; } = 0.01;
    }

    public class LoggingSettings
    {
        public LogLevel MinimumLevel { get; set; } = LogLevel.Info;
        public bool EnableDebugLogging { get; set; } = false;
        public int ProgressLogIntervalMinutes { get; set; } = 1;
    }
}