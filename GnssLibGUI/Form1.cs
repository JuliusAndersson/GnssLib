using GnssLibDL;
using GnssLibNMEA_Writer;
using System.Diagnostics;

namespace GnssLibGUI
{
    public partial class GUI_Window : Form
    {
        private SimulationController? sc;
        private SimulationRunTime srt;
        private bool stopClear = true;
        public GUI_Window()
        {
            InitializeComponent();

            setFile.Items.Add("File 1");
            setFile.Items.Add("File 2");
            setFile.Items.Add("File 3");
            setFile.SelectedIndex = 0;


            srt = new SimulationRunTime();
            srt.tickDone += HandleTickEvent;

        }

        private void Simulate_Click(object sender, EventArgs e)
        {
            DateTime dt;
            Stop.Text = "Stop";
            stopClear = false;
            if (setFile.SelectedIndex == 0)
            {
                dt = new DateTime(2024, 01, 01, (int)setHour.Value, (int)setMinute.Value, (int)setSecond.Value);
            }
            else if (setFile.SelectedIndex == 1)
            {
                dt = new DateTime(2024, 01, 01, (int)setHour.Value, (int)setMinute.Value, (int)setSecond.Value);
            }
            else
            {
                dt = new DateTime(2024, 01, 01, (int)setHour.Value, (int)setMinute.Value, (int)setSecond.Value);
            }

            double value;
            if (double.TryParse(setLat.Text, out value) && double.TryParse(setLong.Text, out value) && double.TryParse(setJammerLat.Text, out value) && double.TryParse(setJammerLong.Text, out value))
            {
                sc = new SimulationController(setGps.Checked, setGalileo.Checked, setGlonass.Checked, dt, double.Parse(setLat.Text), double.Parse(setLong.Text), setIntOn.Checked, double.Parse(setRadR.Value.ToString()), double.Parse(setJammerLat.Text), double.Parse(setJammerLong.Text), setNMEA.Checked);

                srt.RunSimulation(sc);


                Terminal.Text += "it worked" + Environment.NewLine;
            }
            else
            {
                Terminal.Text = "Invalid inputs (Check if you use , or . in Coordinates)" + Environment.NewLine;
            }



        }

        public void HandleTickEvent(object sender, EventArgs e)
        {
            Terminal.Text += "Tick" + Environment.NewLine;
        }

        private void setRadR_Scroll(object sender, EventArgs e)
        {
            labelRadR.Text = setRadR.Value.ToString() + " km";
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            Stop.Text = "Clear";
            srt.StopSimulation();
            if (stopClear)
            {
                Terminal.Clear();
            }
            stopClear = true;
        }

        private void updatePos_Click(object sender, EventArgs e)
        {
            if (sc != null) {
                sc.UpdatePos(double.Parse(setLat.Text), double.Parse(setLong.Text));
                Terminal.Text += "New Position Value Set: " + setLat.Text + " " + setLong.Text + Environment.NewLine;
            }
            
        }

        private void updateJammerPos_Click(object sender, EventArgs e)
        {
            if (sc != null)
            {
                sc.UpdateJammerPos(setIntOn.Checked, double.Parse(setJammerLat.Text), double.Parse(setJammerLong.Text), double.Parse(setRadR.Value.ToString()));
                Terminal.Text += "New Jammer Value Set: " + setJammerLat.Text + " " + setJammerLong.Text + Environment.NewLine;
            }
             

        }

    }
}