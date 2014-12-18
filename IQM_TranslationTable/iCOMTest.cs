using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private IQM_TranslationTable form;
        private ObservableCollection<short> stateQueue;
        private event EventHandler MoveTable;

        public iCOMTest(IQM_TranslationTable form)
        {
            InitializeComponent();
            monitor = new IComMonitor();
            this.form = form;
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (stateQueue[stateQueue.Count - 1] == 5)
            {
                int indexStart = stateQueue.IndexOf(4);
                int indexIrradiate = stateQueue.IndexOf(5);
                int indexTerminate = stateQueue.IndexOf(9);

                if (indexStart < indexIrradiate &&
                    indexIrradiate < indexTerminate &&
                    indexTerminate < stateQueue.Count - 1)
                {
                    form.moveEvent();
                }
            }
        }

        private void Connect()
        {
            stateQueue = new ObservableCollection<short>();
            stateQueue.CollectionChanged += HandleChange;

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
                    if (stateQueue.Count != 0)
                    {
                        if (stateQueue[stateQueue.Count - 1] != data.State)
                        {
                            stateQueue.Add(data.State);
                        }
                    }
                    else
                    {
                        stateQueue.Add(data.State);
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
