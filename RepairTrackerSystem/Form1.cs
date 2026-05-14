using System;
using System.Data.SQLite;
using System.Windows.Forms;
using RepairTrackerSystem.Core;

namespace RepairTrackerSystem
{
    public partial class Form1 : Form
    {
        public static string LoggedInUser = "";

        public static Form1 Instance { get; private set; }

        Form currentForm;

        public Form1()
        {
            InitializeComponent();
            Instance = this;
        }

        private void InitializeDatabase()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string customerTable = @"CREATE TABLE IF NOT EXISTS Customers (
                    ID TEXT PRIMARY KEY,
                    Name TEXT,
                    Contact TEXT,
                    Address TEXT
                );";

                string repairTable = @"CREATE TABLE IF NOT EXISTS Repairs (
                    RepairID TEXT PRIMARY KEY,
                    CustomerID TEXT,
                    DeviceID TEXT,
                    TechnicianID TEXT,
                    Status TEXT,
                    Issue TEXT,
                    Cost REAL,
                    DateReceived TEXT,
                    DateUpdated TEXT
                );";

                string deviceTable = @"CREATE TABLE IF NOT EXISTS Devices (
                    ID TEXT PRIMARY KEY,
                    Type TEXT,
                    Brand TEXT,
                    Model TEXT,
                    CustomerID TEXT
                );";

                string technicianTable = @"CREATE TABLE IF NOT EXISTS Technicians (
                    ID TEXT PRIMARY KEY,
                    Name TEXT,
                    Specialty TEXT,
                    Contact TEXT
                );";

                new SQLiteCommand(customerTable, conn).ExecuteNonQuery();
                new SQLiteCommand(repairTable, conn).ExecuteNonQuery();
                new SQLiteCommand(deviceTable, conn).ExecuteNonQuery();
                new SQLiteCommand(technicianTable, conn).ExecuteNonQuery();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeDatabase();

            var dashboard = new RepairDashboard.DashboardForm();
            dashboard.MainForm = this;
            dashboard.OnSeeAll = () => LoadForm(new RepairsFormcs());

            LoadForm(dashboard);

        }

        public void LoadForm(Form f)
        {
            if (currentForm != null)
                currentForm.Close();

            currentForm = f;

            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.Dock = DockStyle.Fill;

            panelMain.Controls.Clear();
            panelMain.Controls.Add(f);
            f.Show();
        }

        public void RefreshDashboard()
        {
            if (currentForm is RepairDashboard.DashboardForm dashboard)
            {
                dashboard.RefreshDashboard();
            }
        }

        // ================= SIDEBAR =================


        

        private void btnRepairs_Click(object sender, EventArgs e)
        {
            LoadForm(new RepairsFormcs());
        }

        private void btnDevices_Click(object sender, EventArgs e)
        {
            LoadForm(new DevicesForm());
        }

        private void btnCustomers_Click_1(object sender, EventArgs e)
        {
            LoadForm(new CustomersForm());
        }

        private void btnDashboard_Click_1(object sender, EventArgs e)
        {

            var dashboard = new RepairDashboard.DashboardForm();
            dashboard.MainForm = this;
            dashboard.OnSeeAll = () => LoadForm(new RepairsFormcs());
            LoadForm(dashboard);

        }

        private void btnReports_Click_1(object sender, EventArgs e)
        {
            LoadForm(new ReportsForm());
        }

        private void btnTechnicians_Click_1(object sender, EventArgs e)
        {
            LoadForm(new TechniciansForm());
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAnalytics_Click(object sender, EventArgs e)
        {
            LoadForm(new AnalyticsForm());
        }
    }
}