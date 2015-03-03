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
using Commands;

namespace IQM_TranslationTable
{
    public partial class IQM_TranslationTable : Form
    {
        public TranslationTable CSM;
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

            CSM.motor1.MotorMoving += motor1_MotorMoving;
            CSM.motor2.MotorMoving += motor2_MotorMoving;

            CSM.motor1.MotorStopped += motor1_MotorStopped;
            CSM.motor2.MotorStopped += motor2_MotorStopped;

            CSM.Motor1ProfileEnded += CSM_Motor1ProfileEnded;
            CSM.Motor2ProfileEnded += CSM_Motor2ProfileEnded;

            Application.ApplicationExit += new EventHandler(OnApplicationExit);
        }

        private void logDirectoryButton_Click(object sender, EventArgs e)
        {
            DialogResult result = logFolderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                logFolderTextBox.Text = logFolderDialog.SelectedPath;
            }
        }

        private void InitializeButton_Click(object sender, EventArgs e)
        {
            // Ensures a COM port is selected
            if (comDropDown.SelectedItem == null)
            {
                MessageBox.Show("Please select an appropriate COM port or check serial port connection.",
                "COM Port Selection Cannot Be Empty!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);

                return;
            }

            if (!InitializeButtonClick)
            {
                try
                {
                    // Create C:\Temp\IQMLog if it does not exist
                    if (!Utils.EnsurePathExists(logFolderTextBox.Text))
                    {
                        return;
                    }

                    CSM.Initialize();
                    UI.QueryMotorSettings();

                    // Disable related control panels
                    InitializeButton.Text = "Change Motor Settings";
                    InitializeButtonClick = true;

                    motorSettings.ReadOnly = true;
                    comDropDown.Enabled = false;
                    baudrateDropDown.Enabled = false;
                    motorSettings.DefaultCellStyle.BackColor = SystemColors.Control;

                    // Start button enabled when both are clicked
                    if (InitializeButtonClick == true && LoadRecordButtonClick == true)
                    {
                        startButton.Enabled = true;
                        panel2.Enabled = true;
                    }

                    ResetError();
                }
                catch (SerialCommunicationException)
                {
                    ReportError();
                }
            }
            else
            {
                // Enable related control panels
                InitializeButton.Text = "Initialize Motor";
                InitializeButtonClick = false;

                motorSettings.ReadOnly = false;
                comDropDown.Enabled = true;
                baudrateDropDown.Enabled = true;
                motorSettings.DefaultCellStyle.BackColor = SystemColors.Window;
                startButton.Enabled = false;
                panel2.Enabled = false;

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

                    recordNumDropDown.Enabled = false;
                    recordSettings.ReadOnly = true;
                    recordSettings.DefaultCellStyle.BackColor = SystemColors.Control;

                    if (InitializeButtonClick == true && LoadRecordButtonClick == true)
                    {
                        startButton.Enabled = true;
                        panel2.Enabled = true;
                    }

                    ResetError();
                }
                catch (SerialCommunicationException)
                {
                    ReportError();
                }
            }
            else
            {
                loadRecordButton.Text = "Load Record Settings";
                LoadRecordButtonClick = false;

                recordNumDropDown.Enabled = true;
                recordSettings.ReadOnly = false;
                recordSettings.DefaultCellStyle.BackColor = SystemColors.Window;
                startButton.Enabled = false;
                panel2.Enabled = false;

                Status1TextBox.Text = "";
                Status2TextBox.Text = "";
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            CSMThread = new Thread(CSM.CSM);
            CSMThread.Start();
        }

        public void pauseButton_Click(object sender, EventArgs e)
        {
            if (CSMThread != null)
            {
                CSMThread.Abort();
            }

            CSM.motor1.StopTravelProfile();
            CSM.motor2.StopTravelProfile();

            CSM.OnMotor1ProfileEnded(EventArgs.Empty);
            CSM.OnMotor2ProfileEnded(EventArgs.Empty);
        }

        private void ReportError()
        {
            if (CSM.motor1.ErrorFlag)
            {
                Status1TextBox.Text = CSM.motor1.ErrorMessage;
            }
            else
            {
                Status1TextBox.Text = "";
            }
            if (CSM.motor2.ErrorFlag)
            {
                Status2TextBox.Text = CSM.motor2.ErrorMessage;
            }
            else
            {
                Status2TextBox.Text = "";
            }
        }

        private void ResetError()
        {
            CSM.motor1.ResetErrorFlag();
            CSM.motor2.ResetErrorFlag();
            ReportError(); // update error message
        }

        private void manualMoveButtonClick(int direction, int motorAddress)
        {
            TextBox motorStepsTextBox, motorSpeedTextBox;

            if (motorAddress == 1)
            {
                motorStepsTextBox = motor1StepsTextBox;
                motorSpeedTextBox = motor1SpeedTextBox;
            }
            else
            {
                motorStepsTextBox = motor2StepsTextBox;
                motorSpeedTextBox = motor2SpeedTextBox;
            }

            if (motorStepsTextBox.Text == "" || motorSpeedTextBox.Text == "")
            {
                MessageBox.Show("Travel steps and speed cannot be empty.",
                    "Invalid travel steps and speed!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
            }
            else
            {
                Thread t = new Thread(() => CSM.ManualMove(Convert.ToInt32(motorStepsTextBox.Text),
                    Convert.ToInt32(motorSpeedTextBox.Text), direction, motorAddress));
                t.Start();
            }
        }

        private void motor1LeftButton_Click(object sender, EventArgs e)
        {
            manualMoveButtonClick(0, 1);
        }

        private void motor2LeftButton_Click(object sender, EventArgs e)
        {
            manualMoveButtonClick(0, 2);
        }

        private void motor1RightButton_Click(object sender, EventArgs e)
        {
            manualMoveButtonClick(1, 1);
        }

        private void motor2RightButton_Click(object sender, EventArgs e)
        {
            manualMoveButtonClick(1, 2);
        }

        private void motor1HomeButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => CSM.ManualHome(1));
            t.Start();
        }

        private void motor2HomeButton_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => CSM.ManualHome(2));
            t.Start();
        }

        private void motor1RefPositionButton_Click(object sender, EventArgs e)
        {
            CSM.motor1.SetRefPosition();
            Motor1RelPosTextBox.Text = "0";
        }

        private void motor2RefPositionButton_Click(object sender, EventArgs e)
        {
            CSM.motor2.SetRefPosition();
            Motor2RelPosTextBox.Text = "0";
        }

        delegate void MotorStatusCallBack(object sender, MotorStatusEventArg e);
        delegate void MotorStatusEndCallBack(object sender, EventArgs e);

        private void motor1_MotorMoving(object sender, MotorStatusEventArg e)
        {
            if (this.motor1StatusLabel.InvokeRequired || this.motor1AbsPosTextBox.InvokeRequired ||
                motor1RelPosTextBox.InvokeRequired)
            {
                MotorStatusCallBack d = new MotorStatusCallBack(motor1_MotorMoving);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                groupBox2.Enabled = false;

                motor1StatusLabel.Text = "Moving";
                motor1StatusLabel.BackColor = System.Drawing.Color.LawnGreen;

                motor1AbsPosTextBox.Text = e.AbsPosition.ToString();
                motor1RelPosTextBox.Text = e.RelPosition.ToString();
            }
        }

        private void motor2_MotorMoving(object sender, MotorStatusEventArg e)
        {
            if (this.motor2StatusLabel.InvokeRequired || this.motor2AbsPosTextBox.InvokeRequired ||
                motor2RelPosTextBox.InvokeRequired)
            {
                MotorStatusCallBack d = new MotorStatusCallBack(motor2_MotorMoving);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                groupBox3.Enabled = false;

                motor2StatusLabel.Text = "Moving";
                motor2StatusLabel.BackColor = System.Drawing.Color.LawnGreen;

                motor2AbsPosTextBox.Text = e.AbsPosition.ToString();
                motor2RelPosTextBox.Text = e.RelPosition.ToString();
            }
        }

        private void motor1_MotorStopped(object sender, MotorStatusEventArg e)
        {
            if (this.motor1StatusLabel.InvokeRequired || this.motor1AbsPosTextBox.InvokeRequired ||
                motor1RelPosTextBox.InvokeRequired)
            {
                MotorStatusCallBack d = new MotorStatusCallBack(motor1_MotorStopped);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                motor1StatusLabel.Text = "Paused";
                motor1StatusLabel.BackColor = System.Drawing.Color.Yellow;

                motor1AbsPosTextBox.Text = e.AbsPosition.ToString();
                motor1RelPosTextBox.Text = e.RelPosition.ToString();
            }
        }

        private void motor2_MotorStopped(object sender, MotorStatusEventArg e)
        {
            if (this.motor2StatusLabel.InvokeRequired || this.motor2AbsPosTextBox.InvokeRequired ||
                motor2RelPosTextBox.InvokeRequired)
            {
                MotorStatusCallBack d = new MotorStatusCallBack(motor2_MotorStopped);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                motor2StatusLabel.Text = "Paused";
                motor2StatusLabel.BackColor = System.Drawing.Color.Yellow;

                motor2AbsPosTextBox.Text = e.AbsPosition.ToString();
                motor2RelPosTextBox.Text = e.RelPosition.ToString();
            }
        }

        private void CSM_Motor1ProfileEnded(object sender, EventArgs e)
        {
            if (this.motor1StatusLabel.InvokeRequired)
            {
                MotorStatusEndCallBack d = new MotorStatusEndCallBack(CSM_Motor1ProfileEnded);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                groupBox2.Enabled = true;

                motor1StatusLabel.Text = "Profile ended";
                motor1StatusLabel.BackColor = System.Drawing.SystemColors.GrayText;
            }
        }

        private void CSM_Motor2ProfileEnded(object sender, EventArgs e)
        {
            if (this.motor2StatusLabel.InvokeRequired)
            {
                MotorStatusEndCallBack d = new MotorStatusEndCallBack(CSM_Motor2ProfileEnded);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                groupBox3.Enabled = true;

                motor2StatusLabel.Text = "Profile ended";
                motor2StatusLabel.BackColor = System.Drawing.SystemColors.GrayText;
            }
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            ComMotorCommands.ClosePort();
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

        public ComboBox RecordNumDropDown
        {
            get { return recordNumDropDown; }
        }

        public Label Motor1StatusLabel
        {
            get { return motor1StatusLabel; }
        }

        public Label Motor2StatusLabel
        {
            get { return motor2StatusLabel; }
        }

        public DataGridView MeasurementDataGridView
        {
            get { return dataGridView1; }
        }

        public EventWaitHandle _move = new AutoResetEvent(false);
        public bool inputFlag = true;

        public void moveEvent()
        {
            if (inputFlag)
            {
                inputFlag = false;
                _move.Set();
            }
        }

        private void moveEventButton_Click(object sender, EventArgs e)
        {
            moveEvent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            iCOMTest logTest = new iCOMTest(this);
            logTest.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CommandLine commandLine = new CommandLine(this);
            commandLine.Show();
        }

        private void clearGridButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }
    }
}
