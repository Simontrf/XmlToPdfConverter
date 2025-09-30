using System;
using System.IO;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Configuration
{
    //Configuration et gestion du cycle de vie de chrome
    //Résolution de chemin chrome et gestion des profils
    public class ChromeConfiguration
    {
        private readonly ChromePathResolver _pathResolver;
        private readonly ILogger _logger;

        public string ChromePath { get; private set; } //Chemin complet de chrome.exe
        public string ProfilePath { get; private set; } //Chemin du profil chrome utilisé

        //Initialisation d'une nouvelle configuration chrome avec réolution automatique de chemin chrome et création de profil
        public ChromeConfiguration(ILogger logger = null, IResourceManager resourceManager = null, AppConfiguration config = null)
        {
            _logger = logger; //Logger pour tracer les opérations
            _resourceManager = resourceManager ?? new ResourceManager(logger); //Gestionnaire de ressources des profils temporaires chrome
            _config = config ?? new AppConfiguration(); //Configuration de chrome
            _pathResolver = new ChromePathResolver(logger); //Résolution du chemin chrome
            Initialize();
        }

        //Initialise la résolution de chemin chrome et profil chrome
        private void Initialize()
        {
            // Résoudre le chemin Chrome
            ChromePath = _pathResolver.GetChromeExecutablePath(); //Trouve l'executable chrome

            // Créer le profil
            CreateProfile(); //Création du profil chrome
        }

        //Fonction responsable de la création des profils
        private void CreateProfile()
        {
            if (_config.Chrome.UseTemporaryProfile)
            {
                ProfilePath = _resourceManager.CreateTemporaryProfile(); //profil temporaire
            }
            else //profil persistant
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

        //Nettoyage du profil si temporaire
        public void CleanupProfile()
        {
            if (_config.Chrome.UseTemporaryProfile)
            {
                _resourceManager?.CleanupProfile(ProfilePath);
            }
        }

        //Libère toutes les ressources (profil et ressource manager)
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