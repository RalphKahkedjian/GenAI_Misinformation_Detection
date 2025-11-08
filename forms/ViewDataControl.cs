using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GenAiMisinformation; // for MisinformationRecord

namespace GenAiMisinformation.forms
{
    public partial class viewDataControl : UserControl
    {
        // Expose records so dashboardForm can analyze them
        public List<MisinformationRecord> Records { get; private set; } = new List<MisinformationRecord>();

        public viewDataControl()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            // Style the existing designer dataGridView1
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;

            // Ensure load handler is hooked (Designer may already do this)
            this.Load += viewDataControl_Load;
        }

        private void viewDataControl_Load(object sender, EventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "generative_ai_misinformation_dataset.csv");

            if (!File.Exists(path))
            {
                MessageBox.Show("CSV file not found: " + path, "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<MisinformationRecord>().ToList();
                    Records = records;
                    dataGridView1.DataSource = Records;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading CSV:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // not used, but can stay for Designer wiring
        }
    }
}
