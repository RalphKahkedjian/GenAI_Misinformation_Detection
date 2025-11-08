using System.Threading;
using System.Threading.Tasks;


namespace GenAiMisinformation.forms
{
    public partial class dashboardForm : Form
    {
        private Mutex dataMutex = new Mutex();
        private SemaphoreSlim loadSemaphore = new SemaphoreSlim(1, 1);

        private Button btnViewData;
        private Button btnAdminView;
        private Button btnInsights;
        private Panel sidebarPanel;
        private Panel mainPanel;

        private UserControl viewDataControl;
        private UserControl adminViewControl;
        private UserControl insightsControl;


        public dashboardForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "AI Misinformation Detection";
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.WhiteSmoke;

            sidebarPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                BackColor = System.Drawing.ColorTranslator.FromHtml("#BDECAF")
            };


            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.WhiteSmoke
            };
            this.Controls.Add(mainPanel);
            this.Controls.Add(sidebarPanel);

            viewDataControl = new viewDataControl();

            Label titleLabel = new Label
            {
                Text = "🧠 GenAI Misinformation Detection",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 120,
            };
            sidebarPanel.Controls.Add(titleLabel);

            btnViewData = createSidebarButton("View Data", 300);
            btnViewData.Click += (s, e) => showPage(viewDataControl);

            btnAdminView = createSidebarButton("Admin View", 390);
            //btnAdminView.Click += (s, e) => showPage(adminViewControl);

            btnInsights = createSidebarButton("Insights", 480);
            //btnInsights.Click += (s, e) => showPage(viewDataControl);

            sidebarPanel.Controls.Add(btnViewData);
            sidebarPanel.Controls.Add(btnAdminView);
            sidebarPanel.Controls.Add(btnInsights);
        }

        private Button createSidebarButton(string text, int top)
        {
            Button btn = new Button
            {
                Text = text,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 45),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Height = 50,
                Width = 220,
                Top = top,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.Left = (sidebarPanel.Width - btn.Width) / 2;
            sidebarPanel.Resize += (s, e) =>
            {
                btn.Left = (sidebarPanel.Width - btn.Width) / 2;
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
                    Thread.Sleep(1000);
                }
                finally
                {
                    loadSemaphore.Release();
                    dataMutex.ReleaseMutex();
                }
            });

            userControl.Visible = true;
            userControl.BringToFront();
        }


        private void dashboardForm_Load(object sender, EventArgs e)
        {

        }
    }
}
