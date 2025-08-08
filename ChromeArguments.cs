using System.IO;

namespace XmlToPdfConverter.Core.Engine
{
    public static class ChromeArguments
    {
        public static string[] GetChromeArguments(string pdfPath, string xmlUrl, string profilePath)
        {
            string LogDirectory = Path.Combine(Path.GetDirectoryName(pdfPath), "logs");
            Directory.CreateDirectory(LogDirectory);

            return new string[]
            {
                "--headless",
                "--single-process",
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
                //"--disable-logging",
                //"--silent",
                //"--log-level=3",
                "--enable-logging=stderr",
                "--log-level=0",  // 0 = tout, 1 = info, 2 = warning, 3 = error
                "--v=1",          // Logs verbeux
                "--vmodule=*=1",  // Tous les modules en verbeux

                "--enable-crash-reporter",
                "--crash-dumps-dir=\"logs/crashes\"",
                "--enable-gpu-memory-buffer-video-frames",
                "--log-gpu-control-list-decisions",

                $"--log-file=\"{LogDirectory}\\chrome.log\"",
                $"--user-data-dir=\"{profilePath}\"",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-renderer-backgrounding",
                "--print-to-pdf-display-header-footer",
                "--print-to-pdf-header",
                "--print-to-pdf-footer",
                "--memory-pressure-off=true",
                "--max-heap-size=32000",
                "--js-flags=--max-old-space-size=16000",
                "--force-gpu-mem-available-mb=8192",
                "--run-all-compositor-stages-before-draw",
                "--no-first-run",
                "--disable-dev-tools",
                "--disable-features=TranslateUI",
                "--disable-features=MediaRouter",
                "--disable-component-update",
                "--disable-domain-reliability",
                "--disable-client-side-phishing-detection",
                "--disable-background-mode",
                $"--user-data-dir=\"{profilePath}\"",
                $"--print-to-pdf=\"{pdfPath}\"",
                xmlUrl
            };
        }
    }
}