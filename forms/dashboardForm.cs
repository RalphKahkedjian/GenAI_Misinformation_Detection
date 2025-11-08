using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenAiMisinformation.forms
{
    public partial class dashboardForm : Form
    {
        private readonly Mutex dataMutex = new Mutex();
        private readonly SemaphoreSlim loadSemaphore = new SemaphoreSlim(1, 1);

        private Panel sidebarPanel;
        private Panel mainPanel;
        private FlowLayoutPanel buttonPanel;

        private Button btnViewData;
        private Button btnAnalyze;
        private Button btnCancel;

        private Panel resultsPanel;
        private Label lblResultsTitle;
        private Label lblTotalPosts;
        private Label lblMisinfoPosts;
        private Label lblMisinfoRate;

        private viewDataControl viewDataControl;
        private CancellationTokenSource analysisCts;

        public dashboardForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "AI Misinformation Detection";
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(1200, 800);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // ----- Sidebar -----
            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 380,
                BackColor = ColorTranslator.FromHtml("#BDECAF")
            };

            // ----- Main area (right) -----
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };

            this.Controls.Add(mainPanel);
            this.Controls.Add(sidebarPanel);

            // ----- Button panel (under title, fixed) -----
            buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 20, 0, 20),
                WrapContents = false,
                Height = 230,
                AutoSize = false
            };

            btnViewData = CreateSidebarButton("View Data");
            btnViewData.Click += (s, e) => showPage(viewDataControl);
            buttonPanel.Controls.Add(btnViewData);

            btnAnalyze = CreateSidebarButton("Analyze");
            btnAnalyze.Click += (s, e) => StartAnalysis();
            buttonPanel.Controls.Add(btnAnalyze);

            btnCancel = CreateSidebarButton("Cancel");
            btnCancel.Click += (s, e) => CancelAnalysis();
            btnCancel.Enabled = false;
            buttonPanel.Controls.Add(btnCancel);

            // ----- Results panel at bottom -----
            resultsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 160,
                BackColor = Color.FromArgb(230, 255, 230)
            };

            lblResultsTitle = new Label
            {
                Text = "Analysis Results",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(15, 15)
            };
            resultsPanel.Controls.Add(lblResultsTitle);

            lblTotalPosts = new Label
            {
                Text = "Total posts: -",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(25, 45)
            };
            resultsPanel.Controls.Add(lblTotalPosts);

            lblMisinfoPosts = new Label
            {
                Text = "Misinformation posts: -",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(25, 70)
            };
            resultsPanel.Controls.Add(lblMisinfoPosts);

            lblMisinfoRate = new Label
            {
                Text = "Misinformation rate: -",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(25, 95)
            };
            resultsPanel.Controls.Add(lblMisinfoRate);

            // ----- Title panel (added LAST so it stays on top) -----
            Panel titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = ColorTranslator.FromHtml("#BDECAF")
            };

            Label titleLabel = new Label
            {
                Text = "🧠 GenAI Misinformation Detection",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            titlePanel.Controls.Add(titleLabel);

            // Important: add controls to sidebar in this order
            // so Docking results in: Title (top), Buttons (under), Results (bottom)
            sidebarPanel.Controls.Add(resultsPanel);
            sidebarPanel.Controls.Add(buttonPanel);
            sidebarPanel.Controls.Add(titlePanel);

            // ----- Default page -----
            viewDataControl = new viewDataControl();
            showPage(viewDataControl);
        }

        private Button CreateSidebarButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 45),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Height = 50,
                Width = 240,
                Margin = new Padding(70, 10, 70, 10),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(70, 70, 70);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(45, 45, 45);

            return btn;
        }

        private async void showPage(UserControl userControl)
        {
            foreach (Control control in mainPanel.Controls)
                control.Visible = false;

            if (!mainPanel.Controls.Contains(userControl))
            {
                userControl.Dock = DockStyle.Fill;
                mainPanel.Controls.Add(userControl);
            }

            await Task.Run(() =>
            {
                dataMutex.WaitOne();
                loadSemaphore.Wait();
                try
                {
                    Thread.Sleep(300);
                }
                finally
                {
                    loadSemaphore.Release();
                    dataMutex.ReleaseMutex();
                }
            });

            if (!userControl.IsDisposed)
            {
                userControl.Visible = true;
                userControl.BringToFront();
            }
        }

        private async void StartAnalysis()
        {
            if (viewDataControl == null || viewDataControl.Records == null || viewDataControl.Records.Count == 0)
            {
                MessageBox.Show("Please click 'View Data' first to load the dataset.",
                    "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (analysisCts != null)
            {
                MessageBox.Show("Analysis is already running.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            analysisCts = new CancellationTokenSource();
            var token = analysisCts.Token;

            btnAnalyze.Enabled = false;
            btnViewData.Enabled = false;
            btnCancel.Enabled = true;

            lblTotalPosts.Text = "Total posts: ...";
            lblMisinfoPosts.Text = "Misinformation posts: ...";
            lblMisinfoRate.Text = "Misinformation rate: ...";

            try
            {
                var records = viewDataControl.Records;

                var result = await Task.Run(() =>
                {
                    int total = 0;
                    int misinfo = 0;

                    foreach (var r in records)
                    {
                        token.ThrowIfCancellationRequested();

                        total++;
                        if (r.is_misinformation)
                            misinfo++;

                        Thread.Sleep(1);
                    }

                    double rate = total == 0 ? 0 : (double)misinfo * 100.0 / total;
                    return Tuple.Create(total, misinfo, rate);
                }, token);

                if (!token.IsCancellationRequested && !this.IsDisposed)
                {
                    this.Invoke((Action)(() =>
                    {
                        lblTotalPosts.Text = "Total posts: " + result.Item1;
                        lblMisinfoPosts.Text = "Misinformation posts: " + result.Item2;
                        lblMisinfoRate.Text = "Misinformation rate: " + result.Item3.ToString("0.00") + " %";
                    }));
                }
            }
            catch (OperationCanceledException)
            {
                if (!this.IsDisposed)
                {
                    this.Invoke((Action)(() =>
                    {
                        lblTotalPosts.Text = "Analysis canceled.";
                        lblMisinfoPosts.Text = "Misinformation posts: -";
                        lblMisinfoRate.Text = "Misinformation rate: -";
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during analysis:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!this.IsDisposed)
                {
                    this.Invoke((Action)(() =>
                    {
                        btnAnalyze.Enabled = true;
                        btnViewData.Enabled = true;
                        btnCancel.Enabled = false;
                    }));
                }

                analysisCts = null;
            }
        }

        private void CancelAnalysis()
        {
            analysisCts?.Cancel();
        }

        private void dashboardForm_Load(object sender, EventArgs e)
        {
        }
    }
}
