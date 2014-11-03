using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Commands;

namespace IQM_TranslationTable
{
    public class TranslationTable
    {
        public MyComMotorCommands motor1;
        public MyComMotorCommands motor2;
        private IQM_TranslationTable form;

        public static string logDirectory;

        public TranslationTable(IQM_TranslationTable form)
        {
            motor1 = new MyComMotorCommands(form, form.Motor1AbsPosTextBox, form.Motor1RelPosTextBox, 
                form.Motor1StatusLabel);
            motor2 = new MyComMotorCommands(form, form.Motor2AbsPosTextBox, form.Motor2RelPosTextBox,
                form.Motor2StatusLabel);

            // Set motor address
            motor1.MotorAddresse = 1;
            motor2.MotorAddresse = 2;

            this.form = form;
        }

        public void Initialize()
        {
            // Set COM port
            motor1.SelectedPort = form.COMDropDown.SelectedItem.ToString();
            motor2.SelectedPort = form.COMDropDown.SelectedItem.ToString();

            // Set baudrate
            motor1.Baudrate = Convert.ToInt32(form.BaudrateDropDown.SelectedItem.ToString());
            motor2.Baudrate = Convert.ToInt32(form.BaudrateDropDown.SelectedItem.ToString());

            // Read motor settings from datagrid
            form.UI.ReadMotor1Settings();
            form.UI.ReadMotor2Settings();

            // Set step mode
            motor1.MySetStepMode(form.UI.motor1Settings["Step Mode"]);
            motor2.MySetStepMode(form.UI.motor2Settings["Step Mode"]);

            // Set phase current
            motor1.SetPhaseCurrent(form.UI.motor1Settings["PhaseCurrent"]);
            motor2.SetPhaseCurrent(form.UI.motor2Settings["PhaseCurrent"]);

            // Set current reduction
            motor1.SetCurrentReduction(form.UI.motor1Settings["CurrentReduct"]);
            motor2.SetCurrentReduction(form.UI.motor2Settings["CurrentReduct"]);

            // Set bounce back at limit switch
            motor1.SetLimitSwitchBehavior(2, 8, 1024, 4096);
            motor2.SetLimitSwitchBehavior(2, 8, 1024, 4096);

            // Set limit switch type as opener
            motor1.SetInputMaskEdge(458767);
            motor2.SetInputMaskEdge(458767);
        }

        public void LoadRecord()
        {
            motor1.RecordNum = Convert.ToInt32(form.RecordNumTextBox.Text);
            motor2.RecordNum = Convert.ToInt32(form.RecordNumTextBox.Text); 

            // Load run record
            motor1.ChooseRecord(motor1.RecordNum);
            motor2.ChooseRecord(motor2.RecordNum);

            // Set relative positioning mode
            motor1.SetPositionType(1);
            motor2.SetPositionType(1);

            // Read record settings from datagrid
            form.UI.ReadMotor1Record();
            form.UI.ReadMotor2Record();

            // Set motor direction
            motor1.SetDirection(form.UI.motor1Record["Direction"]);
            motor2.SetDirection(form.UI.motor2Record["Direction"]);

            // Set maximum speed in Hz
            motor1.MySetMaxFrequency(form.UI.motor1Record["Maximum Speed"]);
            motor2.MySetMaxFrequency(form.UI.motor2Record["Maximum Speed"]);

            // Set minimum speed in Hz
            motor1.MySetStartFrequency(form.UI.motor1Record["Minimum Speed"]);
            motor2.MySetStartFrequency(form.UI.motor2Record["Minimum Speed"]);

            // Set the ramp type
            motor1.SetRampType(form.UI.motor1Record["Ramp Type"]);
            motor2.SetRampType(form.UI.motor2Record["Ramp Type"]);

            // Set ramp acceleration in Hz/ms
            motor1.MySetRamp(form.UI.motor1Record["Acceleration"]);
            motor2.MySetRamp(form.UI.motor2Record["Acceleration"]);

            // Set brake deceleration in Hz/ms
            motor1.MySetBrakeRamp(form.UI.motor1Record["Brake"]);
            motor2.MySetBrakeRamp(form.UI.motor2Record["Brake"]);

            // Set repeat
            motor1.SetRepeat(1);
            motor2.SetRepeat(1);

            motor1.Repeat = form.UI.motor1Record["Repeat"];
            motor2.Repeat = form.UI.motor2Record["Repeat"];

            // Set travel distance
            motor1.MySetSteps(form.UI.motor1Record["Position Demand"]);
            motor2.MySetSteps(form.UI.motor2Record["Position Demand"]);

            // Set record number to use for CSM runs
            motor1.SetRecord(motor1.RecordNum);
            motor2.SetRecord(motor2.RecordNum);

            // Set settings for homing
            motor1.SetHoming();
            motor2.SetHoming();
        }

        public void CSM()
        {
            motor1.Home();
            motor2.Home();

            // String array to save position data
            string[] LogText = new string[motor1.Repeat * motor2.Repeat + 1];
            LogText[0] = "Motor2Pos Motor1Pos StartTime EndTime";

            for (int i = 0; i < motor2.Repeat; i++) // loop for motor2 movement
            {
                String tempPos = motor2.QueryCurrentPosition().ToString(); // temporary holder for motor2 position
                for (int j = 0; j < motor1.Repeat; j++) // loop for motor1 movement
                {
                    motor1.WaitEvent();

                    motor1.StartTravelProfile();
                    motor1.WaitMotor();

                    LogText[motor2.Repeat * i + j + 1] = tempPos + " " + motor1.GetPosition().ToString() + " " + DateTime.Now.ToString();
                    Thread.Sleep(1000);
                    LogText[motor1.Repeat * i + j + 1] += " " + DateTime.Now.ToString();
                }
                motor2.WaitEvent();

                motor2.StartTravelProfile();
                motor2.WaitMotor();
                
                motor1.Home();
            }

            motor2.Home();

            motor1.StatusEnd();
            motor2.StatusEnd();

            if (logDirectory != null)
            {
                System.IO.File.WriteAllLines(@logDirectory, LogText);
            }
        }
    }
}
