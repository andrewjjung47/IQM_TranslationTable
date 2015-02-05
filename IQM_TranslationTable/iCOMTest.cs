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

        public iCOMTest(IQM_TranslationTable form)
        {
            InitializeComponent();
            monitor = new IComMonitor();
            this.form = form;
        }

        // Change this later
        private readonly int irradNum = 2;
        private int irradCount = 0;
        private int IrradCount
        {
            get { return irradCount; }
            set
            {
                irradCount = value;
                if (irradCount == irradNum)
                {
                    form.moveEvent();
                    irradCount = 0;
                }
            }
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (stateQueue[stateQueue.Count - 1] == 9)
            {
                int indexStart = stateQueue.IndexOf(4);

                if (indexStart != -1 && indexStart == stateQueue.Count - 3)
                {
                    IrradCount++;

                    int index = form.MeasurementDataGridView.Rows.Add();
                    DataGridViewRow row = form.MeasurementDataGridView.Rows[index];
                    row.HeaderCell.Value = DateTime.Now.ToString("HH:mm:ss");
                    row.Cells[0].Value = form.CSM.motor1.CurrentRelPosition;
                    row.Cells[1].Value = form.CSM.motor2.CurrentRelPosition;
                }
                stateQueue.Clear();
            }
            else if (stateQueue[stateQueue.Count - 1] == 11)
            {
                form.pauseButton_Click(this, EventArgs.Empty);
                stateQueue.Clear();
                IrradCount = 0;
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
                    if (form.CSM.motor1.status == MotorStatus.Paused && 
                        form.CSM.motor2.status == MotorStatus.Paused)
                    {
                        if (stateQueue.Count != 0 && stateQueue[stateQueue.Count - 1] != data.State)
                        {
                            stateQueue.Add(data.State);
                        }
                        else if (stateQueue.Count == 0)
                        {
                            stateQueue.Add(data.State);
                        }
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

        private void OnIcomFormExit(object sender, FormClosingEventArgs e)
        {
            if (icom != null)
            {
                icom.Stop();
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            IrradCount++;

            int index = form.MeasurementDataGridView.Rows.Add();
            DataGridViewRow row = form.MeasurementDataGridView.Rows[index];
            row.HeaderCell.Value = DateTime.Now.ToString("HH:mm:ss");
            row.Cells[0].Value = form.CSM.motor1.CurrentRelPosition;
            row.Cells[1].Value = form.CSM.motor2.CurrentRelPosition;
        }
    }
}
