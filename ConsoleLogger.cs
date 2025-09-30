using System;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Logging
{
    //Logger pour afficher les messages dans la console
    public class ConsoleLogger : ILogger
    {
        //Enregistre un message dans la console avec horodatage et couleur selon le niveau
        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            switch (level)
            {
                case LogLevel.Error: //Couleur rouge pour les messages d'erreurs
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Warning: //Couleur jaune pour les messages d'avertissements
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Debug: //Couleur bleu cyan pour les messages de debug
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default: //Info normal
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            //Horodatages, construction des lingnes de logs
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{level}] {message}");
            //Réinitailisie la couleur
            Console.ResetColor();
        }
    }
}

