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
    class PositionInput
    {
        private IQM_TranslationTable form;
        private LogStream log;
        private List<Tuple<int, int>> pairList;
        private TransTableMotor motor1, motor2;

        public PositionInput(IQM_TranslationTable form, LogStream log)
        {
            this.form = form;
            this.log = log;

            motor1 = form.CSM.motor1;
            motor2 = form.CSM.motor2;
        }

        public string input(string text)
        {
            pairList = Utils.parsePairListText(text); 
            return Utils.parsePairList(pairList);
        }

        public string move()
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

            return Utils.parsePairList(pairList);
        }

        public void pause()
        {
            motor1.StopTravelProfile();
            motor2.StopTravelProfile();
        }

        public void end()
        {
            motor1.StopTravelProfile();
            motor2.StopTravelProfile();

            pairList = null;
        }
    }
}
