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

        public ChromeConfiguration(ILogger logger = null, IResourceManager resourceManager = null, AppConfiguration config = null)
        {
            _logger = logger;
            _resourceManager = resourceManager ?? new ResourceManager(logger);
            _config = config ?? new AppConfiguration();
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
            if (_config.Chrome.UseTemporaryProfile)
            {
                ProfilePath = _resourceManager.CreateTemporaryProfile();
            }
            else
            {
                string baseDir = !string.IsNullOrEmpty(_config.Chrome.CustomProfilePath)
                    ? _config.Chrome.CustomProfilePath
                    : AppDomain.CurrentDomain.BaseDirectory;
                ProfilePath = Path.Combine(baseDir, "chrome-profile-persistent");

                try
                {
                    if (!Directory.Exists(ProfilePath))
                    {
                        Directory.CreateDirectory(ProfilePath);
                        _logger?.Log($"✓ Profil Chrome persistant créé: {ProfilePath}", LogLevel.Debug);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.Log($"⚠ Erreur création profil: {ex.Message}", LogLevel.Warning);
                }
            }
        }

        public void CleanupProfile()
        {
            if (_config.Chrome.UseTemporaryProfile)
            {
                _resourceManager?.CleanupProfile(ProfilePath);
            }
        }

        public void Dispose()
        {
            CleanupProfile();
            _resourceManager?.Dispose();
        }
        public bool IsAvailable => _pathResolver.IsChromeAvailable();

        private readonly IResourceManager _resourceManager;
        private readonly AppConfiguration _config;
    }

}