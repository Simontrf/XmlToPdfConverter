using System;
using System.Collections.Generic;

namespace XmlToPdfConverter.Core.Interfaces
{
    public interface IResourceManager : IDisposable
    {
        string CreateTemporaryProfile();
        void RegisterTemporaryFile(string filePath);
        void RegisterProcess(int processId);
        void CleanupAll();
        void CleanupProfile(string profilePath);
    }

    public class ResourceManager : IResourceManager
    {
        private readonly List<string> _temporaryFiles = new List<string>();
        private readonly List<string> _temporaryProfiles = new List<string>();
        private readonly List<int> _processes = new List<int>();
        private readonly ILogger _logger;
        private bool _disposed = false;

        public ResourceManager(ILogger logger = null)
        {
            _logger = logger;
        }

        public string CreateTemporaryProfile()
        {
            string profilePath = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                $"chrome-profile-{Guid.NewGuid()}");

            System.IO.Directory.CreateDirectory(profilePath);
            _temporaryProfiles.Add(profilePath);
            _logger?.Log($"✓ Profil temporaire créé: {profilePath}", LogLevel.Debug);

            return profilePath;
        }

        public void RegisterTemporaryFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                _temporaryFiles.Add(filePath);
            }
        }

        public void RegisterProcess(int processId)
        {
            _processes.Add(processId);
        }

        public void CleanupProfile(string profilePath)
        {
            try
            {
                if (System.IO.Directory.Exists(profilePath))
                {
                    System.IO.Directory.Delete(profilePath, true);
                    _temporaryProfiles.Remove(profilePath);
                    _logger?.Log("✓ Profil Chrome nettoyé", LogLevel.Debug);
                }
            }
            catch (Exception ex)
            {
                _logger?.Log($"⚠ Erreur nettoyage profil: {ex.Message}", LogLevel.Warning);
            }
        }

        public void CleanupAll()
        {
            // Nettoyer les fichiers temporaires
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

            // Nettoyer les profils
            foreach (string profile in _temporaryProfiles.ToArray())
            {
                CleanupProfile(profile);
            }

            // Nettoyer les processus
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
                catch (Exception ex)
                {
                    _logger?.Log($"⚠ Erreur arrêt processus {processId}: {ex.Message}", LogLevel.Warning);
                }
            }
            _processes.Clear();
        }

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