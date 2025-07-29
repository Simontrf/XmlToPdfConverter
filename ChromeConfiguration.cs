using System;
using System.IO;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Configuration
{
    public class ChromeConfiguration
    {
        private readonly ChromePathResolver _pathResolver;
        private readonly ILogger _logger;

        public string ChromePath { get; private set; }
        public string ProfilePath { get; private set; }
        public bool UseTemporaryProfile { get; set; } = true;

        public ChromeConfiguration(ILogger logger = null)
        {
            _logger = logger;
            _pathResolver = new ChromePathResolver(logger);
            Initialize();
        }

        private void Initialize()
        {
            // Résoudre le chemin Chrome
            ChromePath = _pathResolver.GetChromeExecutablePath();

            // Créer le profil
            CreateProfile();
        }

        private void CreateProfile()
        {
            if (UseTemporaryProfile)
            {
                ProfilePath = Path.Combine(Path.GetTempPath(), "chrome-profile-" + Guid.NewGuid());
            }
            else
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                ProfilePath = Path.Combine(baseDir, "chrome-profile-persistent");
            }

            try
            {
                if (!Directory.Exists(ProfilePath))
                {
                    Directory.CreateDirectory(ProfilePath);
                    _logger?.Log($"✓ Profil Chrome créé: {ProfilePath}", LogLevel.Debug);
                }
            }
            catch (Exception ex)
            {
                _logger?.Log($"⚠ Erreur création profil: {ex.Message}", LogLevel.Warning);
            }
        }

        public void CleanupProfile()
        {
            if (UseTemporaryProfile && Directory.Exists(ProfilePath))
            {
                try
                {
                    Directory.Delete(ProfilePath, true);
                    _logger?.Log("✓ Profil Chrome temporaire supprimé", LogLevel.Debug);
                }
                catch (Exception ex)
                {
                    _logger?.Log($"⚠ Erreur suppression profil: {ex.Message}", LogLevel.Warning);
                }
            }
        }

        public bool IsAvailable => _pathResolver.IsChromeAvailable();
    }
}