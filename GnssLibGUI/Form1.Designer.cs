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
            Close = new Button();
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
            groupBox7 = new GroupBox();
            labelRadR = new Label();
            setRadR = new TrackBar();
            groupBox6 = new GroupBox();
            updateJammerPos = new Button();
            setJammerLong = new TextBox();
            setJammerLat = new TextBox();
            label1 = new Label();
            label7 = new Label();
            setIntOff = new RadioButton();
            setIntOn = new RadioButton();
            setNMEA = new CheckBox();
            Stop = new Button();
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
            Terminal.Location = new Point(47, 313);
            Terminal.Multiline = true;
            Terminal.Name = "Terminal";
            Terminal.ReadOnly = true;
            Terminal.ScrollBars = ScrollBars.Both;
            Terminal.Size = new Size(817, 192);
            Terminal.TabIndex = 0;
            // 
            // Close
            // 
            Close.BackColor = Color.Black;
            Close.Location = new Point(927, 464);
            Close.Name = "Close";
            Close.Size = new Size(91, 41);
            Close.TabIndex = 1;
            Close.Text = "Close";
            Close.UseVisualStyleBackColor = false;
            Close.Click += Close_Click;
            // 
            // Simulate
            // 
            Simulate.BackColor = Color.DimGray;
            Simulate.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            Simulate.ForeColor = Color.FromArgb(56, 203, 52);
            Simulate.Location = new Point(748, 257);
            Simulate.Name = "Simulate";
            Simulate.Size = new Size(116, 41);
            Simulate.TabIndex = 2;
            Simulate.Text = "Simulate ";
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
            groupBox1.Location = new Point(47, 88);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(423, 60);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Time";
            // 
            // setSecond
            // 
            setSecond.Location = new Point(347, 22);
            setSecond.Margin = new Padding(2);
            setSecond.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            setSecond.Name = "setSecond";
            setSecond.Size = new Size(45, 31);
            setSecond.TabIndex = 10;
            // 
            // setMinute
            // 
            setMinute.Location = new Point(259, 24);
            setMinute.Margin = new Padding(2);
            setMinute.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            setMinute.Name = "setMinute";
            setMinute.Size = new Size(33, 31);
            setMinute.TabIndex = 9;
            // 
            // setHour
            // 
            setHour.Location = new Point(122, 22);
            setHour.Margin = new Padding(2);
            setHour.Maximum = new decimal(new int[] { 24, 0, 0, 0 });
            setHour.Name = "setHour";
            setHour.Size = new Size(33, 31);
            setHour.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(303, 25);
            label4.Name = "label4";
            label4.Size = new Size(39, 25);
            label4.TabIndex = 6;
            label4.Text = "sec";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(181, 24);
            label3.Name = "label3";
            label3.Size = new Size(73, 25);
            label3.TabIndex = 4;
            label3.Text = "Minute";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(60, 25);
            label2.Name = "label2";
            label2.Size = new Size(55, 25);
            label2.TabIndex = 2;
            label2.Text = "Hour";
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
            groupBox2.Location = new Point(47, 220);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(423, 78);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            groupBox2.Text = "Reciever Position";
            // 
            // updatePos
            // 
            updatePos.BackColor = Color.DimGray;
            updatePos.Location = new Point(322, 33);
            updatePos.Name = "updatePos";
            updatePos.Size = new Size(95, 34);
            updatePos.TabIndex = 10;
            updatePos.Text = "Update pos";
            updatePos.UseVisualStyleBackColor = false;
            // 
            // setLong
            // 
            setLong.Location = new Point(213, 35);
            setLong.Name = "setLong";
            setLong.Size = new Size(76, 31);
            setLong.TabIndex = 8;
            // 
            // setLat
            // 
            setLat.Location = new Point(60, 34);
            setLat.Name = "setLat";
            setLat.Size = new Size(76, 31);
            setLat.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(148, 35);
            label5.Name = "label5";
            label5.Size = new Size(59, 25);
            label5.TabIndex = 6;
            label5.Text = "Long:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 34);
            label6.Name = "label6";
            label6.Size = new Size(43, 25);
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
            groupBox3.Location = new Point(47, 22);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(423, 60);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "GNSS Type";
            // 
            // setGlonass
            // 
            setGlonass.AutoSize = true;
            setGlonass.Location = new Point(303, 25);
            setGlonass.Name = "setGlonass";
            setGlonass.Size = new Size(100, 29);
            setGlonass.TabIndex = 3;
            setGlonass.Text = "Glonass";
            setGlonass.UseVisualStyleBackColor = true;
            // 
            // setGalileo
            // 
            setGalileo.AutoSize = true;
            setGalileo.Location = new Point(170, 25);
            setGalileo.Name = "setGalileo";
            setGalileo.Size = new Size(93, 29);
            setGalileo.TabIndex = 2;
            setGalileo.Text = "Galileo";
            setGalileo.UseVisualStyleBackColor = true;
            // 
            // setGps
            // 
            setGps.AutoSize = true;
            setGps.Checked = true;
            setGps.CheckState = CheckState.Checked;
            setGps.Location = new Point(60, 25);
            setGps.Name = "setGps";
            setGps.Size = new Size(68, 29);
            setGps.TabIndex = 1;
            setGps.Text = "GPS";
            setGps.UseVisualStyleBackColor = true;
            // 
            // setFile
            // 
            setFile.DropDownStyle = ComboBoxStyle.DropDownList;
            setFile.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            setFile.FormattingEnabled = true;
            setFile.Location = new Point(122, 20);
            setFile.Name = "setFile";
            setFile.Size = new Size(284, 31);
            setFile.TabIndex = 10;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(setFile);
            groupBox4.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox4.ForeColor = Color.FromArgb(255, 184, 56);
            groupBox4.Location = new Point(47, 154);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(423, 60);
            groupBox4.TabIndex = 11;
            groupBox4.TabStop = false;
            groupBox4.Text = "Choose File";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(groupBox7);
            groupBox5.Controls.Add(groupBox6);
            groupBox5.Controls.Add(setIntOff);
            groupBox5.Controls.Add(setIntOn);
            groupBox5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox5.ForeColor = Color.FromArgb(255, 184, 56);
            groupBox5.Location = new Point(579, 22);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(454, 221);
            groupBox5.TabIndex = 12;
            groupBox5.TabStop = false;
            groupBox5.Text = "Interference";
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(labelRadR);
            groupBox7.Controls.Add(setRadR);
            groupBox7.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox7.ForeColor = Color.FromArgb(255, 184, 56);
            groupBox7.Location = new Point(16, 54);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(423, 82);
            groupBox7.TabIndex = 16;
            groupBox7.TabStop = false;
            groupBox7.Text = "Jammer Radius";
            // 
            // labelRadR
            // 
            labelRadR.AutoSize = true;
            labelRadR.Location = new Point(59, 37);
            labelRadR.Name = "labelRadR";
            labelRadR.Size = new Size(59, 28);
            labelRadR.TabIndex = 17;
            labelRadR.Text = "0 km";
            // 
            // setRadR
            // 
            setRadR.Location = new Point(148, 24);
            setRadR.Maximum = 100;
            setRadR.Name = "setRadR";
            setRadR.Size = new Size(272, 56);
            setRadR.TabIndex = 15;
            setRadR.TickFrequency = 10;
            setRadR.Scroll += setRadR_Scroll;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(updateJammerPos);
            groupBox6.Controls.Add(setJammerLong);
            groupBox6.Controls.Add(setJammerLat);
            groupBox6.Controls.Add(label1);
            groupBox6.Controls.Add(label7);
            groupBox6.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox6.ForeColor = Color.FromArgb(255, 184, 56);
            groupBox6.Location = new Point(16, 132);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(423, 78);
            groupBox6.TabIndex = 5;
            groupBox6.TabStop = false;
            groupBox6.Text = "Jammer Position";
            // 
            // updateJammerPos
            // 
            updateJammerPos.BackColor = Color.DimGray;
            updateJammerPos.Location = new Point(322, 33);
            updateJammerPos.Name = "updateJammerPos";
            updateJammerPos.Size = new Size(95, 34);
            updateJammerPos.TabIndex = 10;
            updateJammerPos.Text = "Update pos";
            updateJammerPos.UseVisualStyleBackColor = false;
            // 
            // setJammerLong
            // 
            setJammerLong.Location = new Point(213, 35);
            setJammerLong.Name = "setJammerLong";
            setJammerLong.Size = new Size(76, 31);
            setJammerLong.TabIndex = 8;
            // 
            // setJammerLat
            // 
            setJammerLat.Location = new Point(60, 34);
            setJammerLat.Name = "setJammerLat";
            setJammerLat.Size = new Size(76, 31);
            setJammerLat.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(148, 35);
            label1.Name = "label1";
            label1.Size = new Size(59, 25);
            label1.TabIndex = 6;
            label1.Text = "Long:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(16, 34);
            label7.Name = "label7";
            label7.Size = new Size(43, 25);
            label7.TabIndex = 4;
            label7.Text = "Lat:";
            // 
            // setIntOff
            // 
            setIntOff.AutoSize = true;
            setIntOff.Checked = true;
            setIntOff.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            setIntOff.Location = new Point(76, 35);
            setIntOff.Name = "setIntOff";
            setIntOff.Size = new Size(58, 27);
            setIntOff.TabIndex = 1;
            setIntOff.TabStop = true;
            setIntOff.Text = "Off";
            setIntOff.UseVisualStyleBackColor = true;
            // 
            // setIntOn
            // 
            setIntOn.AutoSize = true;
            setIntOn.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            setIntOn.Location = new Point(16, 35);
            setIntOn.Name = "setIntOn";
            setIntOn.Size = new Size(54, 27);
            setIntOn.TabIndex = 0;
            setIntOn.Text = "On";
            setIntOn.UseVisualStyleBackColor = true;
            // 
            // setNMEA
            // 
            setNMEA.AutoSize = true;
            setNMEA.Location = new Point(595, 263);
            setNMEA.Name = "setNMEA";
            setNMEA.Size = new Size(123, 24);
            setNMEA.TabIndex = 13;
            setNMEA.Text = "NMEA Output";
            setNMEA.UseVisualStyleBackColor = true;
            // 
            // Stop
            // 
            Stop.BackColor = Color.Silver;
            Stop.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point);
            Stop.ForeColor = Color.FromArgb(217, 40, 40);
            Stop.Location = new Point(897, 257);
            Stop.Name = "Stop";
            Stop.Size = new Size(116, 41);
            Stop.TabIndex = 14;
            Stop.Text = "Stop";
            Stop.UseVisualStyleBackColor = false;
            // 
            // GUI_Window
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(52, 50, 46);
            ClientSize = new Size(1047, 584);
            ControlBox = false;
            Controls.Add(Stop);
            Controls.Add(setNMEA);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(Simulate);
            Controls.Add(Close);
            Controls.Add(Terminal);
            ForeColor = Color.FromArgb(255, 184, 56);
            FormBorderStyle = FormBorderStyle.FixedSingle;
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
        private Button Close;
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
    }
}