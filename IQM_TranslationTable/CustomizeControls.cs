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

        public static void UpdateDataGridView(this DataGridView dataGridView,
            Dictionary<string, int> motor1Values, Dictionary<string, int> motor2Values)
        {
            /* Extension method to update DataGridView cell values.*/

            string headerName; // headerName is used to get the value corresponding to the key

            foreach (DataGridViewRow rows in dataGridView.Rows)
            {
                headerName = rows.HeaderCell.Value.ToString();

                rows.Cells[0].Value = motor1Values[headerName];
                rows.Cells[1].Value = motor2Values[headerName];
            }
        }
    }
}
