namespace XmlToPdfConverter.Core.Engine
{
    public static class ChromeArguments
    {
        public static string[] GetChromeArguments(string pdfPath, string xmlUrl, string profilePath)
        {
            return new string[]
            {
                "--headless",
                //"--window-size=1920,1080",
                
                // SÉCURITÉ ET PERMISSIONS
                "--allow-running-insecure-content",
                "--allow-file-access-from-files",
                
                // OPTIMISATIONS SYSTÈME
                "--disable-dev-shm-usage",
                "--disable-background-networking",
                "--disable-default-apps",
                "--disable-extensions",
                "--disable-sync",
                "--disable-translate",
                
                // MÉMOIRE CORRIGÉE
                "--js-flags=--max-old-space-size=2048 --max-semi-space-size=256",
                "--memory-pressure-off",
                "--force-gpu-mem-available-mb=1024",
                
                // RENDU ET PERFORMANCE
                "--disable-background-timer-throttling",
                "--disable-backgrounding-occluded-windows",
                "--disable-renderer-backgrounding",
                "--run-all-compositor-stages-before-draw",
                "--disable-hang-monitor",
                
                // COULEURS (version compatible)
                "--print-backgrounds",
                
                // CONFIGURATION
                "--no-first-run",
                "--disable-component-update",
                "--disable-domain-reliability",
                "--disable-client-side-phishing-detection",
                "--disable-background-mode",
                
                // PROFIL ET SORTIE
                $"--user-data-dir=\"{profilePath}\"",
                $"--print-to-pdf=\"{pdfPath}\"",

                xmlUrl
            };
        }
    }
}