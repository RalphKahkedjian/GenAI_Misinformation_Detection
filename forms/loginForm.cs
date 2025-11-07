using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenAiMisinformation.forms
{

    public partial class loginForm : Form
    {

        private readonly Dictionary<string, string> _credentials = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            {"Ralph", "Ralph123" },
            {"Elie", "Elie123" }
        };

        public loginForm()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                button1.Enabled = false;
                bool isSuccess = await ValidateCredentialAsync(username, password);

                if(isSuccess)
                {
                    this.Hide();
                }

                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occurred during login:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
            }
        }

        private Task<bool> ValidateCredentialAsync(string username, string password)
        {
            return Task.Run(async () =>
            {
                await Task.Delay(500);
                if(_credentials.TryGetValue(username, out string correctPassword))
                {
                    return string.Equals(password, correctPassword, StringComparison.Ordinal);
                }

                return false;
            });
        }
    }
}
