using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OdlotoweCyferki
{
    internal static class Program
    {
        //importowanie funkcji SetProcessDPIAware z pliku DLL user32.dll

        [DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main()
        {
            SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OdlotoweCyferki());
        }
    }
}