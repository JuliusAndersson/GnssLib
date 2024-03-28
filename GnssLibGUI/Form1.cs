using GnssLibDL;

namespace GnssLibGUI
{
    public partial class GUI_Window : Form
    {
        public GUI_Window()
        {
            InitializeComponent();

            setFile.Items.Add("File 1");
            setFile.Items.Add("File 2");
            setFile.Items.Add("File 3");
            setFile.SelectedIndex = 0;



        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Simulate_Click(object sender, EventArgs e)
        {
            TestClass tc = new TestClass();
            //String hey = tc.hello((int)setHour.Value, (int)setMinute.Value, (int)setSecond.Value);
            //Terminal.Text += hey + " __ ";
            //Terminal.Text += tc.testVar;
            DateTime dt;
            if (setFile.SelectedIndex == 0)
            {
                dt = new DateTime(2024, 01, 01, (int)setHour.Value, (int)setMinute.Value, (int)setSecond.Value);
            }
            else if (setFile.SelectedIndex == 1)
            {
                dt = new DateTime(2024, 02, 02, (int)setHour.Value, (int)setMinute.Value, (int)setSecond.Value);
            }
            else
            {
                dt = new DateTime(2024, 03, 03, (int)setHour.Value, (int)setMinute.Value, (int)setSecond.Value);
            }




            double value;
            if (double.TryParse(setLat.Text, out value) && double.TryParse(setLong.Text, out value) && double.TryParse(setJammerLat.Text, out value) && double.TryParse(setJammerLong.Text, out value))
            {
                string hey = tc.setValues(setGps.Checked, setGalileo.Checked, setGlonass.Checked, dt, double.Parse(setLat.Text), double.Parse(setLong.Text), setIntOn.Checked, double.Parse(setRadR.Value.ToString()), double.Parse(setJammerLat.Text), double.Parse(setJammerLong.Text));
                Terminal.Text += hey + Environment.NewLine;
            }
            else
            {
                Terminal.Text = "Invalid inputs";
            }
        }

        private void setRadR_Scroll(object sender, EventArgs e)
        {
            labelRadR.Text = setRadR.Value.ToString() + " km";
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            Terminal.Clear();
        }

        private void updateJammerPos_Click(object sender, EventArgs e)
        {






        }
    }
}