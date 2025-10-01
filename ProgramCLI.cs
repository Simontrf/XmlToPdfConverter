using System;
using System.IO;
using System.Threading.Tasks;
using XmlToPdfConverter.Core.Configuration;
using XmlToPdfConverter.Core.Interfaces;
using XmlToPdfConverter.Core.Logging;
using XmlToPdfConverter.Core.Progress;
using XmlToPdfConverter.Core.Services;

namespace XmlToPdfConverter.CLI
{
    //Gère les arguments, la validation, la conversion et l'affichage des résultats
    class ProgramCLI
    {
        private static ChromeConversionService _conversionService;
        private static IResourceManager _resourceManager;

        //Traite les arguments, exécute la conversion et retourne un code de sortie
        static async Task<int> Main(string[] args)
        {
            try
            {
                //Vérifier le nombre minimum d'arguments
                if (args.Length < 2)
                {
                    ShowUsage();
                    return 1;
                }
                //Parser les arguments (XML, XSL, sortie optionnelle)
                var (xmlPath, xslPath, outputPath) = ParseArguments(args);

                //Valider les chemins de fichiers
                if (!ValidateArguments(xmlPath, xslPath, outputPath))
                {
                    return 1;
                }
                //Lancement de la conversion
                return await ConvertAsync(xmlPath, xslPath, outputPath);
            }
            catch (Exception ex)
            {
                //Gestion des erreurs critiques
                Console.WriteLine($"\n💥 Erreur critique: {ex.Message}");
                if (args.Contains("--debug"))
                {
                    //Afficher la stack trace si mode debug
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }
                return 3;
            }
            finally
            {
                //Nettoyage des ressources quoi qu'il arrive
                Cleanup();
            }
        }

        //Parse les arguments de ligne de commande
        private static (string xmlPath, string xslPath, string outputPath) ParseArguments(string[] args)
        {
            string xmlPath = args[0];
            string xslPath = args[1];
            string outputPath = args.Length > 2 ? args[2] : null;

            //Si pas de chemin de sortie sélectionner, utiliser le même dossier que le XML
            if (string.IsNullOrEmpty(outputPath))
            {
                string xmlDir = Path.GetDirectoryName(Path.GetFullPath(xmlPath));
                string baseName = Path.GetFileNameWithoutExtension(xmlPath);
                outputPath = Path.Combine(xmlDir, baseName + ".pdf");
            }

            return (xmlPath, xslPath, outputPath);
        }

        //Valide que les fichiers d'entrée existent et que le dossier de sortie est accessible
        private static bool ValidateArguments(string xmlPath, string xslPath, string outputPath)
        {
            //Vérification du fichier XML
            if (!File.Exists(xmlPath))
            {
                Console.WriteLine($"❌ Fichier XML introuvable: {xmlPath}");
                return false;
            }
            //Vérification du fichier XSL
            if (!File.Exists(xslPath))
            {
                Console.WriteLine($"❌ Fichier XSL introuvable: {xslPath}");
                return false;
            }
            //Vérification ou création du dossier de sortie du PDF
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                    Console.WriteLine($"✓ Dossier de sortie créé: {outputDir}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Impossible de créer le dossier de sortie: {ex.Message}");
                return false;
            }

            return true;
        }
        //Exécute la conversion XML/XSL vers PDF avec affichage de la progression
        private static async Task<int> ConvertAsync(string xmlPath, string xslPath, string outputPath)
        {
            //Initialisation du logger console avec couleurs
            var logger = new ConsoleLogger();
            //Configuration avec debug activé et pas d'ouverture auto du PDF
            var appConfig = new AppConfiguration
            {
                Logging = { EnableDebugLogging = true },
                Conversion = { OpenResultAfterConversion = false }
            };

            //Création des services
            _resourceManager = new ResourceManager(logger);
            _conversionService = new ChromeConversionService(logger, appConfig, _resourceManager);

            //Vérification de la disponibilité de Chrome
            if (!_conversionService.IsAvailable)
            {
                Console.WriteLine("❌ Chrome n'est pas disponible pour la conversion");
                return 1;
            }
            //Affichage des informations de conversion
            Console.WriteLine("🚀 Début de la conversion...");
            Console.WriteLine($"   XML : {xmlPath}");
            Console.WriteLine($"   XSL : {xslPath}");
            Console.WriteLine($"   PDF : {outputPath}");
            Console.WriteLine();

            //Création d'un reporter de progression ASCII
            var progressReporter = new ConsoleProgressReporter();
            var progress = new Progress<ConversionProgress>(p =>
            {
                progressReporter.Report(p.Percentage, p.CurrentStep);
            });
            //Lancement de la conversion
            var result = await _conversionService.ConvertAsync(
                xmlPath,
                xslPath,
                outputPath);

            //Traitment du résultat de conversion
            if (result.Success)
            {
                Console.WriteLine();
                Console.WriteLine("✅ Conversion terminée avec succès !");
                Console.WriteLine($"   Fichier PDF: {result.OutputPath}");
                Console.WriteLine($"   Taille: {result.FileSizeBytes:N0} octets");
                Console.WriteLine($"   Durée: {FormatDuration(result.Duration)}");
                return 0;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"❌ Échec de la conversion: {result.ErrorMessage}");
                return 1;
            }
        }
        //Nettoyage des ressources (services, fichgiers, profils...)
        private static void Cleanup()
        {                       
            _conversionService?.Dispose();
            _resourceManager?.CleanupAll();                        
        }
        public void Dispose()
        {
            Cleanup();
            _resourceManager?.Dispose();
            _conversionService?.Dispose();
        }
        //Affichage intelligent de la durée
        private static string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalMinutes < 1)
                return $"{duration.Seconds}s";
            else if (duration.TotalHours < 1)
                return $"{duration.Minutes}min {duration.Seconds}s";
            else
                return $"{duration.Hours}h {duration.Minutes}min {duration.Seconds}s";
        }
        //Instructions d'utilisation de l'interface CLI
        private static void ShowUsage()
        {
            Console.WriteLine("Convertisseur XML vers PDF (Chrome)");
            Console.WriteLine();
            Console.WriteLine("USAGE:");
            Console.WriteLine("  XmlToPdfConverter.CLI.exe <fichier.xml> <fichier.xsl> [sortie.pdf]");
            Console.WriteLine();
            Console.WriteLine("PARAMÈTRES:");
            Console.WriteLine("  fichier.xml    Fichier XML d'entrée (requis)");
            Console.WriteLine("  fichier.xsl    Fichier XSL de transformation (requis)");
            Console.WriteLine("  sortie.pdf     Fichier PDF de sortie (optionnel)");
            Console.WriteLine();
            Console.WriteLine("EXEMPLES:");
            Console.WriteLine("  XmlToPdfConverter.CLI.exe document.xml style.xsl");
            Console.WriteLine("  XmlToPdfConverter.CLI.exe document.xml style.xsl rapport.pdf");
            Console.WriteLine();
            Console.WriteLine("OPTIONS:");
            Console.WriteLine("  --debug        Afficher les détails de débogage");
        }
    }
}