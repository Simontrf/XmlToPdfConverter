using System;
using System.Windows.Forms;

namespace XmlToPdfConverter.GUI
{
    internal static class ProgramGUI
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
