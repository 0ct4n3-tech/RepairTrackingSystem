using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using RepairTrackerSystem.Core;
using System.Windows.Forms;
namespace RepairTrackerSystem
{

public partial class DashboardForm : Form
{
    public DashboardForm()
    {
        InitializeComponent();
    }

    private void dgvRecent_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void DashboardForm_Load(object sender, EventArgs e)
    {
        // populate top cards using DatabaseService
        var repairsAll = DatabaseService.GetRepairs();
        var total = repairsAll.Count;
        var pending = repairsAll.Count(r => string.Equals(r.Status, "Pending", StringComparison.OrdinalIgnoreCase));
        var inProgress = repairsAll.Count(r => string.Equals(r.Status, "Repairing", StringComparison.OrdinalIgnoreCase) || string.Equals(r.Status, "Diagnosing", StringComparison.OrdinalIgnoreCase));
        var completed = repairsAll.Count(r => string.Equals(r.Status, "Fixed", StringComparison.OrdinalIgnoreCase) || string.Equals(r.Status, "Released", StringComparison.OrdinalIgnoreCase));

        label3.Text = total.ToString();
        label4.Text = pending.ToString();
        label6.Text = inProgress.ToString();
        label8.Text = completed.ToString();

        // recent repairs table
        dgvRecent.Rows.Clear();
        foreach (var r in repairsAll.OrderByDescending(x => x.DateReceived).Take(5))
        {
            var cust = DatabaseService.GetCustomer(r.CustomerID)?.Name ?? "";
            var dev = DatabaseService.GetDevice(r.DeviceID)?.Type ?? "";
            dgvRecent.Rows.Add(r.RepairID, cust, dev, r.Status, r.DateReceived.ToShortDateString());
        }

        // simple textual overview inside panel1
        panel1.Controls.Clear();
        var statuses = repairsAll.GroupBy(r => r.Status).Select(g => new { Status = g.Key, Count = g.Count() }).ToList();
        int y = 10;
        foreach (var s in statuses)
        {
            var lbl = new Label { Text = $"{s.Status}: {s.Count}", Location = new Point(10, y), AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Regular) };
            panel1.Controls.Add(lbl);
            y += 25;
        }
    }

    private void panelMain_Paint(object sender, PaintEventArgs e)
    {

    }

    private void panelPending_Paint(object sender, PaintEventArgs e)
    {

    }

    private void panel1_Paint(object sender, PaintEventArgs e)
    {

    }

    private void chartOverview_Click(object sender, EventArgs e)
    {
        chartOverview.Series.Clear();

        Series series = new Series("Repairs");
        series.ChartType = SeriesChartType.Pie;

        var repairsAll = DatabaseService.GetRepairs();
        int pending = repairsAll.Count(r => r.Status == "Pending");
        int progress = repairsAll.Count(r => r.Status == "Repairing");
        int completed = repairsAll.Count(r => r.Status == "Completed" || r.Status == "Released" || r.Status == "Fixed");

        series.Points.AddXY("Pending", pending);
        series.Points.AddXY("In Progress", progress);
        series.Points.AddXY("Completed", completed);

        chartOverview.Series.Add(series);
    }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void lblRecent_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void panelTotal_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panelCompleted_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void panelProgress_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panelTopCards_Paint(object sender, PaintEventArgs e)
        {

        }
    } }