using System.Text;

namespace GenAiMisinformation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string text = "Test";
            sb.Append(text);
            MessageBox.Show(sb.ToString());
        }
    }
}
