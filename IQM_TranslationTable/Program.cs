using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IQM_TranslationTable
{
    static class Program
    {
        static LogStream log;
        static Logger logger;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            log = new LogStream();
            logger = new Logger(log, "Main");

            Application.Run(new IQM_TranslationTable(log));
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;

            if (log.Open())
            {
                logger.Log(ex.ToString());
            }
            else
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
