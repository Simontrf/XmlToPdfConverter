using System;
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
                "--disable-gpu",
                "--no-sandbox",
                "--disable-dev-shm-usage",
                "--aggressive-cache-discard",
                "--disable-background-networking",
                "--disable-default-apps",
                "--disable-extensions",
                "--disable-sync",
                "--disable-translate",
                "--disable-ipc-flooding-protection",
                "--disable-web-security",
                "--allow-running-insecure-content",
                "--allow-file-access-from-files",
                "--disable-features=VizDisplayCompositor",
                "--disable-logging",
                "--silent",
                "--log-level=3",
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-renderer-backgrounding",
                "--virtual-time-budget=3000",
                "--print-to-pdf-display-header-footer",
                "--print-to-pdf-header",
                "--print-to-pdf-footer",
                "--memory-pressure-off",
                "--max_old_space_size=4096",
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