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
        public bool UseTemporaryProfile { get; set; } = true;
        public string CustomProfilePath { get; set; }

        public string[] GetArguments(string pdfPath, string xmlUrl, string profilePath)
        {
            return new[]
            {
                "--headless", // Mode sans interface graphique
                "--no-sandbox", // Désactive le bac à sable sécurisé
                "--disable-dev-shm-usage", // Evite problèmes mémoire sur certains OS

                // Rendu complet des pages, pas partiel
                "--disable-partial-raster",
                "--disable-threaded-compositing", // Désactive le rendu multi-threadé
                "--disable-checker-imaging", // Désactive l'accélération image
                "--run-all-compositor-stages-before-draw", // Force le rendu complet avant affichage
                "--disable-background-timer-throttling", // Timers toujours actifs, même en arrière plan
                "--disable-renderer-backgrounding", // Empêche la pause du rendu en arrière plan
                "--disable-backgrounding-occluded-windows", // Désactive la pause pour fenêtres masquées

                // Polices et texte
                "--disable-font-subpixel-positioning", // Désactive la position subpixel
                "--enable-font-antialiasing", // Active l'anticrénelage des polices
                "--force-device-scale-factor=1.0", // Force l'échelle à 1:1

                // Timeout et temporisation
                "--virtual-time-budget=30000", // Budget de temps virtuel 30s
                "--timeout=60000", // Timeout après 60 secondes

                // Mémoire
                "--js-flags=--max-old-space-size=8192", // Limite JS à 8Go de RAM
                "--memory-pressure-off", // Ignore la pression mémoire

                // Sécurité et accès fichiers
                "--disable-web-security", // (Obligatoire) Désactive la politique de sécurité navigateur, permet de charger un document XML
                "--allow-file-access-from-files", // Autorise l'accès aux fichiers locaux
                "--allow-running-insecure-content", // Autorise le contenu non sécurisé

                // Impression PDF et couleurs
                "--print-backgrounds", // Imprime les fonds/arrières plans

                // Chemins de configuration
                $"--user-data-dir=\"{profilePath}\"", // Répertoire du profil Chrome
                $"--print-to-pdf=\"{pdfPath}\"", // Chemin du PDF en sortie
                xmlUrl // URL du document XML à charger
            };
        }
    }

    public class ConversionSettings
    {
        public int FileStabilityCheckSeconds { get; set; } = 3;
        public int MaxWaitTimeMinutes { get; set; } = 60;
        public int ProgressUpdateIntervalMs { get; set; } = 300;
        public bool OpenResultAfterConversion { get; set; } = false;
    }

    public class LoggingSettings
    {
        public bool EnableDebugLogging { get; set; } = false;
        public int ProgressLogIntervalMinutes { get; set; } = 1;
    }
}