using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Commands;

namespace IQM_TranslationTable
{
    public class MyComMotorCommands : Conversion
    {
        private UpdateUI ui;

        // UI windows form.
        private IQM_TranslationTable form;

        delegate void PositionDelegate();

        delegate void StatusDelegate(string status);

        // Reference position set by the user.
        public int RefPosition
        { get; set; }

        // Current position relative to the RefPosition.
        public int CurrentPosition
        { get; private set; }

        // Whether the table has been homed and aware of its position or not
        public bool Referenced 
        { get; private set; }

        // Number of repeat of a travel profile
        private int repeat;
        public int Repeat
        {
            get { return repeat;}
            set 
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Repeat number must be greater than 0.");
                }
                else
                {
                    repeat = value;
                }
            }
        }

        // Record number of the travel profile
        private int recordNum;
        public int RecordNum
        {
            get { return recordNum; }
            set 
            {
                if (value < 1 | value > 31)
                {
                    throw new ArgumentOutOfRangeException("Record number must be between 1 and 31.");
                }
                else
                {
                    recordNum = value;
                    homeRecordNum = recordNum + 1;
                }
            }
        }

        // Record number of the homing profile. This is automatically set as recordNum + 1.
        private int homeRecordNum;

        public MyComMotorCommands(IQM_TranslationTable form,
    TextBox absPosTextBox, TextBox relPosTextBox, Label statusLabel)
        {
            this.form = form;
            ui = new UpdateUI(this, form, absPosTextBox, relPosTextBox, statusLabel);
        }

        /* Customized commands */

        public void MySetStepMode(int stepMode)
        {
            SetStepMode(stepMode);
            this.StepMode = stepMode;
        }

        public void MySetSteps(double steps)
        {
            SetSteps(ConvertMMToSteps(steps));
        }

        public double MyGetSteps(int recordNum)
        {
            return ConvertStepsToMM(GetSteps(recordNum));
        }

        public void MySetMaxFrequency(double speed)
        {
            SetMaxFrequency(ConvertMMToSteps(speed));
        }

        public double MyGetMaxFrequency(int recordNum)
        {
            return ConvertStepsToMM(GetMaxFrequency(recordNum));
        }

        public void MySetStartFrequency(double speed)
        {
            SetStartFrequency(ConvertMMToSteps(speed));
        }

        public double MyGetStartFrequency(int recordNum)
        {
            return ConvertStepsToMM((int)GetStartFrequency(recordNum));
        }

        public void MySetRamp(double ramp)
        {
            SetRamp(ConvertToAccelParam(ramp));
        }

        public double MyGetRamp(int recordNum)
        {
            return ConvertToHzMs(GetRamp(recordNum));
        }

        public void MySetBrakeRamp(double brake)
        {
            SetBrakeRamp(ConvertToAccelParam(brake));
        }

        public double MyGetBrakeRamp(int recordNum)
        {
            return ConvertToHzMs(GetBrakeRamp(recordNum));
        }

        public int QueryCurrentPosition()
        {
            /* Query motor's current position */
            CurrentPosition = -GetPosition();
            return CurrentPosition;
        }

        class UpdateUI
        {
            MyComMotorCommands motor;
            IQM_TranslationTable form;
            TextBox absPosTextBox;
            TextBox relPosTextBox;
            Label statusLabel;

            public UpdateUI(MyComMotorCommands motor, IQM_TranslationTable form,
                TextBox absPosTextBox, TextBox relPosTextBox, Label statusLabel)
            {
                this.motor = motor;
                this.form = form;
                this.absPosTextBox = absPosTextBox;
                this.relPosTextBox = relPosTextBox;
                this.statusLabel = statusLabel;
            }

            public void UpdatePositions()
            {
                /* Update motor's current position to the motor's position text box. */
                if (motor.Referenced)
                {
                    if (absPosTextBox.InvokeRequired | relPosTextBox.InvokeRequired)
                    {
                        PositionDelegate d = new PositionDelegate(UpdatePositions);
                        form.Invoke(d, new object[] { });
                    }
                    else
                    {
                        absPosTextBox.Text = Math.Abs(motor.QueryCurrentPosition()).ToString();
                        relPosTextBox.Text = (Math.Abs(motor.CurrentPosition) - motor.RefPosition).ToString();
                    }
                }
            }

            public void UpdateStatus(string status)
            {
                if (statusLabel.InvokeRequired)
                {
                    StatusDelegate d = new StatusDelegate(UpdateStatus);
                    form.Invoke(d, new object[] { status });
                }
                else
                {
                    if (status == "moving")
                    {
                        statusLabel.Text = "Moving";
                        statusLabel.BackColor = System.Drawing.Color.LawnGreen;
                    }
                    else if (status == "paused")
                    {
                        statusLabel.Text = "Pause";
                        statusLabel.BackColor = System.Drawing.Color.Yellow;
                    }
                    else if (status == "end")
                    {
                        statusLabel.Text = "Command completed. Ready for next movement.";
                        statusLabel.BackColor = System.Drawing.SystemColors.GrayText;
                    }
                }
            }
        }

        public void StatusEnd()
        {
            ui.UpdateStatus("end");
        }

        public void WaitMotor()
        {
            /* Wait for motor to finish movement. Update the motor position and its status label. */

            while (GetStatusByte() % 2 == 0)
            {
                ui.UpdateStatus("moving");
                ui.UpdatePositions();
                Thread.Sleep(50);
            }
            ui.UpdatePositions();
            ui.UpdateStatus("paused");
            Thread.Sleep(500); // gives 1000ms break between movements

            form.inputFlag = true;
        }

        public void SetHoming()
        {
            /* Initialize settings for homing. */

            // Choose homing record
            ChooseRecord(homeRecordNum);

            // Set motor direction to right
            SetDirection(1);

            // Set ramp acceleration in Hz/ms
            SetRamp(19113);

            // Set brake deceleration in Hz/ms
            SetBrakeRamp(19113);

            // Set the ramp type as trapezoidal
            SetRampType(0);

            // Set the number of repeats
            SetRepeat(1);

            SetRecord(homeRecordNum);
        }

        public void Home()
        {
            /* Rehomes the motor. When it has not been referenced prviously, performs fast external 
             * reference run to the limit switch, moves away from the switch 1cm, and moves slowly back 
             * towards the switch in external reference run. If the motor was referenced before,
             * the table is moved to 1cm position away from the switch in fast speed and external reference 
             * run is performed at slow speed. */

            ChooseRecord(homeRecordNum); 

            if (Referenced) // Previously referenced and current position is known
            {
                if (QueryCurrentPosition() > 1600)
                {
                    SetPositionType(1); // Relative positioning mode

                    SetMaxFrequency(1600); // in Hz
                    SetStartFrequency(200);

                    SetSteps(CurrentPosition - 1600); // travel most of the distance in fast speed

                    StartTravelProfile(); // start the fast relative position run

                    WaitMotor();
                }

                SetPositionType(4); // external reference run
                SetStartFrequency(200); // in Hz
                SetMaxFrequency(500);

                StartTravelProfile(); // start the slow reference run

                WaitMotor();
            }
            else // Has not been referenced and current position is not known
            {
                SetPositionType(4); // external reference run
                SetMaxFrequency(1600); // in Hz
                SetStartFrequency(200);

                StartTravelProfile(); // start the fast reference run all the way

                WaitMotor();

                SetDirection(0); // set motor direction to left
                SetPositionType(1); // relative positioning mode
                SetSteps(1600);

                StartTravelProfile(); // move 1cm away from the limit switch

                WaitMotor();

                SetDirection(1); // set motor direction to right
                SetPositionType(4); // external reference run
                SetStartFrequency(200); // in Hz
                SetMaxFrequency(500);

                StartTravelProfile(); // start the slow reference run

                WaitMotor();

                Referenced = true;
            }
            ui.UpdatePositions();

            Thread.Sleep(2000); // give 5000ms pause after the homing.

            ChooseRecord(RecordNum); // revert back to run record
        }

        public void ManualHome()
        {
            Home();
            StatusEnd();
        }

        public void ManualMove(int steps, int speed, int direction)
        {
            ChooseRecord(RecordNum);

            SetMaxFrequency(speed);
            SetSteps(steps);
            SetDirection(direction);

            StartTravelProfile();
            WaitMotor();

            ChooseRecord(RecordNum);

            ui.UpdatePositions();
            StatusEnd();
        }

        public dynamic[] MotorSettings()
        {
            dynamic[] motorSettings = {GetStepMode(), 
                                      GetPhaseCurrent(), 
                                      GetCurrentReduction()};
            return motorSettings;
        }

        public dynamic[] RecordSettings()
        {
            dynamic[] recordSettings = {GetDirection(RecordNum),
                                    MyGetMaxFrequency(RecordNum),
                                    MyGetStartFrequency(RecordNum),
                                    GetRampType(RecordNum),
                                    MyGetRamp(RecordNum),
                                    MyGetBrakeRamp(RecordNum),
                                    Repeat,
                                    MyGetSteps(RecordNum)};

            return recordSettings;
        }


        public void WaitEvent()
        {
            form._move.WaitOne();
        }
    }
}
