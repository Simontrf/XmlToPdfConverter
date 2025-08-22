using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XmlToPdfConverter.Core.Configuration;
using XmlToPdfConverter.Core.Interfaces;
using XmlToPdfConverter.Core.Logging;
using XmlToPdfConverter.Core.Progress;
using XmlToPdfConverter.Core.Services;

namespace XmlToPdfConverter.CLI
{
    class Program
    {
        private static ChromeConversionService _conversionService;
        private static IResourceManager _resourceManager;

        static async Task<int> Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    ShowUsage();
                    return 1;
                }

                var (xmlPath, xslPath, outputPath) = ParseArguments(args);

                if (!ValidateArguments(xmlPath, xslPath, outputPath))
                {
                    return 1;
                }

                return await ConvertAsync(xmlPath, xslPath, outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n💥 Erreur critique: {ex.Message}");
                if (args.Contains("--debug"))
                {
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }
                return 3;
            }
            finally
            {
                Cleanup();
            }
        }

        private static (string xmlPath, string xslPath, string outputPath) ParseArguments(string[] args)
        {
            string xmlPath = args[0];
            string xslPath = args[1];
            string outputPath = args.Length > 2 ? args[2] : null;

            // Si pas de chemin de sortie spécifié, utiliser le même dossier que le XML
            if (string.IsNullOrEmpty(outputPath))
            {
                string xmlDir = Path.GetDirectoryName(Path.GetFullPath(xmlPath));
                string baseName = Path.GetFileNameWithoutExtension(xmlPath);
                outputPath = Path.Combine(xmlDir, baseName + ".pdf");
            }

            return (xmlPath, xslPath, outputPath);
        }

        private static bool ValidateArguments(string xmlPath, string xslPath, string outputPath)
        {
            if (!File.Exists(xmlPath))
            {
                Console.WriteLine($"❌ Fichier XML introuvable: {xmlPath}");
                return false;
            }

            if (!File.Exists(xslPath))
            {
                Console.WriteLine($"❌ Fichier XSL introuvable: {xslPath}");
                return false;
            }

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

        private static async Task<int> ConvertAsync(string xmlPath, string xslPath, string outputPath)
        {
            var logger = new ConsoleLogger();
            var appConfig = new AppConfiguration
            {
                Logging = { EnableDebugLogging = true },
                Conversion = { OpenResultAfterConversion = false }
            };

            _resourceManager = new ResourceManager(logger);
            _conversionService = new ChromeConversionService(logger, appConfig, _resourceManager);

            if (!_conversionService.IsAvailable)
            {
                Console.WriteLine("❌ Chrome n'est pas disponible pour la conversion");
                return 1;
            }

            Console.WriteLine("🚀 Début de la conversion...");
            Console.WriteLine($"   XML : {xmlPath}");
            Console.WriteLine($"   XSL : {xslPath}");
            Console.WriteLine($"   PDF : {outputPath}");
            Console.WriteLine();

            var progressReporter = new ConsoleProgressReporter();
            var progress = new Progress<ConversionProgress>(p =>
            {
                progressReporter.Report(p.Percentage, p.CurrentStep);
            });

            var result = await _conversionService.ConvertAsync(
                xmlPath,
                xslPath,
                outputPath);

            if (result.Success)
            {
                Console.WriteLine();
                Console.WriteLine("✅ Conversion terminée avec succès !");
                Console.WriteLine($"   Fichier PDF: {result.OutputPath}");
                Console.WriteLine($"   Taille: {result.FileSizeBytes:N0} octets");
                Console.WriteLine($"   Durée: {result.Duration.TotalSeconds:F1} secondes");
                return 0;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"❌ Échec de la conversion: {result.ErrorMessage}");
                return 1;
            }
        }

        private static void Cleanup()
        {
            try
            {
                _conversionService?.Dispose();
                _resourceManager?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Erreur lors du nettoyage: {ex.Message}");
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Convertisseur XML vers PDF (Chrome) - Version Refactorisée");
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