using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IQM_TranslationTable
{
    static class CustomizeControls
    {
        /* Methods to customize controls */

        public static void InitializeMotorSettings(this DataGridView motorSettings,
            Dictionary<string, dynamic> motor1Settings, Dictionary<string, dynamic> motor2Settings)
        {
            /* Extension method to initialize the DataGridView motorSettings */

            string[] HeaderNames = { "Step Mode", "PhaseCurrent", "CurrentReduct" };

            foreach (string key in HeaderNames)
            {
                motor1Settings.Add(key, -1);
                motor2Settings.Add(key, -1);
            }

            dynamic[] Motor1Values = { 1, 40, 10 };
            dynamic[] Motor2Values = { 1, 50, 10 };
            motorSettings.InitializeDataGridView(HeaderNames, Motor1Values, Motor2Values);
        }

        public static void InitializeRecordSettings(this DataGridView recordSettings,
            Dictionary<string, dynamic> motor1Record, Dictionary<string, dynamic> motor2Record)
        {
            /* Extension method to initialize the DataGridView recordSettings */

            string[] HeaderNames = {"Direction", "Maximum Speed", "Minimum Speed", "Ramp Type", 
                                    "Acceleration", "Brake", "Repeat", "Position Demand" };

            foreach (string key in HeaderNames)
            {
                motor1Record.Add(key, -1);
                motor2Record.Add(key, -1);
            }

            dynamic[] Motor1Values = { 0, 10, 2, 0, 62, 62, 3, 10 };
            dynamic[] Motor2Values = { 0, 10, 2, 0, 62, 62, 2, 10 };
            recordSettings.InitializeDataGridView(HeaderNames, Motor1Values, Motor2Values);
        }

        private static void InitializeDataGridView(this DataGridView dataGridView,
            string[] headerNames, dynamic[] motor1Values, dynamic[] motor2Values)
        {
            /* Extension method to initialize DataGridView header cells and preset values.*/

            int rowLength = headerNames.Length;
            dataGridView.Rows.Add(rowLength);
            for (int i = 0; i < rowLength; i++)
            {
                dataGridView.Rows[i].HeaderCell.Value = headerNames[i];
                dataGridView.Rows[i].Cells[0].Value = motor1Values[i];
                dataGridView.Rows[i].Cells[1].Value = motor2Values[i];
            }
        }

        public static void UpdateDataGridView(this DataGridView dataGridView,
            dynamic[] motor1Values, dynamic[] motor2Values)
        {
            /* Extension method to update DataGridView cell values.*/

            int rowLength = motor1Values.Length;
            for (int i = 0; i < rowLength; i++)
            {
                dataGridView.Rows[i].Cells[0].Value = motor1Values[i];
                dataGridView.Rows[i].Cells[1].Value = motor2Values[i];
            }
        }
    }
}
