using System;
using System.Drawing;
using System.Windows.Forms;
using XmlToPdfConverter.Core.Interfaces;

namespace XmlToPdfConverter.GUI
{
    public class GuiLogger : ILogger
    {
        private readonly RichTextBox _logBox;

        public GuiLogger(RichTextBox logBox)
        {
            _logBox = logBox;
        }

        public void Log(string message, LogLevel level = LogLevel.Info)
        {
            if (_logBox.InvokeRequired)
            {
                _logBox.Invoke(new Action(() => Append(message, level)));
            }
            else
            {
                Append(message, level);
            }
        }

        private void Append(string message, LogLevel level)
        {
            string prefix = $"[{DateTime.Now:HH:mm:ss}] [{level}] ";
            Color color;

            switch (level)
            {
                case LogLevel.Error:
                    color = Color.Red;
                    break;
                case LogLevel.Warning:
                    color = Color.Orange;
                    break;
                case LogLevel.Debug:
                    color = Color.Gray;
                    break;
                default:
                    color = Color.Black;
                    break;
            }

            _logBox.SelectionColor = color;

            _logBox.AppendText(prefix + message + Environment.NewLine);
            _logBox.ScrollToCaret();
        }
    }
}
