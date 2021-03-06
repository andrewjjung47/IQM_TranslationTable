﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IQM_TranslationTable
{
    /// <summary>
    /// Write stream for log file.
    /// </summary>
    public class LogStream
    {
        private readonly object _locker = new Object();

        private StreamWriter sw;

        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                if (value == "")
                {
                    path = "";
                }
                else
                {
                    string date = DateTime.Now.ToString("yyyyMMdd");
                    path = string.Format("{0}\\TransTableLog_{1}.txt",
                        value, date);
                }
            }
        }

        private bool open = false;
        /// <summary>
        /// Open a stream writer for logging. If Path is an empty string, 
        /// no stream writer is created.
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            if (!open)
            {
                try
                {
                    if (Path == null) return false;
                    else if (Path == "") return true;
                    else
                    {
                        sw = new StreamWriter(Path, true); // always append to file
                        open = true;
                        Write("Open");
                        return true;
                    }

                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public void Close()
        {
            if (open)
            {
                Write("Close");
                if (sw != null)
                {
                    sw.Close();
                    sw = null;
                }
                open = false;
            }
        }

        /// <summary>
        /// Thread safe write to log file.
        /// </summary>
        /// <param name="message"></param>
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

    /// <summary>
    /// Formats log write message with the name of the caller.
    /// </summary>
    public class Logger
    {
        private LogStream log;
        private string callerName;

        public Logger(LogStream log, string callerName)
        {
            this.log = log;
            this.callerName = callerName;
        }

        public void Log(string message)
        {
            if (log != null)
            {
                string fullMessage = string.Format("{0, -15}  {1}",
                    callerName, message);
                log.Write(fullMessage);
            }
        }
    }
}
