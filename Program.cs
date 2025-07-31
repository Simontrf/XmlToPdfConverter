using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using XmlToPdfConverter.Core.Configuration;
using XmlToPdfConverter.Core.Engine;
using XmlToPdfConverter.Core.Interfaces;
using XmlToPdfConverter.Core.Logging;
using XmlToPdfConverter.Core.Progress;

namespace XmlToPdfConverter.CLI
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Vérifier si l'option -gui est présente
            if (args.Length > 0 && (args[0] == "-gui" || args[0] == "--gui"))
            {
                var exePath = Path.Combine(AppContext.BaseDirectory, "XmlToPdfConverter.GUI.exe");
                if (File.Exists(exePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = exePath,
                        UseShellExecute = true // pour ouvrir la fenêtre
                    });
                }
                else
                {
                    Console.Error.WriteLine("Erreur : GUI introuvable à " + exePath);
                }
                return;
            }

            // Logique CLI existante
            if (args.Length < 3 || args[0] == "--help" || args[0] == "-h")
            {
                PrintHelp();
                Environment.Exit(2);
            }

            string inputXml = args[0];
            string inputXsl = args[1];
            string outputPdf = args[2];

            RunCLI(inputXml, inputXsl, outputPdf);
        }

        private static void RunCLI(string inputXml, string inputXsl, string outputPdf)
        {
            ILogger logger = new ConsoleLogger();
            IProgressReporter progress = new ConsoleProgressReporter();

            var chromeConfig = new ChromeConfiguration(logger);

            if (!chromeConfig.IsAvailable)
            {
                logger.Log("Chrome portable non disponible", LogLevel.Error);
                Environment.Exit(2);
            }

            logger.Log($"Chrome trouvé: {chromeConfig.ChromePath}", LogLevel.Info);
            logger.Log($"Profil Chrome: {chromeConfig.ProfilePath}", LogLevel.Debug);

            if (!File.Exists(inputXml))
            {
                logger.Log("Fichier XML introuvable: " + inputXml, LogLevel.Error);
                Environment.Exit(2);
            }

            if (!File.Exists(inputXsl))
            {
                logger.Log("Fichier XSL introuvable: " + inputXsl, LogLevel.Error);
                Environment.Exit(2);

                try
                {
                    string xmlDir = Path.GetDirectoryName(Path.GetFullPath(inputXml));
                    string xslFullPath = Path.GetFullPath(inputXsl);

                    logger.Log($"Répertoire XML: {xmlDir}", LogLevel.Debug);
                    logger.Log($"Chemin XSL: {xslFullPath}", LogLevel.Debug);
                }
                catch (Exception ex)
                {
                    logger.Log($"Erreur validation chemins: {ex.Message}", LogLevel.Error);
                    Environment.Exit(2);
                }
            }

            try
            {
                var preprocessor = new XmlPreprocessor();
                string processedXml = preprocessor.Preprocess(inputXml, inputXsl, logger);

                var converter = new ChromeConverter(chromeConfig.ChromePath, chromeConfig.ProfilePath);

                logger.Log("Démarrage de la conversion...", LogLevel.Info);
                bool success = converter.Convert(processedXml, outputPdf, progress, logger);

                if (File.Exists(processedXml))
                {
                    File.Delete(processedXml);
                }

                chromeConfig.CleanupProfile();

                if (success)
                {
                    logger.Log("Conversion terminée avec succès.", LogLevel.Info);
                    Environment.Exit(0);
                }
                else
                {
                    logger.Log("Échec de la conversion.", LogLevel.Error);
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Erreur inattendue: {ex.Message}", LogLevel.Error);
                Environment.Exit(1);
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Convertisseur XML vers PDF");
            Console.WriteLine();
            Console.WriteLine("Usage CLI:");
            Console.WriteLine("  XmlToPdfConverter.exe input.xml input.xsl output.pdf");
            Console.WriteLine();
            Console.WriteLine("Usage GUI:");
            Console.WriteLine("  XmlToPdfConverter.exe -gui");
            Console.WriteLine("  XmlToPdfConverter.exe --gui");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -gui, --gui          Lance l'interface graphique");
            Console.WriteLine("  --help, -h           Affiche cette aide");
            Console.WriteLine();
            Console.WriteLine("Exemples:");
            Console.WriteLine("  XmlToPdfConverter.exe invoice.xml invoice.xsl invoice.pdf");
            Console.WriteLine("  XmlToPdfConverter.exe -gui");
        }
    }
}