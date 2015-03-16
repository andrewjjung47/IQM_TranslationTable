namespace IQM_TranslationTable
{
    partial class PositionInput
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
            this.displayRichTextBox = new System.Windows.Forms.RichTextBox();
            this.inputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.inputButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // displayRichTextBox
            // 
            this.displayRichTextBox.Location = new System.Drawing.Point(13, 13);
            this.displayRichTextBox.Name = "displayRichTextBox";
            this.displayRichTextBox.ReadOnly = true;
            this.displayRichTextBox.Size = new System.Drawing.Size(501, 102);
            this.displayRichTextBox.TabIndex = 0;
            this.displayRichTextBox.Text = "Position input format: \na list of position pairs for motor 1 and motor 2, (motor1" +
    "Pos, motor2Pos), separated by a comma.\n\nex. (0, 0), (1600, 1600), (3200, 1600) \n" +
    "\n";
            // 
            // inputRichTextBox
            // 
            this.inputRichTextBox.Location = new System.Drawing.Point(13, 122);
            this.inputRichTextBox.Name = "inputRichTextBox";
            this.inputRichTextBox.Size = new System.Drawing.Size(501, 97);
            this.inputRichTextBox.TabIndex = 1;
            this.inputRichTextBox.Text = "";
            // 
            // inputButton
            // 
            this.inputButton.Location = new System.Drawing.Point(437, 226);
            this.inputButton.Name = "inputButton";
            this.inputButton.Size = new System.Drawing.Size(75, 23);
            this.inputButton.TabIndex = 2;
            this.inputButton.Text = "Read Input";
            this.inputButton.UseVisualStyleBackColor = true;
            this.inputButton.Click += new System.EventHandler(this.inputButton_Click);
            // 
            // PositionInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 369);
            this.Controls.Add(this.inputButton);
            this.Controls.Add(this.inputRichTextBox);
            this.Controls.Add(this.displayRichTextBox);
            this.Name = "PositionInput";
            this.Text = "PositionInput";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox displayRichTextBox;
        private System.Windows.Forms.RichTextBox inputRichTextBox;
        private System.Windows.Forms.Button inputButton;
    }
}