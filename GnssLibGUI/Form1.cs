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
            String hey = tc.hello((int)setHour.Value, (int)setMinute.Value, (int)setSecond.Value);
            Terminal.Text += hey + " __ ";
        }

        private void setRadR_Scroll(object sender, EventArgs e)
        {
            labelRadR.Text = setRadR.Value.ToString() + " km";
        }
    }
}