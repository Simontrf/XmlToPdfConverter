using System.IO;

namespace XmlToPdfConverter.Core.Engine
{
    public static class ChromeArguments
    {
        public static string[] GetChromeArguments(string pdfPath, string xmlUrl, string profilePath)
        {
            return new string[]
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
}