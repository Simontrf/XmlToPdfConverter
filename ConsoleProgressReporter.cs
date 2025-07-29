using System;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.Core.Progress
{
    public class ConsoleProgressReporter : IProgressReporter
    {
        private int lastPercent = -1;
        private readonly int barWidth = 40;
        private bool isFirstReport = true;

        public void Report(int percent, string message)
        {
            // Limiter entre 0 et 100
            percent = Math.Max(0, Math.Min(100, percent));

            // Ne mettre à jour que si le pourcentage a changé
            if (percent != lastPercent)
            {
                lastPercent = percent;
                DrawProgressBar(percent, message);
            }
        }

        private void DrawProgressBar(int percent, string message)
        {
            // Si c'est le premier rapport, faire un saut de ligne
            if (isFirstReport)
            {
                Console.WriteLine();
                isFirstReport = false;
            }

            // Sauvegarder la position du curseur
            int currentLineCursor = Console.CursorTop;

            // Calculer le nombre de caractères remplis
            int filledWidth = (int)((percent / 100.0) * barWidth);

            // Créer la barre avec des caractères ASCII compatibles
            string bar = "[" + new string('#', filledWidth) + new string('.', barWidth - filledWidth) + "]";

            // Effacer la ligne actuelle
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, currentLineCursor);

            // Afficher la barre et le pourcentage
            Console.Write($"{bar} {percent:000}% - {message}");

            // Aller à la ligne suivante seulement si c'est terminé
            if (percent >= 100)
            {
                Console.WriteLine();
                Console.WriteLine(); // Ligne vide après la completion
            }
        }
    }
}