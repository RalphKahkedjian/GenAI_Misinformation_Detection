using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenAiMisinformation.forms
{
    public partial class viewDataControl : UserControl
    {
        public viewDataControl()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            dataGridView1 = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            this.Controls.Add(dataGridView1);
            this.Load += (s, e) => viewDataControl_Load(s, e);
        }

        private void viewDataControl_Load(object sender, EventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "generative_ai_misinformation_dataset.csv");

            if (!File.Exists(path))
            {
                MessageBox.Show("CSV file not found: " + path);
                return;
            }

            using (var reader = new StreamReader(path))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<MisinformationRecord>().ToList();
                dataGridView1.DataSource = records;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
