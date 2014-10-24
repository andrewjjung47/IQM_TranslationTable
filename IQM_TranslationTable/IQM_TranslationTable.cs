using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Command;
using CommandsPD4I;

namespace IQM_TranslationTable
{
    public partial class IQM_TranslationTable : Form
    {
        private TranslationTable CSM;
        private Thread CSMThread;
        public ControlUI UI
        {
            get;
            private set;
        }

        private bool InitializeButtonClick = false;
        private bool LoadRecordButtonClick = false;

        public IQM_TranslationTable()
        {
            InitializeComponent();

            CSM = new TranslationTable(this);

            UI = new ControlUI(this, CSM);
        }

        private void logDirectoryButton_Click(object sender, EventArgs e)
        {
            DialogResult result = logFolderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                logFolderTextBox.Text = logFolderDialog.SelectedPath;
                TranslationTable.logDirectory = logFolderDialog.SelectedPath;
            }
        }

        private void InitializeButton_Click(object sender, EventArgs e)
        {
            if (!InitializeButtonClick)
            {
                try
                {
                    CSM.Initialize();
                    UI.QueryMotorSettings();

                    InitializeButton.Text = "Change Motor Settings";
                    InitializeButtonClick = true;

                    motorSettings.ReadOnly = true;
                    comDropDown.Enabled = false;
                    baudrateDropDown.Enabled = false;
                    motorSettings.DefaultCellStyle.BackColor = SystemColors.Control;

                    reportError();
                }
                catch (NullReferenceException)
                {
                    Status1TextBox.Text = "Communication error.";
                    Status2TextBox.Text = "Communication error.";

                    if (comDropDown.SelectedItem == null)
                    {
                        MessageBox.Show("Please select an appropriate COM port or check serial port connection.",
                        "COM Port Selection Cannot Be Empty!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1);
                    }
                }
            }
            else
            {
                InitializeButton.Text = "Initialize Motor";
                InitializeButtonClick = false;

                motorSettings.ReadOnly = false;
                comDropDown.Enabled = true;
                baudrateDropDown.Enabled = true;
                motorSettings.DefaultCellStyle.BackColor = SystemColors.Window;

                Status1TextBox.Text = "";
                Status2TextBox.Text = "";
            }
        }

        private void loadRecordButton_Click(object sender, EventArgs e)
        {
            if (!LoadRecordButtonClick)
            {
                try
                {
                    CSM.LoadRecord();
                    UI.QueryRecordSettings();

                    loadRecordButton.Text = "Change Record Settings";
                    LoadRecordButtonClick = true;

                    recordNumTextBox.ReadOnly = true;
                    recordSettings.ReadOnly = true;
                    recordSettings.DefaultCellStyle.BackColor = SystemColors.Control;
                    
                    reportError();
                }
                catch (NullReferenceException)
                {
                    Status1TextBox.Text = "Communication error.";
                    Status2TextBox.Text = "Communication error.";
                }
                catch (ArgumentNullException)
                {
                    Status1TextBox.Text = "Communication error.";
                    Status2TextBox.Text = "Communication error.";
                }

            }
            else
            {
                loadRecordButton.Text = "Load Record Settings";
                LoadRecordButtonClick = false;

                recordNumTextBox.ReadOnly = false;
                recordSettings.ReadOnly = false;
                recordSettings.DefaultCellStyle.BackColor = SystemColors.Window;

                Status1TextBox.Text = "";
                Status2TextBox.Text = "";
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            CSMThread = new Thread(CSM.CSM);
            CSMThread.Start();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            // CSMThread.Abort();

            CSM.motor1.StopTravelProfile();
            CSM.motor2.StopTravelProfile();
        }

        private void reportError()
        {
            if (CSM.motor1.ErrorFlag)
            {
                Status1TextBox.Text = CSM.motor1.ErrorMessageString;
            }
            else
            {
                Status1TextBox.Text = " OK";
            }
            if (CSM.motor2.ErrorFlag)
            {
                Status2TextBox.Text = CSM.motor2.ErrorMessageString;
            }
            else
            {
                Status2TextBox.Text = " OK";
            }
        }

        private void motor1LeftButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => CSM.motor1.ManualMove(Convert.ToInt32(motor1StepsTextBox.Text),
                Convert.ToInt32(motor1SpeedTextBox.Text),
                0));
            t.Start();
        }

        private void motor2LeftButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => CSM.motor2.ManualMove(Convert.ToInt32(motor2StepsTextBox.Text),
                Convert.ToInt32(motor2SpeedTextBox.Text),
                0));
            t.Start();
        }

        private void motor1RightButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => CSM.motor1.ManualMove(Convert.ToInt32(motor1StepsTextBox.Text),
                Convert.ToInt32(motor1SpeedTextBox.Text),
                1));
            t.Start();
        }

        private void motor2RightButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => CSM.motor2.ManualMove(Convert.ToInt32(motor2StepsTextBox.Text),
                Convert.ToInt32(motor2SpeedTextBox.Text),
                1));
            t.Start();
        }

        private void motor1HomeButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(CSM.motor1.ManualHome);
            t.Start();

        }

        private void motor2HomeButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(CSM.motor2.ManualHome);
            t.Start();
        }

        private void motor1RefPositionButton_Click(object sender, EventArgs e)
        {
            CSM.motor1.RefPosition = CSM.motor1.QueryCurrentPosition();
            Motor1RelPosTextBox.Text = "0";
        }

        private void motor2RefPositionButton_Click(object sender, EventArgs e)
        {
            CSM.motor2.RefPosition = CSM.motor2.QueryCurrentPosition();
            Motor2RelPosTextBox.Text = "0";
        }

        public ComboBox COMDropDown
        {
            get { return comDropDown; }
        }

        public ComboBox BaudrateDropDown
        {
            get { return baudrateDropDown; }
        }

        public DataGridView MotorSettings
        {
            get { return motorSettings; }
        }

        public DataGridView RecordSettings
        {
            get { return recordSettings; }
        }

        public TextBox Motor1AbsPosTextBox
        {
            get { return motor1AbsPosTextBox; }
        }

        public TextBox Motor2AbsPosTextBox
        {
            get { return motor2AbsPosTextBox; }
        }

        public TextBox Motor1RelPosTextBox
        {
            get { return motor1RelPosTextBox; }
        }

        public TextBox Motor2RelPosTextBox
        {
            get { return motor2RelPosTextBox; }
        }

        public TextBox RecordNumTextBox
        {
            get { return recordNumTextBox; }
        }

        public Label Motor1StatusLabel
        {
            get { return motor1StatusLabel; }
        }

        public Label Motor2StatusLabel
        {
            get { return motor2StatusLabel; }
        }

        public EventWaitHandle _move = new AutoResetEvent(false);

        private void moveEventButton_Click(object sender, EventArgs e)
        {
            _move.Set();
        }
    }
}
