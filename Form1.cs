using CsvHelper;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
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
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "generative_ai_misinformation_dataset.csv");

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<MisinformationRecord>().ToList();
                dataGridView1.DataSource = records;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "generative_ai_misinformation_dataset.csv");

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<MisinformationRecord>().ToList();
                dataGridView1.DataSource = records;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "generative_ai_misinformation_dataset.csv");

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<MisinformationRecord>().ToList();

                // 🔍 Filter where Platform == "Facebook"
                var filteredRecords = records
                    .Where(r => r.platform.Equals("Facebook", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                dataGridView1.DataSource = filteredRecords;
            }
        }
    }
}
