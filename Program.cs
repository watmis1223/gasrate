using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace CalculationOilPrice
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] arguments)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            SingleInstanceApplication.Run(new MainForm(), NewInstanceHandler);
        }

        public static void NewInstanceHandler(object sender, StartupNextInstanceEventArgs e)
        {
            //first parameter is application path
            //second is parameter from proffix
            string sParams = e.CommandLine[1];
            MessageBox.Show(sParams);
            e.BringToForeground = false;
            //ControlPanel.uploadImage(imageLocation);
        }

        public class SingleInstanceApplication : WindowsFormsApplicationBase
        {
            private SingleInstanceApplication()
            {
                base.IsSingleInstance = true;
            }

            public static void Run(Form f, StartupNextInstanceEventHandler startupHandler)
            {
                SingleInstanceApplication app = new SingleInstanceApplication();
                app.MainForm = f;
                app.StartupNextInstance += startupHandler;
                app.Run(Environment.GetCommandLineArgs());
            }
        }

    }
}
