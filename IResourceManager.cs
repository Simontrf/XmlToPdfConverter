using System;
using System.Collections.Generic;

namespace XmlToPdfConverter.Core.Interfaces
{
    public interface IResourceManager : IDisposable
    {
        string CreateTemporaryProfile(); //Création d'un profil temporaire
        void RegisterTemporaryFile(string filePath); //Enregistrement du fichier XML prétraité pour nettoyage futur
        void RegisterProcess(int processId); //Enregistrement de l'ID du processus Chrome pour surveillance et nettoyage
        void CleanupAll(); //Nettoyage de toutes les ressources 
        void CleanupProfile(string profilePath); //Nettoyage du profil chrome rensaigner via profilePath
    }

    //Gestionnaire de ressources temporaires pour Chrome, centralise la gestion des profils, fichiers temporaires et processus.
    public class ResourceManager : IResourceManager
    {
        //Collections des ressopurces à gérer
        private readonly List<string> _temporaryFiles = new List<string>();
        private readonly List<string> _temporaryProfiles = new List<string>();
        private readonly List<int> _processes = new List<int>();
        private readonly ILogger _logger;
        private bool _disposed = false;

        //Initialise le gestionnaire de ressources
        public ResourceManager(ILogger logger = null)
        {
            _logger = logger; //Logger pour tracer les opérations
        }

        //Crée un profil Chrome temporaire unique dans le dossier système temp, automatiquement enregistré pour nettoyage ultérieur.
        public string CreateTemporaryProfile()
        {
            string profilePath = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                $"chrome-profile-{Guid.NewGuid()}");

            System.IO.Directory.CreateDirectory(profilePath);
            _temporaryProfiles.Add(profilePath);
            _logger?.Log($"✓ Profil temporaire créé: {profilePath}", LogLevel.Debug);

            return profilePath; //Chemin complet du rpofil créé
        }

        //Enregistre un fichier temporaire pour nettoyage automatique
        public void RegisterTemporaryFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                _temporaryFiles.Add(filePath);
            }
        }

        //Enregistre un processus pour surveillance et arrêt potentiel
        public void RegisterProcess(int processId)
        {
            _processes.Add(processId);
        }


        //Nettoyage du profil chrome
        public void CleanupProfile(string profilePath)
        {
            try
            {
                if (System.IO.Directory.Exists(profilePath))
                {
                    //Suppression récursive
                    System.IO.Directory.Delete(profilePath, true);
                    _temporaryProfiles.Remove(profilePath);
                    _logger?.Log("✓ Profil Chrome nettoyé", LogLevel.Debug);
                }
            }
            catch (Exception ex)
            {
                //Ne bloque pas si le nettoyage échoue
                _logger?.Log($"⚠ Erreur nettoyage profil: {ex.Message}", LogLevel.Warning);
            }
        }

        //Nettoie toutes les ressources, fichiers temporaires, profils, processus 
        public void CleanupAll()
        {
            //Nettoyer les fichiers temporaires
            foreach (string file in _temporaryFiles.ToArray())
            {
                try
                {
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                        _logger?.Log($"✓ Fichier temporaire supprimé: {file}", LogLevel.Debug);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.Log($"⚠ Erreur suppression fichier: {ex.Message}", LogLevel.Warning);
                }
            }
            _temporaryFiles.Clear();

            //Nettoyer les profils
            foreach (string profile in _temporaryProfiles.ToArray())
            {
                CleanupProfile(profile);
            }

            //Nettoyer les processus
            foreach (int processId in _processes.ToArray())
            {
                try
                {
                    var process = System.Diagnostics.Process.GetProcessById(processId);
                    if (!process.HasExited)
                    {
                        process.Kill();
                        _logger?.Log($"✓ Processus {processId} terminé", LogLevel.Debug);
                    }
                }
                catch (ArgumentException)
                {
                    //Le processus n'existe plus, c'est normal
                    _logger?.Log($"✓ Processus {processId} s'est terminé normalement", LogLevel.Debug);
                }
                catch (Exception ex)
                {
                    _logger?.Log($"⚠ Erreur arrêt processus {processId}: {ex.Message}", LogLevel.Warning);
                }
            }
            _processes.Clear();
        }

        //Libère toutes les ressources (implémentation IDisposable), appelle CleanupAll() pour nettoyer fichiers, profils et processus.
        public void Dispose()
        {
            if (!_disposed)
            {
                CleanupAll();
                _disposed = true;
            }
        }
    }
}