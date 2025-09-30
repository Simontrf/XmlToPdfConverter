using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.GUI
{
    //Logger pour interface graphique
    //Affiche les messages dans un RichTextBox avec couleurs selon le niveau
    public class GuiLogger : ILogger
    {
        private readonly RichTextBox _logBox;

        //Initialise le logger GUI avec RichTextBox
        public GuiLogger(RichTextBox logBox)
        {
            _logBox = logBox;
        }

        //Enregistre un message avec horodatage et couleur selon le niveau
        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            //Vérifier si on est sur le thread UI
            if (_logBox.InvokeRequired)
            {
                //Appel depuis un  autre thread
                _logBox.Invoke(new Action(() => Append(message, level)));
            }
            else
            {
                //Si déjà sur le thread UI : appel direct
                Append(message, level);
            }
        }

        //Ajoute le message au RichTextBox avec formatage coloré, doit être appelé sur le thread UI
        private void Append(string message, LogLevel level)
        {
            //Création du préfixe avec timestamp et niveau
            string prefix = $"[{DateTime.Now:HH:mm:ss}] [{level}] ";
            Color color;

            switch (level)
            {
                // Définition de la couleur selon le niveau
                case LogLevel.Error: //Couleur rouge pour les erreurs
                    color = Color.Red;
                    break;
                case LogLevel.Warning: //Couleur orange pour les avertissements
                    color = Color.Orange;
                    break;
                case LogLevel.Debug: //Couleur grise pour les messeages de debug
                    color = Color.Gray;
                    break;
                default: //Couleur noir pour les infos
                    color = Color.Black;
                    break;
            }

            //Appliquer la couleur au message suivant
            _logBox.SelectionColor = color;

            //Ajout d'un saut de ligne
            _logBox.AppendText(prefix + message + Environment.NewLine);
            //Scroller automatiquement vers le bas afin d'avoir le dernier message
            _logBox.ScrollToCaret();
        }
    }
}
