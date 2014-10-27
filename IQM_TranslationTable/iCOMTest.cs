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

using IQM.Common;
using IQM.Elekta.ICom;
using IQM.Elekta.IComInterface;

namespace IQM_TranslationTable
{
    public partial class iCOMTest : Form
    {
        public iCOMTest()
        {
            InitializeComponent();
        }

        private IComVxListener icom;
        private IComMonitor monitor;

        /*
        private void Connect()
        {
            string ip = "192.168.108.2";

            icom = new IComVxListener(null);
            icom.OnIComData += new IComVxListener.IComDataHandler(DataHandler);
            icom.OnIComEvent += new IComVxListener.IComEventHandler(EventHandler);
            icom.Start(ip);

            monitor.Reset();
        }*/

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
        /*
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
                    Show(data);
                    _waitHandle.Set();
                }
                catch (Exception)
                {
                }
            }
        }


        private void Show(IComData data)
        {
            dataListView.BeginUpdate();

            for (int t = 0; t < icomTags.Length; t++)
            {
                UInt32 tag = icomTags[t];

                IComDataItem newItem = data[tag];
                IComDataItem oldItem = (IComDataItem)dataListView.Items[t].Tag;

                string newPre = null;
                string newSet = null;
                string newRun = null;
                if (newItem != null)
                {
                    newPre = newItem.Get(IComData.Part.Pre);
                    newSet = newItem.Get(IComData.Part.Set);
                    newRun = newItem.Get(IComData.Part.Run);
                }

                string oldPre = null;
                string oldSet = null;
                string oldRun = null;
                if (oldItem != null)
                {
                    oldPre = oldItem.Get(IComData.Part.Pre);
                    oldSet = oldItem.Get(IComData.Part.Set);
                    oldRun = oldItem.Get(IComData.Part.Run);
                }

                dataListView.Items[t].SubItems[columnPrescribed.DisplayIndex].Text = (newPre == null) ? "---" : newPre;
                dataListView.Items[t].SubItems[columnSet.DisplayIndex].Text = (newSet == null) ? "---" : newSet;
                dataListView.Items[t].SubItems[columnRun.DisplayIndex].Text = (newRun == null) ? "---" : newRun;

                dataListView.Items[t].Tag = newItem;
            }

            dataListView.EndUpdate();

            if (!widthSet)
            {
                dataListView.Columns[columnPrescribed.DisplayIndex].Width = -1;
                dataListView.Columns[columnSet.DisplayIndex].Width = -1;
                dataListView.Columns[columnRun.DisplayIndex].Width = -1;

                int width = Math.Max(Math.Max(dataListView.Columns[columnPrescribed.DisplayIndex].Width,
                                              dataListView.Columns[columnSet.DisplayIndex].Width),
                                     dataListView.Columns[columnRun.DisplayIndex].Width);

                dataListView.Columns[columnPrescribed.DisplayIndex].Width = width;
                dataListView.Columns[columnSet.DisplayIndex].Width = width;
                dataListView.Columns[columnRun.DisplayIndex].Width = width;

                widthSet = true;
            }

            if (recorder != null)
            {
                data.Save(recorder);
            }

            Log("Data {0}/{1}, seq {2}, {3}, [{4}]", data.State, IComApi.StateToString(data.State), data.SeqNumber.ToString("x4"), DateTimeStrings.TimeMs(data.Dt), data.InhibitCount);
        }         * */

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

        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            Disconnect();
        }


        static EventWaitHandle _waitHandle = new AutoResetEvent(false);
        delegate void UIDelegate(string message);

        private void SimulateTable()
        {
            for (int i = 0; i < 10; i++)
            {
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
