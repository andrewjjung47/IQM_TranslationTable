using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;

namespace IQM_TranslationTable
{
    public class Conversion : ComMotorCommands
    {
        /* Converts units used in the controller of stepper motors into more 'human-friendly' units, 
         * or vice versa. */

        protected int StepMode
        {
            private get;
            set;
        }

        protected int ConvertMMToSteps(double mm)
        {
            /* Convert distance in 'mm' to 'steps'. 
             * Can also be used to convert speed in units 'mm/s' to 'Hz'.*/
            
            return (int) mm*160*StepMode;
        }

        protected double ConvertStepsToMM(int steps)
        {
            /* Convert distance in 'steps' to 'mm'. 
             * Can also be used to convert speed in units 'Hz' to 'mm/s'/ */
            
            return (double) steps / 160 * StepMode; 
        }

        protected int ConvertToAccelParam(double mm_ss)
        {
            /* Convert acceleration in mm/s^2 to parameter unit used by the controller. 
             * The equation used is Hz/ms = 3000.0 / sqrt((float)<parameter>) - 11.7 */

            return (int)Math.Pow(3000.0 / (ConvertMMToSteps(mm_ss)*0.001 + 11.7), 2);
        }

        protected double ConvertToHzMs(int accelParam)
        {
            /* Convert acceleration in unit used by controller to mm/s^2. 
             * The equation used is Hz/ms = 3000.0 / sqrt((float)<parameter>) - 11.7 */

            return ConvertStepsToMM((int)((3000.0 / Math.Sqrt((double)accelParam) - 11.7)*1000));
        }
    }
}
