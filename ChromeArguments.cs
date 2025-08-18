using System.IO;

namespace XmlToPdfConverter.Core.Engine
{
    public static class ChromeArguments
    {
        public static string[] GetChromeArguments(string pdfPath, string xmlUrl, string profilePath)
        {
            // Récupère le dossier où se trouve chrome.exe portable
            string chromeDir = Path.GetDirectoryName(profilePath);
            if (string.IsNullOrEmpty(chromeDir) || !Directory.Exists(chromeDir))
                chromeDir = Directory.GetCurrentDirectory();

            // Fichier de log interne Chrome (sera écrasé à chaque run)
            string chromeDebugLog = Path.Combine(chromeDir, "chrome_debug.log");

            return new string[]
            {
                "--headless",
                "--disable-gpu",
                "--disable-accelerated-2d-canvas",
                "--disable-dev-shm-usage",
                "--disable-background-networking",
                "--disable-default-apps",
                "--disable-extensions",
                "--disable-sync",
                "--disable-translate",
                "--disable-ipc-flooding-protection",
                "--allow-running-insecure-content",
                "--allow-file-access-from-files",
                "--disable-features=VizDisplayCompositor",
                "--enable-logging=stderr",
                "--log-level=0",
                "--v=1",
                "--vmodule=*=1",
                $"--log-file={chromeDebugLog}",
                $"--user-data-dir=\"{profilePath}\"",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-renderer-backgrounding",
                "--print-to-pdf-display-header-footer",
                "--print-to-pdf-header",
                "--print-to-pdf-footer",
                "--memory-pressure-off=true",
                "--max-heap-size=32000",
                "--js-flags=--max-old-space-size=32000",
                //"--force-gpu-mem-available-mb=16384",
                "--disable-software-rasterizer",
                "--run-all-compositor-stages-before-draw",
                "--no-first-run",
                //"--disable-dev-tools",
                "--disable-component-update",
                "--disable-domain-reliability",
                "--disable-client-side-phishing-detection",
                "--disable-background-mode",
                $"--print-to-pdf=\"{pdfPath}\"",
                xmlUrl
            };
        }
    }
}