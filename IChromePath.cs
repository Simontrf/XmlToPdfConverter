namespace XmlToPdfConverter.Core.Configuration
{
    public interface IChromePathResolver
    {
        //Obtient le chemin complet de l'exécutable Chrome
        string GetChromeExecutablePath();
        //Vérifie si Chrome est disponible
        bool IsChromeAvailable();
    }
}