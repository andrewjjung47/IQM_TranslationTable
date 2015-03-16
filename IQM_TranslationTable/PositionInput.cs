using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IQM_TranslationTable
{
    public partial class PositionInput : Form
    {
        IQM_TranslationTable form;
        List<Tuple<int, int>> pairList;

        public PositionInput(IQM_TranslationTable form)
        {
            this.form = form;
            InitializeComponent();
        }

        private void inputButton_Click(object sender, EventArgs e)
        {
            try
            {
                pairList = Utils.parsePairList(inputRichTextBox.Text);
                StringBuilder builder = new StringBuilder();
                foreach (Tuple<int, int> pair in pairList)
                {
                    builder.Append(pair.ToString()).Append(", ");
                }
                displayRichTextBox.AppendText("Input parsed:\n" + builder.ToString() + "\n\n");
            }
            catch (Exception ex)
            {
                displayRichTextBox.AppendText("Error occured while parsing the input string: \n\n" +
                    ex.ToString() + "\n");
            }
        }
    }


}
