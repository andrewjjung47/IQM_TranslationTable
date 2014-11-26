using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQM_TranslationTable
{
    class UIHandle
    {
        private UpdateUI ui;

        // UI windows form.
        private IQM_TranslationTable form;

        delegate void PositionDelegate();

        delegate void StatusDelegate(string status);

                public MyComMotorCommands(IQM_TranslationTable form,
    TextBox absPosTextBox, TextBox relPosTextBox, Label statusLabel)
        {
            this.form = form;
            ui = new UpdateUI(this, form, absPosTextBox, relPosTextBox, statusLabel);
        }
    }

            class UpdateUI
        {
            MyComMotorCommands motor;
            IQM_TranslationTable form;
            TextBox absPosTextBox;
            TextBox relPosTextBox;
            Label statusLabel;

            public UpdateUI(MyComMotorCommands motor, IQM_TranslationTable form,
                TextBox absPosTextBox, TextBox relPosTextBox, Label statusLabel)
            {
                this.motor = motor;
                this.form = form;
                this.absPosTextBox = absPosTextBox;
                this.relPosTextBox = relPosTextBox;
                this.statusLabel = statusLabel;
            }

            public void UpdatePositions()
            {
                /* Update motor's current position to the motor's position text box. */
                if (motor.Referenced)
                {
                    if (absPosTextBox.InvokeRequired | relPosTextBox.InvokeRequired)
                    {
                        PositionDelegate d = new PositionDelegate(UpdatePositions);
                        form.Invoke(d, new object[] { });
                    }
                    else
                    {
                        absPosTextBox.Text = Math.Abs(motor.QueryCurrentPosition()).ToString();
                        relPosTextBox.Text = (Math.Abs(motor.CurrentPosition) - motor.RefPosition).ToString();
                    }
                }
            }

            public void UpdateStatus(string status)
            {
                if (statusLabel.InvokeRequired)
                {
                    StatusDelegate d = new StatusDelegate(UpdateStatus);
                    form.Invoke(d, new object[] { status });
                }
                else
                {
                    if (status == "moving")
                    {
                        statusLabel.Text = "Moving";
                        statusLabel.BackColor = System.Drawing.Color.LawnGreen;
                    }
                    else if (status == "paused")
                    {
                        statusLabel.Text = "Pause";
                        statusLabel.BackColor = System.Drawing.Color.Yellow;
                    }
                    else if (status == "end")
                    {
                        statusLabel.Text = "Command completed. Ready for next movement.";
                        statusLabel.BackColor = System.Drawing.SystemColors.GrayText;
                    }
                }
            }
        }

        public void StatusEnd()
        {
            ui.UpdateStatus("end");
        }
}
