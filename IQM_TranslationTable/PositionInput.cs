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

namespace IQM_TranslationTable
{
    public partial class PositionInput : Form
    {
        IQM_TranslationTable form;
        LogStream log;
        List<Tuple<int, int>> pairList;
        Thread moveThread;
        TransTableMotor motor1, motor2;

        public PositionInput(IQM_TranslationTable form, LogStream log)
        {
            this.form = form;
            this.log = log;
            InitializeComponent();
            motor1 = form.CSM.motor1;
            motor2 = form.CSM.motor2;
        }

        private void inputButton_Click(object sender, EventArgs e)
        {
            try
            {
                pairList = Utils.parsePairListText(inputRichTextBox.Text);
                displayRichTextBox.AppendText("Input parsed:\n" + 
                    Utils.parsePairList(pairList) + "\n\n");
            }
            catch (Exception ex)
            {
                displayRichTextBox.AppendText("Error occured while parsing the input string: \n\n" +
                    ex.ToString() + "\n");
            }

            moveButton.Enabled = true;
            pauseButton.Enabled = true;
        }

        private void moveButton_Click(object sender, EventArgs e)
        {
            moveThread = new Thread(moveNext);
            moveThread.Start();
        }

        delegate void MoveNextCallBack();

        private void moveNext()
        {
            if (displayRichTextBox.InvokeRequired)
            {
                MoveNextCallBack d = new MoveNextCallBack(moveNext);
                this.Invoke(d, new object[] {});
            }
            else
            {
                Tuple<int, int> pair = pairList[0];

                motor1.SetSteps(pair.Item1 - motor1.CurrentRelPosition);
                motor2.SetSteps(pair.Item2 - motor2.CurrentRelPosition);

                motor1.StartTravelProfile();
                motor1.WaitMotor();

                Thread.Sleep(100);

                motor2.StartTravelProfile();
                motor2.WaitMotor();

                pairList.RemoveAt(0);

                displayRichTextBox.AppendText("Remaining position pair queue:\n" +
                        Utils.parsePairList(pairList) + "\n\n");
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (moveThread != null)
            {
                moveThread.Abort();
            }

            motor1.StopTravelProfile();
            motor2.StopTravelProfile();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (moveThread != null)
            {
                moveThread.Abort();
            }

            motor1.StopTravelProfile();
            motor2.StopTravelProfile();

            pairList = null;

            displayRichTextBox.AppendText("Stopping measurements.\n\n");

            moveButton.Enabled = false;
            pauseButton.Enabled = false;

            form.CSM.OnMotor1ProfileEnded(EventArgs.Empty);
            form.CSM.OnMotor2ProfileEnded(EventArgs.Empty);
        }
    }
}
