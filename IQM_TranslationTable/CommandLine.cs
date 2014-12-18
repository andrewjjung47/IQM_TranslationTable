using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Commands;

namespace IQM_TranslationTable
{
    public partial class CommandLine : Form
    {
        IQM_TranslationTable form;

        private TransTableMotor selectedMotor;

        public CommandLine(IQM_TranslationTable form)
        {
            InitializeComponent();

            portNamesComboBox.Items.AddRange(SerialPort.GetPortNames());

            this.form = form;

            this.AcceptButton = sendButton;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (portNamesComboBox.SelectedItem == null)
            {
                responseRichTextBox.AppendText("Please select a port.\n");
            }
            else
            {
                form.CSM.motor1.SelectedPort = portNamesComboBox.SelectedItem.ToString();
                form.CSM.motor2.SelectedPort = portNamesComboBox.SelectedItem.ToString();

                responseRichTextBox.AppendText(String.Format("Port {0} selected.\n", portNamesComboBox.SelectedItem));

                commandTextBox.Enabled = true;
                sendButton.Enabled = true;
                methodCallButton.Enabled = true;
                eraseButton.Enabled = true;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            form.CSM.motor1.ClosePort();

            commandTextBox.Enabled = false;
            sendButton.Enabled = false;
            methodCallButton.Enabled = false;
            eraseButton.Enabled = false;
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (selectedMotor == null)
            {
                responseRichTextBox.AppendText("Please select which motor to send the command to.\n");
            }
            else
            {
                responseRichTextBox.AppendText("\n" + commandTextBox.Text + "\n" + 
                    selectedMotor.MotorCommand(commandTextBox.Text) + "\n");
                commandTextBox.Text = "";

                if (selectedMotor.ErrorFlag == true)
                {
                    responseRichTextBox.AppendText(selectedMotor.ErrorMessage + "\n");
                    selectedMotor.ResetErrorFlag();
                }
            }
        }

        private void eraseButton_Click(object sender, EventArgs e)
        {
            responseRichTextBox.Text = "";
        }

        private void methodCallButton_Click(object sender, EventArgs e)
        {
            if (selectedMotor == null)
            {
                responseRichTextBox.AppendText("Please select which motor to send the command to.\n");
            }
            else
            {
                responseRichTextBox.AppendText("\n" + commandTextBox.Text + "\n");

                int firstIndex = commandTextBox.Text.IndexOf("(");
                int lastIndex = commandTextBox.Text.LastIndexOf(")");

                if (firstIndex == -1 || lastIndex == -1 || firstIndex > lastIndex)
                {
                    responseRichTextBox.AppendText("Invalid method call.\n");
                }
                else
                {
                    string methodName = commandTextBox.Text.Substring(0, firstIndex);
                    string[] parameterStr = commandTextBox.Text.Substring(firstIndex + 1, lastIndex - firstIndex - 1).Split(',');
                    object[] parameter;

                    if (parameterStr[0] == "")
                    {
                        parameter = null;
                    }
                    else
                    {
                        parameter = new object[parameterStr.Length];
                        int value;
                        for (int i = 0; i < parameterStr.Length; i++)
                        {
                            int.TryParse(parameterStr[i], out value);
                            parameter[i] = value;
                        }
                    }

                    Type type = typeof(Commands.ComMotorCommands);
                    MethodInfo method = type.GetMethod(methodName);

                    try
                    {
                        responseRichTextBox.AppendText(method.Invoke(selectedMotor, parameter).ToString() + "\n");
                    }
                    catch (TargetParameterCountException)
                    {
                        responseRichTextBox.AppendText("Parameter count mismatch.\n");
                    }
                }

                commandTextBox.Text = "";
            }
        }

        private void motor1RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (motor1RadioButton.Checked)
            {
                selectedMotor = form.CSM.motor1;
            }
            else if(motor2RadioButton.Checked)
            {
                selectedMotor = form.CSM.motor2;
            }
        }
    }
}
