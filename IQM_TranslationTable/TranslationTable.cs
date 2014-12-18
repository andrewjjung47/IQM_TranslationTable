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
        public TransTableMotor motor1;
        public TransTableMotor motor2;
        private IQM_TranslationTable form;

        public static string logDirectory;

        // Used when all the movements are completed
        public event EventHandler Motor1ProfileEnded;
        public event EventHandler Motor2ProfileEnded;

        public TranslationTable(IQM_TranslationTable form)
        {
            motor1 = new TransTableMotor();
            motor2 = new TransTableMotor();

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
            form.UI.ReadMotorSettings();

            // Set step mode
            motor1.SetStepMode(form.UI.motor1Settings["StepMode"]);
            motor2.SetStepMode(form.UI.motor2Settings["StepMode"]);

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
            motor1.RecordNum = Convert.ToInt32(form.RecordNumDropDown.SelectedItem.ToString());
            motor2.RecordNum = Convert.ToInt32(form.RecordNumDropDown.SelectedItem.ToString()); 

            // Load run record
            motor1.ChooseRecord(motor1.RecordNum);
            motor2.ChooseRecord(motor2.RecordNum);

            // Set relative positioning mode
            motor1.SetPositionType(1);
            motor2.SetPositionType(1);

            // Read record settings from datagrid
            form.UI.ReadMotorRecord();

            // Set motor direction
            motor1.SetDirection(form.UI.motor1Record["Direction"]);
            motor2.SetDirection(form.UI.motor2Record["Direction"]);

            // Set maximum speed in Hz
            motor1.SetMaxFrequency(form.UI.motor1Record["MaximumSpeed"]);
            motor2.SetMaxFrequency(form.UI.motor2Record["MaximumSpeed"]);

            // Set minimum speed in Hz
            motor1.SetStartFrequency(form.UI.motor1Record["MinimumSpeed"]);
            motor2.SetStartFrequency(form.UI.motor2Record["MinimumSpeed"]);

            // Set the ramp type
            motor1.SetRampType(form.UI.motor1Record["RampType"]);
            motor2.SetRampType(form.UI.motor2Record["RampType"]);

            // Set ramp acceleration in Hz/ms
            motor1.SetRamp(form.UI.motor1Record["Acceleration"]);
            motor2.SetRamp(form.UI.motor2Record["Acceleration"]);

            // Set brake deceleration in Hz/ms
            motor1.SetBrakeRamp(form.UI.motor1Record["Brake"]);
            motor2.SetBrakeRamp(form.UI.motor2Record["Brake"]);

            // Set repeat
            motor1.SetRepeat(1);
            motor2.SetRepeat(1);

            motor1.TravelRepeat = form.UI.motor1Record["Repeat"];
            motor2.TravelRepeat = form.UI.motor2Record["Repeat"];

            // Set travel distance
            motor1.SetSteps(form.UI.motor1Record["PositionDemand"]);
            motor2.SetSteps(form.UI.motor2Record["PositionDemand"]);

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
            string[] LogText = new string[motor1.TravelRepeat * motor2.TravelRepeat + 1];
            LogText[0] = "Motor2Pos Motor1Pos StartTime EndTime";

            for (int i = 0; i < motor2.TravelRepeat; i++) // loop for motor2 movement
            {
                String tempPos = motor2.CurrentRelPosition.ToString(); // temporary holder for motor2 position
                for (int j = 0; j < motor1.TravelRepeat; j++) // loop for motor1 movement
                {
                    WaitEvent();

                    motor1.StartTravelProfile();
                    motor1.WaitMotor();

                    LogText[motor2.TravelRepeat * i + j + 1] = tempPos + " " + motor1.CurrentRelPosition.ToString() + " " + DateTime.Now.ToString();
                    Thread.Sleep(1000);
                    LogText[motor1.TravelRepeat * i + j + 1] += " " + DateTime.Now.ToString();

                    form.inputFlag = true;
                }

                WaitEvent();

                motor2.StartTravelProfile();
                motor2.WaitMotor();
                
                motor1.Home();
            }

            motor2.Home();

            OnMotor1ProfileEnded(EventArgs.Empty);
            OnMotor2ProfileEnded(EventArgs.Empty);

            if (logDirectory != null)
            {
                System.IO.File.WriteAllLines(@logDirectory, LogText);
            }
        }
        public void ManualHome(int motorNumber)
        {
            // The argument motorNumber is used to specify motor1 or motor2

            if (motorNumber == 1)
            {
                motor1.Home();
                OnMotor1ProfileEnded(EventArgs.Empty);
            }
            else if(motorNumber == 2)
            {
                motor2.Home();
                OnMotor2ProfileEnded(EventArgs.Empty);
            }
            else
            {
                throw new ArgumentOutOfRangeException("motorNumber must be either 1 or 2");
            }
        }

        public void ManualMove(int steps, int speed, int direction, int motorNumber)
        {
            // The argument motorNumber is used to specify motor1 or motor2

            TransTableMotor selectedMotor;

            if (motorNumber == 1)
            {
                selectedMotor = motor1;
            }
            else if (motorNumber == 2)
            {
                selectedMotor = motor2;
            }
            else
            {
                throw new ArgumentOutOfRangeException("motorNumber must be either 1 or 2");
            }

            selectedMotor.ChooseRecord(selectedMotor.RecordNum);

            selectedMotor.SetMaxFrequency(speed);
            selectedMotor.SetSteps(steps);
            selectedMotor.SetDirection(direction);

            selectedMotor.StartTravelProfile();
            selectedMotor.WaitMotor();

            selectedMotor.ChooseRecord(selectedMotor.RecordNum); // to revert the record back to RecordNum

            if (motorNumber == 1)
            {
                OnMotor1ProfileEnded(EventArgs.Empty);
            }
            else
            {
                OnMotor2ProfileEnded(EventArgs.Empty);
            }
        }

        protected virtual void OnMotor1ProfileEnded(EventArgs e)
        {
            if (Motor1ProfileEnded != null) Motor1ProfileEnded(this, e);
        }

        protected virtual void OnMotor2ProfileEnded(EventArgs e)
        {
            if (Motor2ProfileEnded != null) Motor2ProfileEnded(this, e);
        }

        public void WaitEvent()
        {
            form._move.WaitOne();
        }
    }
}
