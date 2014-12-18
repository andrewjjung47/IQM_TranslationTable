using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IQM_TranslationTable
{
    public class ControlUI
    {
        private IQM_TranslationTable form;
        private TranslationTable CSM;

        // These dictionaries are used to delegate information transfer between 
        // the UI and the motor controller. This is used to minimize hard coding. 
        public Dictionary<string, int> motor1Settings;
        public Dictionary<string, int> motor2Settings;
        public Dictionary<string, int> motor1Record;
        public Dictionary<string, int> motor2Record;

        /* Initialize windows form controls added */
        public ControlUI(IQM_TranslationTable form, TranslationTable CSM)
        {
            this.form = form;
            this.CSM = CSM;

            motor1Settings = new Dictionary<string, int>();
            motor2Settings = new Dictionary<string, int>();

            motor1Record = new Dictionary<string, int>();
            motor2Record = new Dictionary<string, int>();

            /* Initialize the datagrid MotorSettings */
            string[] motorHeaderNames = { "StepMode", "PhaseCurrent", "CurrentReduct" };

            // Default motor settings
            int[] motor1Values = { 1, 40, 1 };
            int[] motor2Values = { 1, 40, 1 };

            int rowLength = motorHeaderNames.Length;
            this.form.MotorSettings.Rows.Add(rowLength);

            for (int i = 0; i < rowLength; i++)
            {
                this.form.MotorSettings.Rows[i].HeaderCell.Value = motorHeaderNames[i];
                this.form.MotorSettings.Rows[i].Cells[0].Value = motor1Values[i];
                this.form.MotorSettings.Rows[i].Cells[1].Value = motor2Values[i];

                motor1Settings.Add(motorHeaderNames[i], motor1Values[i]);
                motor2Settings.Add(motorHeaderNames[i], motor2Values[i]);
            }

            /* Initialize the datagrid RecordSettings */
            string[] recordHeaderNames = {"Direction", "MaximumSpeed", "MinimumSpeed", "RampType", 
                "Acceleration", "Brake", "Repeat", "PositionDemand" };

            // Default record settings
            int[] record1Values = { 0, 1600, 200, 0, 19113, 19113, 3, 1600 };
            int[] record2Values = { 0, 1600, 200, 0, 19113, 19113, 1, 1600 };

            rowLength = recordHeaderNames.Length;
            this.form.RecordSettings.Rows.Add(rowLength);

            for (int i = 0; i < rowLength; i++)
            {
                this.form.RecordSettings.Rows[i].HeaderCell.Value = recordHeaderNames[i];
                this.form.RecordSettings.Rows[i].Cells[0].Value = record1Values[i];
                this.form.RecordSettings.Rows[i].Cells[1].Value = record2Values[i];

                motor1Record.Add(recordHeaderNames[i], record1Values[i]);
                motor2Record.Add(recordHeaderNames[i], record2Values[i]);
            }

            // Initialize COMDropDown
            this.form.COMDropDown.Items.AddRange(SerialPort.GetPortNames());

            // Initialize BaudrateDropDown
            this.form.BaudrateDropDown.SelectedIndex = 4;

            // Initialize 
            object[] recordNumRange = new object[31];
            int j = 0;
            foreach (int element in Enumerable.Range(1, 31).ToArray<int>())
            {
                recordNumRange[j] = (object)element;
                j++;
            }
            this.form.RecordNumDropDown.Items.AddRange(recordNumRange);
            this.form.RecordNumDropDown.SelectedIndex = 0;
        }

        public void ReadMotorSettings()
        {
            /* Read motor settings from UI and save them on motor1Settings and motor2Settings */ 

            string headerName; // headerName is used to get the value corresponding to the key

            foreach (DataGridViewRow rows in this.form.MotorSettings.Rows)
            {
                headerName = rows.HeaderCell.Value.ToString();
                motor1Settings[headerName] = Convert.ToInt32(rows.Cells[0].Value.ToString());
                motor2Settings[headerName] = Convert.ToInt32(rows.Cells[1].Value.ToString());
            }
        }

        public void ReadMotorRecord()
        {
            /* Read motor records from UI and save them on motor1Record and motor2Record */

            string headerName; // headerName is used to get the value corresponding to the key

            foreach (DataGridViewRow rows in this.form.RecordSettings.Rows)
            {
                headerName = rows.HeaderCell.Value.ToString();
                motor1Record[headerName] = Convert.ToInt32(rows.Cells[0].Value.ToString());
                motor2Record[headerName] = Convert.ToInt32(rows.Cells[1].Value.ToString());
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
            form.RecordNumDropDown.SelectedValue = CSM.motor1.RecordNum.ToString();

            form.RecordSettings.UpdateDataGridView(CSM.motor1.RecordSettings(), CSM.motor2.RecordSettings());
        }
    }
}
