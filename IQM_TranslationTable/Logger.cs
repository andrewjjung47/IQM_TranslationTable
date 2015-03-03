using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IQM_TranslationTable
{
    public class Logger
    {
        private readonly object _locker = new Object();

        private StreamWriter sw;

        private bool open;

        public string Path
        {
            get;
            set;
        }

        public bool Open()
        {
            if (!open)
            {
                try
                {
                    if (Path == null) return false;
                    sw = new StreamWriter(Path, true); // always append to file
                    open = true;
                }
                catch (Exception)
                {
                    return false;
                }
                Write("Open");
                return true;
            }
            else
            {
                return true;
            }
        }

        public bool Close()
        {
            if (open)
            {
                try
                {
                    Write("Close");
                    if (sw != null)
                    {
                        sw.Close();
                        sw = null;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                open = false;
                return true;
            }
            else
            {
                return true;
            }
        }

        public void Write(string message)
        {
            if (open)
            {
                lock (_locker)
                {
                    sw.WriteLine(string.Format("{0}  {1}", 
                        DateTime.Now.ToString(), message));
                }
            }
        }
    }

    public class LoggerHelper
    {
        private Logger logger;
        private string helperName;

        public LoggerHelper(Logger logger, string helperName)
        {
            this.logger = logger;
            this.helperName = helperName;
        }

        public void Log(string message)
        {
            if (logger != null)
            {
                string fullMessage = string.Format("{0, -10}  {1}",
                    helperName, message);
                logger.Write(fullMessage);
            }
        }
    }
}
