namespace GnssLibGUI
{
    partial class GUI_Window

    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Terminal = new TextBox();
            Simulate = new Button();
            groupBox1 = new GroupBox();
            setSecond = new NumericUpDown();
            setMinute = new NumericUpDown();
            setHour = new NumericUpDown();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            groupBox2 = new GroupBox();
            updatePos = new Button();
            setLong = new TextBox();
            setLat = new TextBox();
            label5 = new Label();
            label6 = new Label();
            groupBox3 = new GroupBox();
            setGlonass = new CheckBox();
            setGalileo = new CheckBox();
            setGps = new CheckBox();
            setFile = new ComboBox();
            groupBox4 = new GroupBox();
            groupBox5 = new GroupBox();
            updateJammerPos = new Button();
            groupBox7 = new GroupBox();
            labelRadR = new Label();
            setRadR = new TrackBar();
            groupBox6 = new GroupBox();
            setJammerLong = new TextBox();
            setJammerLat = new TextBox();
            label1 = new Label();
            label7 = new Label();
            setIntOff = new RadioButton();
            setIntOn = new RadioButton();
            setNMEA = new CheckBox();
            Stop = new Button();
            labelPosAcc = new Label();
            label10 = new Label();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)setSecond).BeginInit();
            ((System.ComponentModel.ISupportInitialize)setMinute).BeginInit();
            ((System.ComponentModel.ISupportInitialize)setHour).BeginInit();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)setRadR).BeginInit();
            groupBox6.SuspendLayout();
            SuspendLayout();
            // 
            // Terminal
            // 
            Terminal.BackColor = Color.Black;
            Terminal.ForeColor = Color.White;
            Terminal.Location = new Point(41, 235);
            Terminal.Margin = new Padding(3, 2, 3, 2);
            Terminal.Multiline = true;
            Terminal.Name = "Terminal";
            Terminal.ReadOnly = true;
            Terminal.ScrollBars = ScrollBars.Both;
            Terminal.Size = new Size(692, 159);
            Terminal.TabIndex = 0;
            Terminal.TabStop = false;
            // 
            // Simulate
            // 
            Simulate.BackColor = Color.DimGray;
            Simulate.Cursor = Cursors.Hand;
            Simulate.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            Simulate.ForeColor = Color.FromArgb(56, 203, 52);
            Simulate.Location = new Point(757, 314);
            Simulate.Margin = new Padding(3, 2, 3, 2);
            Simulate.Name = "Simulate";
            Simulate.Size = new Size(102, 31);
            Simulate.TabIndex = 2;
            Simulate.Text = "Simulate";
            Simulate.UseVisualStyleBackColor = false;
            Simulate.Click += Simulate_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(setSecond);
            groupBox1.Controls.Add(setMinute);
            groupBox1.Controls.Add(setHour);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox1.ForeColor = Color.FromArgb(255, 184, 56);
            groupBox1.Location = new Point(41, 63);
            groupBox1.Margin = new Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 2, 3, 2);
            groupBox1.Size = new Size(370, 51);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Time";
            // 
            // setSecond
            // 
            setSecond.Cursor = Cursors.Hand;
            setSecond.Location = new Point(322, 16);
            setSecond.Margin = new Padding(2);
            setSecond.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            setSecond.Name = "setSecond";
            setSecond.Size = new Size(43, 27);
            setSecond.TabIndex = 10;
            // 
            // setMinute
            // 
            setMinute.Cursor = Cursors.Hand;
            setMinute.Location = new Point(191, 16);
            setMinute.Margin = new Padding(2);
            setMinute.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            setMinute.Name = "setMinute";
            setMinute.Size = new Size(43, 27);
            setMinute.TabIndex = 9;
            // 
            // setHour
            // 
            setHour.Cursor = Cursors.Hand;
            setHour.Location = new Point(66, 16);
            setHour.Margin = new Padding(2);
            setHour.Maximum = new decimal(new int[] { 23, 0, 0, 0 });
            setHour.Name = "setHour";
            setHour.Size = new Size(43, 27);
            setHour.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(245, 20);
            label4.Name = "label4";
            label4.Size = new Size(70, 20);
            label4.TabIndex = 6;
            label4.Text = "Seconds:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(125, 20);
            label3.Name = "label3";
            label3.Size = new Size(63, 20);
            label3.TabIndex = 4;
            label3.Text = "Minute:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 20);
            label2.Name = "label2";
            label2.Size = new Size(49, 20);
            label2.TabIndex = 2;
            label2.Text = "Hour:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(updatePos);
            groupBox2.Controls.Add(setLong);
            groupBox2.Controls.Add(setLat);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label6);
            groupBox2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox2.ForeColor = Color.FromArgb(255, 184, 56);
            groupBox2.Location = new Point(41, 165);
            groupBox2.Margin = new Padding(3, 2, 3, 2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 2, 3, 2);
            groupBox2.Size = new Size(370, 58);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Reciever Position";
            // 
            // updatePos
            // 
            updatePos.BackColor = Color.DimGray;
            updatePos.Cursor = Cursors.Hand;
            updatePos.Location = new Point(282, 25);
            updatePos.Margin = new Padding(3, 2, 3, 2);
            updatePos.Name = "updatePos";
            updatePos.Size = new Size(83, 26);
            updatePos.TabIndex = 10;
            updatePos.Text = "Update pos";
            updatePos.UseVisualStyleBackColor = false;
            updatePos.Click += updatePos_Click;
            // 
            // setLong
            // 
            setLong.Location = new Point(184, 23);
            setLong.Margin = new Padding(3, 2, 3, 2);
            setLong.Name = "setLong";
            setLong.Size = new Size(67, 27);
            setLong.TabIndex = 8;
            setLong.Text = "12,6943";
            // 
            // setLat
            // 
            setLat.BackColor = Color.White;
            setLat.Location = new Point(52, 23);
            setLat.Margin = new Padding(3, 2, 3, 2);
            setLat.Name = "setLat";
            setLat.Size = new Size(67, 27);
            setLat.TabIndex = 7;
            setLat.Text = "56,0467";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(130, 26);
            label5.Name = "label5";
            label5.Size = new Size(48, 20);
            label5.TabIndex = 6;
            label5.Text = "Long:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 26);
            label6.Name = "label6";
            label6.Size = new Size(35, 20);
            label6.TabIndex = 4;
            label6.Text = "Lat:";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(setGlonass);
            groupBox3.Controls.Add(setGalileo);
            groupBox3.Controls.Add(setGps);
            groupBox3.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox3.ForeColor = Color.FromArgb(255, 184, 56);
            groupBox3.Location = new Point(41, 16);
            groupBox3.Margin = new Padding(3, 2, 3, 2);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 2, 3, 2);
            groupBox3.Size = new Size(370, 45);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "GNSS Type";
            // 
            // setGlonass
            // 
            setGlonass.AutoSize = true;
            setGlonass.Cursor = Cursors.Hand;
            setGlonass.Location = new Point(265, 19);
            setGlonass.Margin = new Padding(3, 2, 3, 2);
            setGlonass.Name = "setGlonass";
            setGlonass.Size = new Size(83, 24);
            setGlonass.TabIndex = 3;
            setGlonass.Text = "Glonass";
            setGlonass.UseVisualStyleBackColor = true;
            // 
            // setGalileo
            // 
            setGalileo.AutoSize = true;
            setGalileo.Checked = true;
            setGalileo.CheckState = CheckState.Checked;
            setGalileo.Cursor = Cursors.Hand;
            setGalileo.Location = new Point(149, 19);
            setGalileo.Margin = new Padding(3, 2, 3, 2);
            setGalileo.Name = "setGalileo";
            setGalileo.Size = new Size(76, 24);
            setGalileo.TabIndex = 2;
            setGalileo.Text = "Galileo";
            setGalileo.UseVisualStyleBackColor = true;
            // 
            // setGps
            // 
            setGps.AutoSize = true;
            setGps.Checked = true;
            setGps.CheckState = CheckState.Checked;
            setGps.Cursor = Cursors.Hand;
            setGps.Location = new Point(52, 19);
            setGps.Margin = new Padding(3, 2, 3, 2);
            setGps.Name = "setGps";
            setGps.Size = new Size(56, 24);
            setGps.TabIndex = 1;
            setGps.Text = "GPS";
            setGps.UseVisualStyleBackColor = true;
            // 
            // setFile
            // 
            setFile.Cursor = Cursors.Hand;
            setFile.DropDownStyle = ComboBoxStyle.DropDownList;
            setFile.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            setFile.FormattingEnabled = true;
            setFile.Location = new Point(114, 16);
            setFile.Margin = new Padding(3, 2, 3, 2);
            setFile.Name = "setFile";
            setFile.Size = new Size(251, 27);
            setFile.TabIndex = 10;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(setFile);
            groupBox4.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox4.ForeColor = Color.FromArgb(255, 184, 56);
            groupBox4.Location = new Point(41, 116);
            groupBox4.Margin = new Padding(3, 2, 3, 2);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(3, 2, 3, 2);
            groupBox4.Size = new Size(370, 49);
            groupBox4.TabIndex = 11;
            groupBox4.TabStop = false;
            groupBox4.Text = "Choose File";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(updateJammerPos);
            groupBox5.Controls.Add(groupBox7);
            groupBox5.Controls.Add(groupBox6);
            groupBox5.Controls.Add(setIntOff);
            groupBox5.Controls.Add(setIntOn);
            groupBox5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox5.ForeColor = Color.FromArgb(255, 184, 56);
            groupBox5.Location = new Point(489, 15);
            groupBox5.Margin = new Padding(3, 2, 3, 2);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(3, 2, 3, 2);
            groupBox5.Size = new Size(370, 208);
            groupBox5.TabIndex = 12;
            groupBox5.TabStop = false;
            groupBox5.Text = "Interference";
            // 
            // updateJammerPos
            // 
            updateJammerPos.BackColor = Color.DimGray;
            updateJammerPos.Cursor = Cursors.Hand;
            updateJammerPos.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            updateJammerPos.Location = new Point(285, 159);
            updateJammerPos.Margin = new Padding(3, 2, 3, 2);
            updateJammerPos.Name = "updateJammerPos";
            updateJammerPos.Size = new Size(83, 26);
            updateJammerPos.TabIndex = 10;
            updateJammerPos.Text = "Update pos";
            updateJammerPos.UseVisualStyleBackColor = false;
            updateJammerPos.Click += updateJammerPos_Click;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(labelRadR);
            groupBox7.Controls.Add(setRadR);
            groupBox7.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox7.ForeColor = Color.FromArgb(255, 184, 56);
            groupBox7.Location = new Point(14, 66);
            groupBox7.Margin = new Padding(3, 2, 3, 2);
            groupBox7.Name = "groupBox7";
            groupBox7.Padding = new Padding(3, 2, 3, 2);
            groupBox7.Size = new Size(350, 62);
            groupBox7.TabIndex = 16;
            groupBox7.TabStop = false;
            groupBox7.Text = "Jammer Radius";
            // 
            // labelRadR
            // 
            labelRadR.AutoSize = true;
            labelRadR.Location = new Point(36, 27);
            labelRadR.Name = "labelRadR";
            labelRadR.Size = new Size(47, 21);
            labelRadR.TabIndex = 17;
            labelRadR.Text = "0 km";
            // 
            // setRadR
            // 
            setRadR.AutoSize = false;
            setRadR.Cursor = Cursors.Hand;
            setRadR.Location = new Point(89, 27);
            setRadR.Margin = new Padding(3, 2, 3, 2);
            setRadR.Maximum = 500;
            setRadR.Name = "setRadR";
            setRadR.Size = new Size(255, 27);
            setRadR.TabIndex = 15;
            setRadR.TickFrequency = 10;
            setRadR.Scroll += setRadR_Scroll;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(setJammerLong);
            groupBox6.Controls.Add(setJammerLat);
            groupBox6.Controls.Add(label1);
            groupBox6.Controls.Add(label7);
            groupBox6.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox6.ForeColor = Color.FromArgb(255, 184, 56);
            groupBox6.Location = new Point(14, 137);
            groupBox6.Margin = new Padding(3, 2, 3, 2);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new Padding(3, 2, 3, 2);
            groupBox6.Size = new Size(266, 58);
            groupBox6.TabIndex = 5;
            groupBox6.TabStop = false;
            groupBox6.Text = "Jammer Position";
            // 
            // setJammerLong
            // 
            setJammerLong.Location = new Point(184, 21);
            setJammerLong.Margin = new Padding(3, 2, 3, 2);
            setJammerLong.Name = "setJammerLong";
            setJammerLong.Size = new Size(67, 27);
            setJammerLong.TabIndex = 8;
            setJammerLong.Text = "13,4049";
            // 
            // setJammerLat
            // 
            setJammerLat.Location = new Point(52, 22);
            setJammerLat.Margin = new Padding(3, 2, 3, 2);
            setJammerLat.Name = "setJammerLat";
            setJammerLat.Size = new Size(67, 27);
            setJammerLat.TabIndex = 7;
            setJammerLat.Text = "52,5200";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(130, 26);
            label1.Name = "label1";
            label1.Size = new Size(48, 20);
            label1.TabIndex = 6;
            label1.Text = "Long:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 26);
            label7.Name = "label7";
            label7.Size = new Size(35, 20);
            label7.TabIndex = 4;
            label7.Text = "Lat:";
            // 
            // setIntOff
            // 
            setIntOff.AutoSize = true;
            setIntOff.Checked = true;
            setIntOff.Cursor = Cursors.Hand;
            setIntOff.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            setIntOff.Location = new Point(67, 35);
            setIntOff.Margin = new Padding(3, 2, 3, 2);
            setIntOff.Name = "setIntOff";
            setIntOff.Size = new Size(48, 23);
            setIntOff.TabIndex = 1;
            setIntOff.TabStop = true;
            setIntOff.Text = "Off";
            setIntOff.UseVisualStyleBackColor = true;
            // 
            // setIntOn
            // 
            setIntOn.AutoSize = true;
            setIntOn.Cursor = Cursors.Hand;
            setIntOn.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            setIntOn.Location = new Point(15, 35);
            setIntOn.Margin = new Padding(3, 2, 3, 2);
            setIntOn.Name = "setIntOn";
            setIntOn.Size = new Size(46, 23);
            setIntOn.TabIndex = 0;
            setIntOn.Text = "On";
            setIntOn.UseVisualStyleBackColor = true;
            // 
            // setNMEA
            // 
            setNMEA.AutoSize = true;
            setNMEA.Checked = true;
            setNMEA.CheckState = CheckState.Checked;
            setNMEA.Cursor = Cursors.Hand;
            setNMEA.Location = new Point(758, 237);
            setNMEA.Margin = new Padding(3, 2, 3, 2);
            setNMEA.Name = "setNMEA";
            setNMEA.Size = new Size(101, 19);
            setNMEA.TabIndex = 13;
            setNMEA.Text = "NMEA Output";
            setNMEA.UseVisualStyleBackColor = true;
            // 
            // Stop
            // 
            Stop.BackColor = Color.Silver;
            Stop.Cursor = Cursors.Hand;
            Stop.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            Stop.ForeColor = Color.FromArgb(217, 40, 40);
            Stop.Location = new Point(757, 349);
            Stop.Margin = new Padding(3, 2, 3, 2);
            Stop.Name = "Stop";
            Stop.Size = new Size(102, 31);
            Stop.TabIndex = 14;
            Stop.Text = "Clear";
            Stop.UseVisualStyleBackColor = false;
            Stop.Click += Stop_Click;
            // 
            // labelPosAcc
            // 
            labelPosAcc.AutoSize = true;
            labelPosAcc.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            labelPosAcc.ForeColor = Color.White;
            labelPosAcc.Location = new Point(786, 287);
            labelPosAcc.Name = "labelPosAcc";
            labelPosAcc.Size = new Size(38, 17);
            labelPosAcc.TabIndex = 15;
            labelPosAcc.Text = "-,- m";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label10.Location = new Point(746, 270);
            label10.Name = "label10";
            label10.Size = new Size(121, 17);
            label10.TabIndex = 16;
            label10.Text = "Position Accuracy:";
            // 
            // GUI_Window
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(52, 50, 46);
            ClientSize = new Size(896, 412);
            Controls.Add(label10);
            Controls.Add(labelPosAcc);
            Controls.Add(Stop);
            Controls.Add(setNMEA);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(Simulate);
            Controls.Add(Terminal);
            ForeColor = Color.FromArgb(255, 184, 56);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "GUI_Window";
            ShowIcon = false;
            Text = "GUI GNSS";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)setSecond).EndInit();
            ((System.ComponentModel.ISupportInitialize)setMinute).EndInit();
            ((System.ComponentModel.ISupportInitialize)setHour).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)setRadR).EndInit();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox Terminal;
        private Button Simulate;
        private GroupBox groupBox1;
        private Label label3;
        private Label label2;
        private Label label4;
        private NumericUpDown setSecond;
        private NumericUpDown setMinute;
        private NumericUpDown setHour;
        private GroupBox groupBox2;
        private Label label5;
        private Label label6;
        private TextBox setLong;
        private TextBox setLat;
        private GroupBox groupBox3;
        private CheckBox setGps;
        private CheckBox setGlonass;
        private CheckBox setGalileo;
        private Button updatePos;
        private ComboBox setFile;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private Button updateJammerPos;
        private TextBox setJammerLong;
        private TextBox setJammerLat;
        private Label label1;
        private Label label7;
        private RadioButton setIntOff;
        private RadioButton setIntOn;
        private CheckBox setNMEA;
        private Button Stop;
        private Label label8;
        private GroupBox groupBox7;
        private TrackBar setRadR;
        private Label labelRadR;
        private Label labelPosAcc;
        private Label label10;
    }
}