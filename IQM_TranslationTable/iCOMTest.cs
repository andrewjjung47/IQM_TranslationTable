using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using IQM.Common;
using IQM.Elekta.ICom;
using IQM.Elekta.IComInterface;

namespace IQM_TranslationTable
{
    public partial class iCOMTest : Form
    {
        private IComVxListener icom;
        private IComMonitor monitor;

        public iCOMTest()
        {
            InitializeComponent();
            monitor = new IComMonitor();
        }

        private void Connect()
        {
            string ip = textBox2.Text.Trim();

            icom = new IComVxListener(null);
            icom.OnIComData += new IComVxListener.IComDataHandler(DataHandler);
            icom.OnIComEvent += new IComVxListener.IComEventHandler(EventHandler);
            icom.Start(ip);

            monitor.Reset();
        }

        private void Disconnect()
        {
            icom.Stop();
        }

        private void EventHandler(IComEventType eventType)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new IComVxListener.IComEventHandler(EventHandler), new object[] { eventType });
            }
            else
            {
                Log(eventType.ToString());
            }
        }
 
        private void DataHandler(IComData data)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new IComVxListener.IComDataHandler(DataHandler), new object[] { data });
            }
            else
            {
                try
                {
                    if (data.State == 10)
                    {
                        _waitHandle.Set();
                    }
                    Show(data);
                }
                catch (Exception)
                {
                }
            }
        }

        private StreamWriter recorder;

        private void Show(IComData data)
        {

            if (recorder != null)
            {
                data.Save(recorder);
            }

            Log("Data {0}/{1}, seq {2}, {3}, [{4}]", data.State, IComApi.StateToString(data.State), data.SeqNumber.ToString("x4"), DateTimeStrings.TimeMs(data.Dt), data.InhibitCount);
        }

        private void Log(string text)
        {
            conTextBox.AppendText(text + Environment.NewLine);
        }

        private void Log(string format, params object[] o)
        {
            Log(string.Format(format, o));
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            Disconnect();
        }


        static EventWaitHandle _waitHandle = new AutoResetEvent(false);
        delegate void UIDelegate(string message);
        static readonly object _locker = new object();

        private void SimulateTable()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                WriteStatus("Test " + i.ToString() + ": waiting..." + Environment.NewLine);
                _waitHandle.WaitOne();
                WriteStatus("Test " + i.ToString() + ": signal received" + Environment.NewLine);
            }
            WriteStatus("End simulation" + Environment.NewLine);
        }

        private void WriteStatus(string message)
        {
            if (textBox1.InvokeRequired)
            {
                UIDelegate d = new UIDelegate(WriteStatus);
                Invoke(d, new object[] { message});
            }
            else
            {
                 textBox1.AppendText(message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _waitHandle.Set();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("Start simulation" + Environment.NewLine);
            new Thread(SimulateTable).Start();
        }
    }
}
