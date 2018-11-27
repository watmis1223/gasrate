using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace CalculationOilPrice
{
    static class Program
    {
        //static MainForm _MainForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] arguments)
        {
            MainForm oForm = null;
            bool bInstanceFlag;

            Mutex MyApplicationMutex = new Mutex(true, "CalculationOilPrice_Mutex", out bInstanceFlag);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            if (!bInstanceFlag)
            {
                oForm = new MainForm();
                SingleInstanceApplication.Run(oForm, NewInstanceHandler);
            }
            else
            {                
                oForm = new MainForm(arguments);
                SingleInstanceApplication.Run(oForm, NewInstanceHandler);
                oForm.Close();
            }

            //_MainForm = new MainForm();
            //SingleInstanceApplication.Run(_MainForm, NewInstanceHandler);
        }

        public static void NewInstanceHandler(object sender, StartupNextInstanceEventArgs e)
        {
            //first parameter is application path
            //second is parameter from proffix
            //string sParams = e.CommandLine[1];
            //MessageBox.Show(sParams);            

            if (sender !=null && (e.CommandLine != null && e.CommandLine.Count > 0))
            {
                //_MainForm.ShowModule(Library.Global.ApplicationModules.PriceModuleCalculation, e.CommandLine.ToArray());
                SingleInstanceApplication app = (SingleInstanceApplication)sender;
                MainForm oForm = (MainForm)app.OpenForms[0];

                e.BringToForeground = true;
                oForm.CallByProffix(e.CommandLine.ToArray());
                //oForm.ShowModule(Library.Global.ApplicationModules.PriceModuleCalculation, e.CommandLine.ToArray());                
            }            
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
