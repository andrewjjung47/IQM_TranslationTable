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
        private TransTableMotor motor1, motor2;
        public List<Tuple<int, int>> PairList
        {
            get;
            private set;
        }
        public int NumItems
        {
            get;
            private set;
        }
        

        public PositionInput(IQM_TranslationTable form, LogStream log)
        {
            this.form = form;
            this.log = log;

            motor1 = form.CSM.motor1;
            motor2 = form.CSM.motor2;
        }

        public string input(string text)
        {
            PairList = Utils.parsePairListText(text);
            NumItems = PairList.Count;
            return Utils.parsePairList(PairList);
        }

        public string move()
        {
            Tuple<int, int> pair = PairList[0];

            motor1.SetSteps(pair.Item1 - motor1.CurrentRelPosition);
            motor2.SetSteps(pair.Item2 - motor2.CurrentRelPosition);

            motor1.StartTravelProfile();
            motor1.WaitMotor();

            Thread.Sleep(100);

            motor2.StartTravelProfile();
            motor2.WaitMotor();

            PairList.RemoveAt(0);

            return Utils.parsePairList(PairList);
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

            PairList = null;
        }
    }
}
