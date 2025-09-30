using System;
using System.Threading.Tasks;

namespace XmlToPdfConverter.Core.Interfaces
{
    //Retour d'information sur la conversion effectuer
    public class ConversionResult
    {
        public bool Success { get; set; } //Indique le succés ou non de la conversion
        public string OutputPath { get; set; } //Chemin de sortie du fichier PDF
        public string ErrorMessage { get; set; } //Affiche un message d'erreur en cas d'échec
        public TimeSpan Duration { get; set; } //Durée totale de la conversion
        public long FileSizeBytes { get; set; } //Taille du fichier PDF générer en octets
    }

    //Etat de progression de la conversion en cours
    public class ConversionProgress
    {
        public int Percentage { get; set; } //Pourcentage de complétion
        public string CurrentStep { get; set; } //Description de l'étape actuelle
        public TimeSpan Elapsed { get; set; } //Temps écoulé durant la conversion
    }

    public interface IConversionService
    {
        Task<ConversionResult> ConvertAsync(
            string xmlPath, //Chemin du fichier XML source
            string xslPath, //Chemin du fichier XSL
            string outputPath, //Chemin de sortie du PDF 
            IProgress<ConversionProgress> progress = null);

        //Vérifie si la conversion est prête à s'effectuer
        bool IsAvailable { get; }
        //Nettoyage des ressources temporaires
        void Cleanup();
    }
}