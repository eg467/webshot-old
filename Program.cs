using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Webshot
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(params string[] args)
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var projectPath = File.Exists(args.FirstOrDefault())
                ? args.FirstOrDefault()
                : FileProjectStore.CreateTempProjectDirectory(temporaryDir: true);

            var form = CreateForm(projectPath);

            Application.Run(form);
        }

        private static Form1 CreateForm(string projectPath)
        {
            var frm = new Form1();
            if (File.Exists(projectPath))
            {
                var projectDir = Path.GetDirectoryName(projectPath);
                frm.LoadOrCreateProject(projectPath);
            }
            return frm;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}