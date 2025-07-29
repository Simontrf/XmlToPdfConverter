using System;
using System.IO;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Configuration
{
    public class ChromePathResolver : IChromePathResolver 
    {
        private readonly ILogger _logger;
        private string _cachedChromePath;

        public ChromePathResolver(ILogger logger = null)
        {
            _logger = logger;
        }

        public string GetChromeExecutablePath()
        {
            if (!string.IsNullOrEmpty(_cachedChromePath))
                return _cachedChromePath;

            _cachedChromePath = FindChromeExecutable();
            return _cachedChromePath;
        }

        public bool IsChromeAvailable()
        {
            try
            {
                string chromePath = GetChromeExecutablePath();
                return !string.IsNullOrEmpty(chromePath) && File.Exists(chromePath);
            }
            catch
            {
                return false;
            }
        }

        private string FindChromeExecutable()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _logger?.Log($"Recherche de Chrome depuis : {baseDirectory}", LogLevel.Debug);

            // Stratégie 1: Chemin relatif standard (../../chrome/bin/chrome.exe)
            string standardPath = GetStandardChromePath(baseDirectory);
            if (File.Exists(standardPath))
            {
                _logger?.Log($"Chrome trouvé (chemin standard) : {standardPath}", LogLevel.Debug);
                return standardPath;
            }

            // Stratégie 2: Recherche récursive vers le haut
            string recursivePath = FindChromeRecursively(baseDirectory);
            if (!string.IsNullOrEmpty(recursivePath))
            {
                _logger?.Log($"Chrome trouvé (recherche récursive) : {recursivePath}", LogLevel.Debug);
                return recursivePath;
            }

            // Stratégie 3: Recherche dans le dossier Core (si exécuté depuis Core)
            string corePath = FindChromeInCore(baseDirectory);
            if (!string.IsNullOrEmpty(corePath))
            {
                _logger?.Log($"Chrome trouvé (dossier Core) : {corePath}", LogLevel.Debug);
                return corePath;
            }

            string errorMessage = $"Chrome non trouvé. Chemins tentés :\n" +
                                $"- Chemin standard : {standardPath}\n" +
                                $"- Recherche récursive depuis : {baseDirectory}\n" +
                                $"- Dossier Core : {Path.Combine(baseDirectory, "chrome", "bin", "chrome.exe")}";

            _logger?.Log(errorMessage, LogLevel.Error);
            throw new FileNotFoundException("Chrome portable non trouvé", errorMessage);
        }

        private string GetStandardChromePath(string baseDirectory)
        {
            // Depuis CLI ou GUI: ../../chrome/bin/chrome.exe
            string parentDirectory = Directory.GetParent(baseDirectory)?.FullName;
            if (parentDirectory != null)
            {
                string solutionDirectory = Directory.GetParent(parentDirectory)?.FullName;
                if (solutionDirectory != null)
                {
                    return Path.Combine(solutionDirectory, "XmlToPdfConverter.Core", "chrome", "bin", "chrome.exe");
                }
            }
            return string.Empty;
        }

        private string FindChromeRecursively(string startDirectory)
        {
            DirectoryInfo currentDir = new DirectoryInfo(startDirectory);

            // Remonter maximum 4 niveaux pour éviter une recherche infinie
            for (int level = 0; level < 4 && currentDir != null; level++)
            {
                // Chercher dans le dossier actuel
                string chromePath = Path.Combine(currentDir.FullName, "XmlToPdfConverter.Core", "chrome", "bin", "chrome.exe");
                if (File.Exists(chromePath))
                {
                    return chromePath;
                }

                // Chercher directement un dossier chrome
                string directChromePath = Path.Combine(currentDir.FullName, "chrome", "bin", "chrome.exe");
                if (File.Exists(directChromePath))
                {
                    return directChromePath;
                }

                currentDir = currentDir.Parent;
            }

            return null;
        }

        private string FindChromeInCore(string baseDirectory)
        {
            // Si exécuté depuis le dossier Core lui-même
            string corePath = Path.Combine(baseDirectory, "chrome", "bin", "chrome.exe");
            if (File.Exists(corePath))
            {
                return corePath;
            }

            return null;
        }
    }
}