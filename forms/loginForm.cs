using System;
using System.Collections.Generic;
using System.Drawing;
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

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(189, 236, 175);
            SetupModernLayout();
        }

        private void SetupModernLayout()
        {
            // Main panel to hold all controls
            Panel panel = new Panel
            {
                Size = new Size(400, 350),
                BackColor = Color.WhiteSmoke,
                Location = new Point((this.ClientSize.Width - 400) / 2, (this.ClientSize.Height - 350) / 2),
            };
            this.Controls.Add(panel);

            // Title label
            Label titleLabel = new Label
            {
                Text = "GenAI Login",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                AutoSize = true,
                Location = new Point((panel.Width - 220) / 2, 30)
            };
            panel.Controls.Add(titleLabel);

            // Username label
            label2.Font = new Font("Segoe UI", 12);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(50, 100);
            panel.Controls.Add(label2);

            // Username TextBox inside a panel for rounded effect
            Panel userBoxPanel = new Panel
            {
                Size = new Size(300, 35),
                BackColor = Color.FromArgb(189, 236, 175),
                Location = new Point((panel.Width - 300) / 2, 130)
            };
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Segoe UI", 12);
            textBox1.Location = new Point(5, 5);
            textBox1.Width = 290;
            userBoxPanel.Controls.Add(textBox1);
            panel.Controls.Add(userBoxPanel);

            // Password label
            label3.Font = new Font("Segoe UI", 12);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(50, 180);
            panel.Controls.Add(label3);

            // Password TextBox inside a panel for rounded effect
            Panel passBoxPanel = new Panel
            {
                Size = new Size(300, 35),
                BackColor = Color.FromArgb(189, 236, 175),
                Location = new Point((panel.Width - 300) / 2, 210)
            };
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Font = new Font("Segoe UI", 12);
            textBox2.UseSystemPasswordChar = true;
            textBox2.Location = new Point(5, 5);
            textBox2.Width = 290;
            passBoxPanel.Controls.Add(textBox2);
            panel.Controls.Add(passBoxPanel);

            // Login button
            button1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button1.Width = 150;
            button1.Height = 40;
            button1.BackColor = Color.FromArgb(189, 236, 175);
            button1.ForeColor = Color.Black;
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.Location = new Point((panel.Width - button1.Width) / 2, 280);
            panel.Controls.Add(button1);

            // Enter key triggers login
            this.AcceptButton = button1;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                button1.Enabled = false;
                bool isSuccess = await ValidateCredentialAsync(username, password);

                if (isSuccess)
                {
                    var dashboardForm = new dashboardForm();
                    dashboardForm.StartPosition = FormStartPosition.CenterScreen;
                    dashboardForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
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

                if (_credentials.TryGetValue(username, out var correctPassword))
                {
                    return string.Equals(password, correctPassword, StringComparison.Ordinal);
                }

                return false;
            });
        }


        private void loginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
