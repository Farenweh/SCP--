using System;
using System.Windows.Forms;

namespace ScpLauncher
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try { ConfigStore.EnsureConfigDir(); } catch { }
            Application.Run(new MainForm());
        }
    }
}
