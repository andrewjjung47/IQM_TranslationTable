﻿using System.Windows.Forms;

namespace IQM_TranslationTable
{
    partial class IQM_TranslationTable
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.InitializeButton = new System.Windows.Forms.Button();
            this.motorSettings = new System.Windows.Forms.DataGridView();
            this.MotorSettingMotor1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MotorSettingMotor2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordSettings = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.recordNumTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupbox1 = new System.Windows.Forms.GroupBox();
            this.moveEventButton = new System.Windows.Forms.Button();
            this.baudrateDropDown = new System.Windows.Forms.ComboBox();
            this.comDropDown = new System.Windows.Forms.ComboBox();
            this.logDirectoryButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.logFolderTextBox = new System.Windows.Forms.TextBox();
            this.Status2TextBox = new System.Windows.Forms.TextBox();
            this.Status1TextBox = new System.Windows.Forms.TextBox();
            this.logFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.motor2RefPositionButton = new System.Windows.Forms.Button();
            this.motor2HomeButton = new System.Windows.Forms.Button();
            this.motor2RightButton = new System.Windows.Forms.Button();
            this.motor2SpeedTextBox = new System.Windows.Forms.TextBox();
            this.motor2StepsTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.motor2LeftButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.motor1RefPositionButton = new System.Windows.Forms.Button();
            this.motor1HomeButton = new System.Windows.Forms.Button();
            this.motor1RightButton = new System.Windows.Forms.Button();
            this.motor1SpeedTextBox = new System.Windows.Forms.TextBox();
            this.motor1StepsTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.motor1LeftButton = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.motor2RelPosTextBox = new System.Windows.Forms.TextBox();
            this.motor2AbsPosTextBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.motor1RelPosTextBox = new System.Windows.Forms.TextBox();
            this.motor1AbsPosTextBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label22 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.loadRecordButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pauseButton = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.motor2StatusLabel = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.motor1StatusLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.motorSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordSettings)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupbox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "COM Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baudrate:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // InitializeButton
            // 
            this.InitializeButton.Location = new System.Drawing.Point(6, 12);
            this.InitializeButton.Name = "InitializeButton";
            this.InitializeButton.Size = new System.Drawing.Size(75, 59);
            this.InitializeButton.TabIndex = 5;
            this.InitializeButton.Text = "Initialize\r\nMotor";
            this.InitializeButton.UseVisualStyleBackColor = true;
            this.InitializeButton.Click += new System.EventHandler(this.InitializeButton_Click);
            // 
            // motorSettings
            // 
            this.motorSettings.AllowUserToAddRows = false;
            this.motorSettings.AllowUserToDeleteRows = false;
            this.motorSettings.AllowUserToResizeColumns = false;
            this.motorSettings.AllowUserToResizeRows = false;
            this.motorSettings.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.motorSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.motorSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MotorSettingMotor1,
            this.MotorSettingMotor2});
            this.motorSettings.Location = new System.Drawing.Point(-2, 23);
            this.motorSettings.Name = "motorSettings";
            this.motorSettings.RowHeadersWidth = 130;
            this.motorSettings.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.motorSettings.Size = new System.Drawing.Size(264, 91);
            this.motorSettings.TabIndex = 6;
            // 
            // MotorSettingMotor1
            // 
            this.MotorSettingMotor1.HeaderText = "Motor1";
            this.MotorSettingMotor1.Name = "MotorSettingMotor1";
            this.MotorSettingMotor1.Width = 66;
            // 
            // MotorSettingMotor2
            // 
            this.MotorSettingMotor2.HeaderText = "Motor2";
            this.MotorSettingMotor2.Name = "MotorSettingMotor2";
            this.MotorSettingMotor2.Width = 66;
            // 
            // recordSettings
            // 
            this.recordSettings.AllowUserToAddRows = false;
            this.recordSettings.AllowUserToDeleteRows = false;
            this.recordSettings.AllowUserToResizeColumns = false;
            this.recordSettings.AllowUserToResizeRows = false;
            this.recordSettings.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.recordSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.recordSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.recordSettings.Location = new System.Drawing.Point(-2, 159);
            this.recordSettings.Name = "recordSettings";
            this.recordSettings.RowHeadersWidth = 130;
            this.recordSettings.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.recordSettings.Size = new System.Drawing.Size(264, 201);
            this.recordSettings.TabIndex = 8;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Motor1";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 66;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Motor2";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 66;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(160, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Motor1 Status:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(160, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Motor2 Status:";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(168, 12);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(121, 59);
            this.StartButton.TabIndex = 13;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.recordNumTextBox);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.motorSettings);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.recordSettings);
            this.panel1.Location = new System.Drawing.Point(22, 106);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 364);
            this.panel1.TabIndex = 15;
            // 
            // recordNumTextBox
            // 
            this.recordNumTextBox.Location = new System.Drawing.Point(195, 132);
            this.recordNumTextBox.Name = "recordNumTextBox";
            this.recordNumTextBox.Size = new System.Drawing.Size(66, 20);
            this.recordNumTextBox.TabIndex = 10;
            this.recordNumTextBox.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 16);
            this.label5.TabIndex = 7;
            this.label5.Text = "Motor Settings";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(110, 134);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(85, 13);
            this.label23.TabIndex = 9;
            this.label23.Text = "Record Number:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(4, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 16);
            this.label8.TabIndex = 8;
            this.label8.Text = "Record Settings";
            // 
            // groupbox1
            // 
            this.groupbox1.AutoSize = true;
            this.groupbox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupbox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupbox1.Controls.Add(this.moveEventButton);
            this.groupbox1.Controls.Add(this.baudrateDropDown);
            this.groupbox1.Controls.Add(this.comDropDown);
            this.groupbox1.Controls.Add(this.logDirectoryButton);
            this.groupbox1.Controls.Add(this.label7);
            this.groupbox1.Controls.Add(this.logFolderTextBox);
            this.groupbox1.Controls.Add(this.Status2TextBox);
            this.groupbox1.Controls.Add(this.Status1TextBox);
            this.groupbox1.Controls.Add(this.label1);
            this.groupbox1.Controls.Add(this.label2);
            this.groupbox1.Controls.Add(this.label3);
            this.groupbox1.Controls.Add(this.label4);
            this.groupbox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupbox1.Location = new System.Drawing.Point(22, 12);
            this.groupbox1.Name = "groupbox1";
            this.groupbox1.Size = new System.Drawing.Size(811, 84);
            this.groupbox1.TabIndex = 16;
            this.groupbox1.TabStop = false;
            // 
            // moveEventButton
            // 
            this.moveEventButton.Location = new System.Drawing.Point(730, 42);
            this.moveEventButton.Name = "moveEventButton";
            this.moveEventButton.Size = new System.Drawing.Size(75, 23);
            this.moveEventButton.TabIndex = 18;
            this.moveEventButton.Text = "MoveEvent";
            this.moveEventButton.UseVisualStyleBackColor = true;
            this.moveEventButton.Click += new System.EventHandler(this.moveEventButton_Click);
            // 
            // baudrateDropDown
            // 
            this.baudrateDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.baudrateDropDown.FormattingEnabled = true;
            this.baudrateDropDown.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.baudrateDropDown.Location = new System.Drawing.Point(70, 40);
            this.baudrateDropDown.Name = "baudrateDropDown";
            this.baudrateDropDown.Size = new System.Drawing.Size(60, 21);
            this.baudrateDropDown.TabIndex = 17;
            // 
            // comDropDown
            // 
            this.comDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comDropDown.FormattingEnabled = true;
            this.comDropDown.Location = new System.Drawing.Point(70, 15);
            this.comDropDown.Name = "comDropDown";
            this.comDropDown.Size = new System.Drawing.Size(60, 21);
            this.comDropDown.TabIndex = 16;
            // 
            // logDirectoryButton
            // 
            this.logDirectoryButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logDirectoryButton.Location = new System.Drawing.Point(778, 14);
            this.logDirectoryButton.Name = "logDirectoryButton";
            this.logDirectoryButton.Size = new System.Drawing.Size(27, 22);
            this.logDirectoryButton.TabIndex = 15;
            this.logDirectoryButton.Text = "...";
            this.logDirectoryButton.UseVisualStyleBackColor = true;
            this.logDirectoryButton.Click += new System.EventHandler(this.logDirectoryButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(370, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 15);
            this.label7.TabIndex = 14;
            this.label7.Text = "Log Folder:";
            // 
            // logFolderTextBox
            // 
            this.logFolderTextBox.Location = new System.Drawing.Point(440, 15);
            this.logFolderTextBox.Name = "logFolderTextBox";
            this.logFolderTextBox.Size = new System.Drawing.Size(333, 20);
            this.logFolderTextBox.TabIndex = 13;
            // 
            // Status2TextBox
            // 
            this.Status2TextBox.Location = new System.Drawing.Point(247, 40);
            this.Status2TextBox.Name = "Status2TextBox";
            this.Status2TextBox.ReadOnly = true;
            this.Status2TextBox.Size = new System.Drawing.Size(91, 20);
            this.Status2TextBox.TabIndex = 12;
            // 
            // Status1TextBox
            // 
            this.Status1TextBox.Location = new System.Drawing.Point(247, 15);
            this.Status1TextBox.Name = "Status1TextBox";
            this.Status1TextBox.ReadOnly = true;
            this.Status1TextBox.Size = new System.Drawing.Size(91, 20);
            this.Status1TextBox.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.groupBox5);
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Location = new System.Drawing.Point(309, 106);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(266, 364);
            this.panel2.TabIndex = 17;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.motor2RefPositionButton);
            this.groupBox3.Controls.Add(this.motor2HomeButton);
            this.groupBox3.Controls.Add(this.motor2RightButton);
            this.groupBox3.Controls.Add(this.motor2SpeedTextBox);
            this.groupBox3.Controls.Add(this.motor2StepsTextBox);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.motor2LeftButton);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(0, 277);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(264, 85);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Motor2";
            // 
            // motor2RefPositionButton
            // 
            this.motor2RefPositionButton.Location = new System.Drawing.Point(68, 51);
            this.motor2RefPositionButton.Name = "motor2RefPositionButton";
            this.motor2RefPositionButton.Size = new System.Drawing.Size(75, 25);
            this.motor2RefPositionButton.TabIndex = 7;
            this.motor2RefPositionButton.Text = "Reference";
            this.motor2RefPositionButton.UseVisualStyleBackColor = true;
            this.motor2RefPositionButton.Click += new System.EventHandler(this.motor2RefPositionButton_Click);
            // 
            // motor2HomeButton
            // 
            this.motor2HomeButton.Location = new System.Drawing.Point(206, 51);
            this.motor2HomeButton.Name = "motor2HomeButton";
            this.motor2HomeButton.Size = new System.Drawing.Size(49, 25);
            this.motor2HomeButton.TabIndex = 6;
            this.motor2HomeButton.Text = "Home";
            this.motor2HomeButton.UseVisualStyleBackColor = true;
            this.motor2HomeButton.Click += new System.EventHandler(this.motor2HomeButton_Click);
            // 
            // motor2RightButton
            // 
            this.motor2RightButton.Location = new System.Drawing.Point(144, 51);
            this.motor2RightButton.Name = "motor2RightButton";
            this.motor2RightButton.Size = new System.Drawing.Size(61, 25);
            this.motor2RightButton.TabIndex = 5;
            this.motor2RightButton.Text = "Right >>";
            this.motor2RightButton.UseVisualStyleBackColor = true;
            this.motor2RightButton.Click += new System.EventHandler(this.motor2RightButton_Click);
            // 
            // motor2SpeedTextBox
            // 
            this.motor2SpeedTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motor2SpeedTextBox.Location = new System.Drawing.Point(194, 25);
            this.motor2SpeedTextBox.Name = "motor2SpeedTextBox";
            this.motor2SpeedTextBox.Size = new System.Drawing.Size(61, 20);
            this.motor2SpeedTextBox.TabIndex = 4;
            // 
            // motor2StepsTextBox
            // 
            this.motor2StepsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motor2StepsTextBox.Location = new System.Drawing.Point(43, 25);
            this.motor2StepsTextBox.Name = "motor2StepsTextBox";
            this.motor2StepsTextBox.Size = new System.Drawing.Size(61, 20);
            this.motor2StepsTextBox.TabIndex = 3;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(118, 27);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Speed (mm/s):";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(6, 27);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(37, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Steps:";
            // 
            // motor2LeftButton
            // 
            this.motor2LeftButton.BackColor = System.Drawing.SystemColors.Control;
            this.motor2LeftButton.Location = new System.Drawing.Point(6, 51);
            this.motor2LeftButton.Name = "motor2LeftButton";
            this.motor2LeftButton.Size = new System.Drawing.Size(61, 25);
            this.motor2LeftButton.TabIndex = 0;
            this.motor2LeftButton.Text = "<< Left";
            this.motor2LeftButton.UseVisualStyleBackColor = false;
            this.motor2LeftButton.Click += new System.EventHandler(this.motor2LeftButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.motor1RefPositionButton);
            this.groupBox2.Controls.Add(this.motor1HomeButton);
            this.groupBox2.Controls.Add(this.motor1RightButton);
            this.groupBox2.Controls.Add(this.motor1SpeedTextBox);
            this.groupBox2.Controls.Add(this.motor1StepsTextBox);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.motor1LeftButton);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 188);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(264, 85);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Motor1";
            // 
            // motor1RefPositionButton
            // 
            this.motor1RefPositionButton.Location = new System.Drawing.Point(68, 51);
            this.motor1RefPositionButton.Name = "motor1RefPositionButton";
            this.motor1RefPositionButton.Size = new System.Drawing.Size(75, 25);
            this.motor1RefPositionButton.TabIndex = 7;
            this.motor1RefPositionButton.Text = "Reference";
            this.motor1RefPositionButton.UseVisualStyleBackColor = true;
            this.motor1RefPositionButton.Click += new System.EventHandler(this.motor1RefPositionButton_Click);
            // 
            // motor1HomeButton
            // 
            this.motor1HomeButton.Location = new System.Drawing.Point(206, 51);
            this.motor1HomeButton.Name = "motor1HomeButton";
            this.motor1HomeButton.Size = new System.Drawing.Size(49, 25);
            this.motor1HomeButton.TabIndex = 6;
            this.motor1HomeButton.Text = "Home";
            this.motor1HomeButton.UseVisualStyleBackColor = true;
            this.motor1HomeButton.Click += new System.EventHandler(this.motor1HomeButton_Click);
            // 
            // motor1RightButton
            // 
            this.motor1RightButton.Location = new System.Drawing.Point(144, 51);
            this.motor1RightButton.Name = "motor1RightButton";
            this.motor1RightButton.Size = new System.Drawing.Size(61, 25);
            this.motor1RightButton.TabIndex = 5;
            this.motor1RightButton.Text = "Right >>";
            this.motor1RightButton.UseVisualStyleBackColor = true;
            this.motor1RightButton.Click += new System.EventHandler(this.motor1RightButton_Click);
            // 
            // motor1SpeedTextBox
            // 
            this.motor1SpeedTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motor1SpeedTextBox.Location = new System.Drawing.Point(194, 25);
            this.motor1SpeedTextBox.Name = "motor1SpeedTextBox";
            this.motor1SpeedTextBox.Size = new System.Drawing.Size(61, 20);
            this.motor1SpeedTextBox.TabIndex = 4;
            // 
            // motor1StepsTextBox
            // 
            this.motor1StepsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motor1StepsTextBox.Location = new System.Drawing.Point(43, 25);
            this.motor1StepsTextBox.Name = "motor1StepsTextBox";
            this.motor1StepsTextBox.Size = new System.Drawing.Size(61, 20);
            this.motor1StepsTextBox.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(118, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Speed (mm/s):";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Steps:";
            // 
            // motor1LeftButton
            // 
            this.motor1LeftButton.BackColor = System.Drawing.SystemColors.Control;
            this.motor1LeftButton.Location = new System.Drawing.Point(6, 51);
            this.motor1LeftButton.Name = "motor1LeftButton";
            this.motor1LeftButton.Size = new System.Drawing.Size(61, 25);
            this.motor1LeftButton.TabIndex = 0;
            this.motor1LeftButton.Text = "<< Left";
            this.motor1LeftButton.UseVisualStyleBackColor = false;
            this.motor1LeftButton.Click += new System.EventHandler(this.motor1LeftButton_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(4, 169);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 16);
            this.label12.TabIndex = 7;
            this.label12.Text = "Manual Controls";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.motor2RelPosTextBox);
            this.groupBox5.Controls.Add(this.motor2AbsPosTextBox);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Controls.Add(this.label18);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(0, 84);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(264, 75);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Motor2";
            // 
            // motor2RelPosTextBox
            // 
            this.motor2RelPosTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motor2RelPosTextBox.Location = new System.Drawing.Point(103, 45);
            this.motor2RelPosTextBox.Name = "motor2RelPosTextBox";
            this.motor2RelPosTextBox.ReadOnly = true;
            this.motor2RelPosTextBox.Size = new System.Drawing.Size(91, 20);
            this.motor2RelPosTextBox.TabIndex = 14;
            this.motor2RelPosTextBox.Text = "homing required";
            // 
            // motor2AbsPosTextBox
            // 
            this.motor2AbsPosTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motor2AbsPosTextBox.Location = new System.Drawing.Point(103, 20);
            this.motor2AbsPosTextBox.Name = "motor2AbsPosTextBox";
            this.motor2AbsPosTextBox.ReadOnly = true;
            this.motor2AbsPosTextBox.Size = new System.Drawing.Size(91, 20);
            this.motor2AbsPosTextBox.TabIndex = 13;
            this.motor2AbsPosTextBox.Text = "homing required";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(3, 45);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(86, 16);
            this.label17.TabIndex = 10;
            this.label17.Text = "Rel. Position:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(3, 20);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(89, 16);
            this.label18.TabIndex = 9;
            this.label18.Text = "Abs. Position:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.motor1RelPosTextBox);
            this.groupBox4.Controls.Add(this.motor1AbsPosTextBox);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(0, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(264, 75);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Motor1";
            // 
            // motor1RelPosTextBox
            // 
            this.motor1RelPosTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motor1RelPosTextBox.Location = new System.Drawing.Point(103, 45);
            this.motor1RelPosTextBox.Name = "motor1RelPosTextBox";
            this.motor1RelPosTextBox.ReadOnly = true;
            this.motor1RelPosTextBox.Size = new System.Drawing.Size(91, 20);
            this.motor1RelPosTextBox.TabIndex = 13;
            this.motor1RelPosTextBox.Text = "homing required";
            // 
            // motor1AbsPosTextBox
            // 
            this.motor1AbsPosTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motor1AbsPosTextBox.Location = new System.Drawing.Point(103, 20);
            this.motor1AbsPosTextBox.Name = "motor1AbsPosTextBox";
            this.motor1AbsPosTextBox.ReadOnly = true;
            this.motor1AbsPosTextBox.Size = new System.Drawing.Size(91, 20);
            this.motor1AbsPosTextBox.TabIndex = 12;
            this.motor1AbsPosTextBox.Text = "homing required";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(3, 45);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(86, 16);
            this.label16.TabIndex = 10;
            this.label16.Text = "Rel. Position:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 16);
            this.label9.TabIndex = 9;
            this.label9.Text = "Abs. Position:";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.label22);
            this.panel4.Controls.Add(this.dataGridView1);
            this.panel4.Location = new System.Drawing.Point(596, 106);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(266, 364);
            this.panel4.TabIndex = 19;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(4, 4);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(118, 16);
            this.label22.TabIndex = 8;
            this.label22.Text = "CSM Positions List";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dataGridView1.Location = new System.Drawing.Point(-2, 23);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 130;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.Size = new System.Drawing.Size(264, 337);
            this.dataGridView1.TabIndex = 8;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Motor1";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 66;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Motor2";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 66;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.loadRecordButton);
            this.groupBox6.Controls.Add(this.InitializeButton);
            this.groupBox6.Controls.Add(this.StartButton);
            this.groupBox6.Location = new System.Drawing.Point(843, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(296, 79);
            this.groupBox6.TabIndex = 20;
            this.groupBox6.TabStop = false;
            // 
            // loadRecordButton
            // 
            this.loadRecordButton.Location = new System.Drawing.Point(87, 12);
            this.loadRecordButton.Name = "loadRecordButton";
            this.loadRecordButton.Size = new System.Drawing.Size(75, 59);
            this.loadRecordButton.TabIndex = 14;
            this.loadRecordButton.Text = "Load Record Settings";
            this.loadRecordButton.UseVisualStyleBackColor = true;
            this.loadRecordButton.Click += new System.EventHandler(this.loadRecordButton_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.pauseButton);
            this.panel3.Controls.Add(this.label21);
            this.panel3.Controls.Add(this.progressBar1);
            this.panel3.Controls.Add(this.motor2StatusLabel);
            this.panel3.Controls.Add(this.label20);
            this.panel3.Controls.Add(this.motor1StatusLabel);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Location = new System.Drawing.Point(882, 106);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(266, 364);
            this.panel3.TabIndex = 21;
            // 
            // pauseButton
            // 
            this.pauseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pauseButton.Location = new System.Drawing.Point(6, 239);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(249, 112);
            this.pauseButton.TabIndex = 14;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(3, 4);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(98, 16);
            this.label21.TabIndex = 13;
            this.label21.Text = "CSM Progress:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 24);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(249, 32);
            this.progressBar1.TabIndex = 12;
            // 
            // motor2StatusLabel
            // 
            this.motor2StatusLabel.BackColor = System.Drawing.SystemColors.GrayText;
            this.motor2StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motor2StatusLabel.Location = new System.Drawing.Point(-1, 175);
            this.motor2StatusLabel.Name = "motor2StatusLabel";
            this.motor2StatusLabel.Size = new System.Drawing.Size(266, 47);
            this.motor2StatusLabel.TabIndex = 11;
            this.motor2StatusLabel.Text = "Click Start to Begin";
            this.motor2StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(3, 152);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(92, 16);
            this.label20.TabIndex = 10;
            this.label20.Text = "Motor2 Status:";
            // 
            // motor1StatusLabel
            // 
            this.motor1StatusLabel.BackColor = System.Drawing.SystemColors.GrayText;
            this.motor1StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.motor1StatusLabel.Location = new System.Drawing.Point(-1, 95);
            this.motor1StatusLabel.Name = "motor1StatusLabel";
            this.motor1StatusLabel.Size = new System.Drawing.Size(266, 47);
            this.motor1StatusLabel.TabIndex = 9;
            this.motor1StatusLabel.Text = "Click Start to Begin";
            this.motor1StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "Motor1 Status:";
            // 
            // IQM_TranslationTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 496);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupbox1);
            this.Controls.Add(this.panel1);
            this.Name = "IQM_TranslationTable";
            this.Text = "IQM Translation Table App";
            ((System.ComponentModel.ISupportInitialize)(this.motorSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recordSettings)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupbox1.ResumeLayout(false);
            this.groupbox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button InitializeButton;
        private System.Windows.Forms.DataGridView motorSettings;
        private System.Windows.Forms.DataGridView recordSettings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupbox1;
        private System.Windows.Forms.TextBox Status2TextBox;
        private System.Windows.Forms.TextBox Status1TextBox;
        private System.Windows.Forms.Label label5;
        private Button logDirectoryButton;
        private Label label7;
        private TextBox logFolderTextBox;
        private FolderBrowserDialog logFolderDialog;
        private Panel panel2;
        private Label label8;
        private Label label9;
        private DataGridViewTextBoxColumn MotorSettingMotor1;
        private DataGridViewTextBoxColumn MotorSettingMotor2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private Label label12;
        private GroupBox groupBox2;
        private TextBox motor1SpeedTextBox;
        private TextBox motor1StepsTextBox;
        private Label label13;
        private Label label11;
        private Button motor1LeftButton;
        private Button motor1HomeButton;
        private Button motor1RightButton;
        private GroupBox groupBox5;
        private Label label17;
        private Label label18;
        private GroupBox groupBox4;
        private Label label16;
        private Panel panel4;
        private Label label22;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private GroupBox groupBox6;
        private Button loadRecordButton;
        private Panel panel3;
        private Label label6;
        private Label motor2StatusLabel;
        private Label label20;
        private Label motor1StatusLabel;
        private Button pauseButton;
        private Label label21;
        private ProgressBar progressBar1;
        private TextBox recordNumTextBox;
        private Label label23;
        private ComboBox comDropDown;
        private ComboBox baudrateDropDown;
        private TextBox motor2RelPosTextBox;
        private TextBox motor2AbsPosTextBox;
        private TextBox motor1RelPosTextBox;
        private TextBox motor1AbsPosTextBox;
        private GroupBox groupBox3;
        private Button motor2RefPositionButton;
        private Button motor2HomeButton;
        private Button motor2RightButton;
        private TextBox motor2SpeedTextBox;
        private TextBox motor2StepsTextBox;
        private Label label14;
        private Label label15;
        private Button motor2LeftButton;
        private Button motor1RefPositionButton;
        private Button moveEventButton;
    }
}

