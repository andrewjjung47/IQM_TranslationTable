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
        // TODO: add speed to the log when the table is moving 
        private Logger logger;

        public TransTableMotor(LogStream log, string motorName)
        {
            logger = new Logger(log, motorName);
        }

        public MotorStatus status = MotorStatus.Stopped; // initial state

        /// <summary>
        /// Reference position set by the user.
        /// </summary>
        private int refPosition = 0;
        public int RefPosition
        {
            get { return refPosition; }
            private set
            {
                refPosition = value;
                CurrentRelPosition = currentAbsPosition - RefPosition;
            }
        }
        // TODO: figure out why used this
        public int SetRefPosition()
        {
            RefPosition = -GetPosition();

            logger.Log("Reference position set at " + RefPosition);

            return RefPosition;
        }

        /// <summary>
        /// Current position relative to the homing limit switch (absolute position used by controller). 
        /// Used to decrease the number of unnecessary GetPosition() query to the controller.
        /// </summary>
        private int currentAbsPosition;
        public int CurrentAbsPosition
        {
            get 
            {
                currentAbsPosition = -GetPosition(); // (-) to keep CurrentAbsPosition positive
                CurrentRelPosition = currentAbsPosition - RefPosition;
                return currentAbsPosition;
            }
        }

        /// <summary>
        /// Current position relative to the RefPosition.
        /// </summary>
        public int CurrentRelPosition
        { get; private set; }

        /// <summary>
        /// Number of repetition for a profile
        /// </summary>
        private int repeat;
        public int Repeat
        {
            get { return repeat;}
            set 
            {
                if (value < 1)
                {
                    ErrorMessage = "Repeat number must be greater than 0.";
                }
                else
                {
                    repeat = value;
                }
            }
        }

        /// <summary>
        /// Record number of a travel profile
        /// </summary>
        private int recordNum;
        public int RecordNum
        {
            get { return recordNum; }
            set 
            {
                if (value < 1 | value > 31)
                {
                    ErrorMessage = "Record number must be between 1 and 31.";
                }
                else
                {
                    recordNum = value;
                    homeRecordNum = recordNum + 1;
                }
            }
        }

        /// <summary>
        /// Record number of the homing profile. This is automatically set as recordNum + 1.
        /// </summary>
        private int homeRecordNum;

        /// <summary>
        /// Save step mode in a field to reduce necessary GetStepMode() query
        /// </summary>
        private int stepMode = -1;
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
        /// <summary>
        /// Extend the default from ComMotorCommand and update StepMode as well. 
        /// </summary>
        /// <param name="stepMode"></param>
        /// <returns></returns>
        public override bool SetStepMode(int stepMode)
        {
            this.stepMode = stepMode; 
            return base.SetStepMode(stepMode);
        }


        /* Motor status related commands */


        /// <summary>
        /// Wait for motor to finish movement. 
        /// </summary>
        public void WaitMotor()
        {
            while (GetStatusByte() % 2 == 0)
            {
                OnMotorMoving(new MotorStatusEventArg(Utils.ConvertStepsToDistance(CurrentAbsPosition, stepMode), 
                    Utils.ConvertStepsToDistance(CurrentRelPosition, stepMode)));
                Thread.Sleep(50);
            }

            OnMotorStopped(new MotorStatusEventArg(Utils.ConvertStepsToDistance(CurrentAbsPosition, stepMode), 
                Utils.ConvertStepsToDistance(CurrentRelPosition, stepMode)));
            Thread.Sleep(100); // gives 100ms break between movements
        }

        public override bool StartTravelProfile()
        {
            logger.Log(string.Format("Start moving at absolute position: {0}, relative position: {1}",
                CurrentAbsPosition, CurrentRelPosition));
            return base.StartTravelProfile();
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

            logger.Log("Start homing.");

            ChooseRecord(homeRecordNum);

            if (IsReferenced()) // Previously referenced and current position is known
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
            }
            Thread.Sleep(500); // give 500ms pause after the homing.

            ChooseRecord(RecordNum); // revert back to run record

            logger.Log("Finish homing");
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
            recordSettings["Repeat"] = Repeat;
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
            status = MotorStatus.Moving;
        }

        protected virtual void OnMotorStopped(MotorStatusEventArg e)
        {
            if (MotorStopped != null) MotorStopped(this, e);
            status = MotorStatus.Paused;

            logger.Log(string.Format("Stopped moving at absolute position: {0}, relative position: {1}",
                e.AbsPosition, e.RelPosition));
        }

        public event EventHandler<MotorStatusEventArg> MotorMoving;
        public event EventHandler<MotorStatusEventArg> MotorStopped;
    }


    public class MotorStatusEventArg : EventArgs
    {
        public readonly double AbsPosition;
        public readonly double RelPosition;

        public MotorStatusEventArg(double absPosition, double relPosition)
        {
            AbsPosition = absPosition;
            RelPosition = relPosition;
        }
    }

    public enum MotorStatus
    {
        Moving,
        Paused,
        Stopped
    }
}
