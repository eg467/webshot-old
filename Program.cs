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
            if (File.Exists(projectPath))
            {
                projectPath = Path.GetDirectoryName(projectPath);
            }

            var frmController = new Form1Controller(projectPath);
            return frmController.CreateForm();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}