namespace IQM_TranslationTable
{
    partial class CommandLine
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.responseRichTextBox = new System.Windows.Forms.RichTextBox();
            this.openButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.portNamesComboBox = new System.Windows.Forms.ComboBox();
            this.commandTextBox = new System.Windows.Forms.TextBox();
            this.eraseButton = new System.Windows.Forms.Button();
            this.sendButton = new System.Windows.Forms.Button();
            this.methodCallButton = new System.Windows.Forms.Button();
            this.motor1RadioButton = new System.Windows.Forms.RadioButton();
            this.motor2RadioButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // responseRichTextBox
            // 
            this.responseRichTextBox.Location = new System.Drawing.Point(12, 40);
            this.responseRichTextBox.Name = "responseRichTextBox";
            this.responseRichTextBox.ReadOnly = true;
            this.responseRichTextBox.Size = new System.Drawing.Size(274, 213);
            this.responseRichTextBox.TabIndex = 0;
            this.responseRichTextBox.Text = "";
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(12, 11);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 1;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(93, 11);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // portNamesComboBox
            // 
            this.portNamesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portNamesComboBox.FormattingEnabled = true;
            this.portNamesComboBox.Location = new System.Drawing.Point(174, 12);
            this.portNamesComboBox.Name = "portNamesComboBox";
            this.portNamesComboBox.Size = new System.Drawing.Size(111, 21);
            this.portNamesComboBox.TabIndex = 3;
            // 
            // commandTextBox
            // 
            this.commandTextBox.Enabled = false;
            this.commandTextBox.Location = new System.Drawing.Point(11, 282);
            this.commandTextBox.Name = "commandTextBox";
            this.commandTextBox.Size = new System.Drawing.Size(274, 20);
            this.commandTextBox.TabIndex = 4;
            // 
            // eraseButton
            // 
            this.eraseButton.Enabled = false;
            this.eraseButton.Location = new System.Drawing.Point(129, 309);
            this.eraseButton.Name = "eraseButton";
            this.eraseButton.Size = new System.Drawing.Size(75, 23);
            this.eraseButton.TabIndex = 5;
            this.eraseButton.Text = "Erase";
            this.eraseButton.UseVisualStyleBackColor = true;
            this.eraseButton.Click += new System.EventHandler(this.eraseButton_Click);
            // 
            // sendButton
            // 
            this.sendButton.Enabled = false;
            this.sendButton.Location = new System.Drawing.Point(210, 309);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 6;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // methodCallButton
            // 
            this.methodCallButton.Enabled = false;
            this.methodCallButton.Location = new System.Drawing.Point(11, 309);
            this.methodCallButton.Name = "methodCallButton";
            this.methodCallButton.Size = new System.Drawing.Size(112, 23);
            this.methodCallButton.TabIndex = 7;
            this.methodCallButton.Text = "Method Call";
            this.methodCallButton.UseVisualStyleBackColor = true;
            this.methodCallButton.Click += new System.EventHandler(this.methodCallButton_Click);
            // 
            // motor1RadioButton
            // 
            this.motor1RadioButton.AutoSize = true;
            this.motor1RadioButton.Location = new System.Drawing.Point(163, 259);
            this.motor1RadioButton.Name = "motor1RadioButton";
            this.motor1RadioButton.Size = new System.Drawing.Size(58, 17);
            this.motor1RadioButton.TabIndex = 8;
            this.motor1RadioButton.TabStop = true;
            this.motor1RadioButton.Text = "Motor1";
            this.motor1RadioButton.UseVisualStyleBackColor = true;
            this.motor1RadioButton.CheckedChanged += new System.EventHandler(this.motor1RadioButton_CheckedChanged);
            // 
            // motor2RadioButton
            // 
            this.motor2RadioButton.AutoSize = true;
            this.motor2RadioButton.Location = new System.Drawing.Point(227, 259);
            this.motor2RadioButton.Name = "motor2RadioButton";
            this.motor2RadioButton.Size = new System.Drawing.Size(58, 17);
            this.motor2RadioButton.TabIndex = 9;
            this.motor2RadioButton.TabStop = true;
            this.motor2RadioButton.Text = "Motor2";
            this.motor2RadioButton.UseVisualStyleBackColor = true;
            // 
            // CommandLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 344);
            this.Controls.Add(this.motor2RadioButton);
            this.Controls.Add(this.motor1RadioButton);
            this.Controls.Add(this.methodCallButton);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.eraseButton);
            this.Controls.Add(this.commandTextBox);
            this.Controls.Add(this.portNamesComboBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.responseRichTextBox);
            this.Name = "CommandLine";
            this.Text = "CommandLine";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox responseRichTextBox;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ComboBox portNamesComboBox;
        private System.Windows.Forms.TextBox commandTextBox;
        private System.Windows.Forms.Button eraseButton;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.Button methodCallButton;
        private System.Windows.Forms.RadioButton motor1RadioButton;
        private System.Windows.Forms.RadioButton motor2RadioButton;
    }
}