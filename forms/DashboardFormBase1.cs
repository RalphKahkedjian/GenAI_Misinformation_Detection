using System;
using System.Windows.Forms;

namespace MisinformationDetector.forms
{
    public partial class DashboardFormBase1 : Form
    {
        private System.ComponentModel.IContainer components = null;

        public DashboardFormBase1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Text = "Dashboard Base Form";
        }
    }
}
