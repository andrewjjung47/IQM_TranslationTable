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
    public class TransTableMotor : ComMotorCommands
    {
        // Whether the table has been homed and aware of its position or not
        public bool Referenced
        { get; private set; }

        // Reference position set by the user.
        public int RefPosition
        { get; private set; }
        public int SetRefPosition()
        {
            RefPosition = -GetPosition();
            return RefPosition;
        }

        // Current position relative to the homing limit switch (absolute position used by controller). 
        // Used to decrease the number of unnecessary GetPosition() query to the controller.
        private int currentAbsPosition;
        public int CurrentAbsPosition
        {
            get { return currentAbsPosition; }
            private set
            {
                currentAbsPosition = value;
                CurrentRelPosition = currentAbsPosition - RefPosition;
            }
        }

        // Current position relative to the RefPosition.
        public int CurrentRelPosition
        { get; private set; }

        // Number of repetition of the travel profile
        private int travelRepeat;
        public int TravelRepeat
        {
            get { return travelRepeat;}
            set 
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Repeat number must be greater than 0.");
                }
                else
                {
                    travelRepeat = value;
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

        public event EventHandler<MotorStatusEventArg> MotorMoving;
        public event EventHandler<MotorStatusEventArg> MotorStopped;

        private int stepMode = -1; // Save step mode in a field to reduce necessary GetStepMode() query
        public int StepMode
        {
            get 
            { 
                if (stepMode == -1) // when step mode is not known
                {
                    stepMode = GetStepMode();
                }
                return stepMode;
            }
        }
        public override bool SetStepMode(int stepMode)
        {
            this.stepMode = stepMode; 
            return base.SetStepMode(stepMode);
        }

        /* Motor status related commands */

        public int QueryCurrentAbsPosition()
        {
            // Query motor's current position. 

            CurrentAbsPosition = -GetPosition(); // (-) to keep CurrentAbsPosition positive
            return CurrentAbsPosition;
        }

        public void WaitMotor()
        {
            // Wait for motor to finish movement. 

            while (GetStatusByte() % 2 == 0)
            {
                OnMotorMoving(new MotorStatusEventArg(QueryCurrentAbsPosition(), CurrentRelPosition));
                Thread.Sleep(50);
            }

            OnMotorStopped(new MotorStatusEventArg(QueryCurrentAbsPosition(), CurrentRelPosition));
            Thread.Sleep(500); // gives 1000ms break between movements
        }


        /* Homing related commands */

        public void SetHoming()
        {
            // Initialize settings for homing.

            // Choose homing record
            ChooseRecord(homeRecordNum);

            // Set motor direction to right
            SetDirection(1);

            // Set ramp acceleration in Hz/ms
            SetRamp(ConvertAccelParam(19113, StepMode));

            // Set brake deceleration in Hz/ms
            SetBrakeRamp(ConvertAccelParam(19113, StepMode));

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
                // 1cm = 1600 steps for full step mode.

                if (CurrentAbsPosition > 1600 * StepMode)
                {
                    SetPositionType(1); // Relative positioning mode

                    SetMaxFrequency(1600 * StepMode); 
                    SetStartFrequency(200 * StepMode);

                    SetSteps(CurrentAbsPosition - 1600 * StepMode); // travel most of the distance in fast speed

                    StartTravelProfile(); // start the fast relative position run

                    WaitMotor();
                }

                SetPositionType(4); // external reference run
                SetStartFrequency(200 * StepMode); 
                SetMaxFrequency(300 * StepMode);

                StartTravelProfile(); // start the slow reference run

                WaitMotor();

                if (RefPosition != 0)
                {
                    SetPositionType(1);
                    SetDirection(0);
                    SetSteps(RefPosition);
                    SetMaxFrequency(1600 * StepMode);

                    StartTravelProfile();

                    WaitMotor();
                }
            }
            else // Has not been referenced and current position is not known
            {
                SetPositionType(4); // external reference run
                SetMaxFrequency(1600 * StepMode);
                SetStartFrequency(200 * StepMode);

                StartTravelProfile(); // start the fast reference run all the way

                WaitMotor();

                SetDirection(0); // set motor direction to left
                SetPositionType(1); // relative positioning mode
                SetSteps(1600 * StepMode);

                StartTravelProfile(); // move 1cm away from the limit switch

                WaitMotor();

                SetDirection(1); // set motor direction to right
                SetPositionType(4); // external reference run
                SetStartFrequency(200 * StepMode);
                SetMaxFrequency(300 * StepMode);

                StartTravelProfile(); // start the slow reference run

                WaitMotor();

                Referenced = true;
            }
            Thread.Sleep(500); // give 500ms pause after the homing.

            ChooseRecord(RecordNum); // revert back to run record
        }


        /* Outputs settings */

        public Dictionary<string, int> MotorSettings()
        {
            var motorSettings = new Dictionary<string, int>();
 
            motorSettings["StepMode"] = GetStepMode();
            motorSettings["PhaseCurrent"] = GetPhaseCurrent();
            motorSettings["CurrentReduct"] = GetCurrentReduction();

            return motorSettings;
        }

        public Dictionary<string, int> RecordSettings()
        {
            var recordSettings = new Dictionary<string, int>();

            recordSettings["Direction"] = GetDirection(RecordNum);
            recordSettings["MaximumSpeed"] = GetMaxFrequency(RecordNum);
            recordSettings["MinimumSpeed"] = GetStartFrequency(RecordNum);
            recordSettings["RampType"] = GetRampType(RecordNum);
            recordSettings["Acceleration"] = GetRamp(RecordNum);
            recordSettings["Brake"] = GetBrakeRamp(RecordNum);
            recordSettings["Repeat"] = TravelRepeat;
            recordSettings["PositionDemand"] = GetSteps(RecordNum);

            return recordSettings;
        }

        private int ConvertAccelParam(int param, int stepMode)
        {
            // Acceleration in Hz/ms = ((3000.0 / sqrt(<parameter>)) - 11.7).
            return (int)Math.Pow(3000.0 / ((3000.0 / Math.Sqrt(param) - 11.7) * stepMode + 11.7), 2);
        }

        protected virtual void OnMotorMoving(MotorStatusEventArg e)
        {
            if (MotorMoving != null) MotorMoving(this, e);
        }

        protected virtual void OnMotorStopped(MotorStatusEventArg e)
        {
            if (MotorStopped != null) MotorStopped(this, e);
        }
    }

    public class MotorStatusEventArg : EventArgs
    {
        public readonly int AbsPosition;
        public readonly int RelPosition;

        public MotorStatusEventArg(int absPosition, int relPosition)
        {
            AbsPosition = absPosition;
            RelPosition = relPosition;
        }
    }
}
