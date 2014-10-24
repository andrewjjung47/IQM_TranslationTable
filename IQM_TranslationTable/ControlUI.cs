using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQM_TranslationTable
{
    public class ControlUI
    {
        private IQM_TranslationTable form;
        private TranslationTable CSM;
        public Dictionary<string, dynamic> motor1Settings;
        public Dictionary<string, dynamic> motor2Settings;
        public Dictionary<string, dynamic> motor1Record;
        public Dictionary<string, dynamic> motor2Record;

        /* Initialize windows form controls added */
        public ControlUI(IQM_TranslationTable form, TranslationTable CSM)
        {
            this.form = form;
            this.CSM = CSM;

            motor1Settings = new Dictionary<string, dynamic>();
            motor2Settings = new Dictionary<string, dynamic>();

            motor1Record = new Dictionary<string, dynamic>();
            motor2Record = new Dictionary<string, dynamic>();

            // Initialize the datagrid MotorSettings
            this.form.MotorSettings.InitializeMotorSettings(motor1Settings, motor2Settings);

            // Initialize the datagrid realRecordsettings
            this.form.RecordSettings.InitializeRecordSettings(motor1Record, motor2Record);

            // Initialize COMDropDown
            this.form.COMDropDown.Items.AddRange(SerialPort.GetPortNames());

            // Initialize BaudrateDropDown
            this.form.BaudrateDropDown.SelectedIndex = 4;
        }

        public void ReadMotor1Settings()
        {
            // Store keys in a string list
            List<string> keys = new List<string>(motor1Settings.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                motor1Settings[keys[i]] = Convert.ToInt32(form.MotorSettings.Rows[i].Cells[0].Value.ToString());
            }
        }

        public void ReadMotor2Settings()
        {
            // Store keys in a string list
            List<string> keys = new List<string>(motor2Settings.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                motor2Settings[keys[i]] = Convert.ToInt32(form.MotorSettings.Rows[i].Cells[1].Value.ToString());
            }
        }

        public void ReadMotor1Record()
        {
            // Store keys in a string list
            List<string> keys = new List<string>(motor1Record.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                motor1Record[keys[i]] = Convert.ToInt32(form.RecordSettings.Rows[i].Cells[0].Value.ToString());
            }
        }

        public void ReadMotor2Record()
        {
            // Store keys in a string list
            List<string> keys = new List<string>(motor2Record.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                motor2Record[keys[i]] = Convert.ToInt32(form.RecordSettings.Rows[i].Cells[1].Value.ToString());
            }
        }

        public void QueryMotorSettings()
        {
            /* Read motor settings and display them on datagridview. */

            form.MotorSettings.UpdateDataGridView(CSM.motor1.MotorSettings(), 
                CSM.motor2.MotorSettings());
        }

        public void QueryRecordSettings()
        {
            /* Read record settings and display them on datagridview. */

            // Read record number
            form.RecordNumTextBox.Text = CSM.motor1.RecordNum.ToString();

            form.RecordSettings.UpdateDataGridView(CSM.motor1.RecordSettings(), CSM.motor2.RecordSettings());
        }
    }
}
