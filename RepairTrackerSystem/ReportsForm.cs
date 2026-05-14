// ============================================================
//  REPAIR TRACKER — REPORTS FORM  (WebBrowser Version)
//  Uses the built-in WebBrowser control (no WebView2 needed).
//  Uses your existing Database.GetConnection() + SQLite.
//
//  NO extra NuGet needed — WebBrowser is built into WinForms.
// ============================================================

using System;
using System.Data.SQLite;
using System.Windows.Forms;
using Microsoft.Win32;                  // for IE11 registry fix
using RepairTrackerSystem.Core;

namespace RepairTrackerSystem
{
    public partial class ReportsForm : Form
    {
        // ── Counts loaded from SQLite ─────────────────────────────────────────
        private int _total = 0;
        private int _pending = 0;
        private int _repairing = 0;
        private int _diagnosing = 0;
        private int _completed = 0;

        public ReportsForm()
        {
            InitializeComponent();
            SetIE11Mode();          // ← must run before form loads
            this.Load += ReportsForm_Load;
        }

        // ─────────────────────────────────────────────────────────────────────
        //  CRITICAL — Force WebBrowser to use IE11 rendering engine
        //  Without this Chart.js will not render (defaults to IE7).
        //  This writes one registry key for your .exe only.
        // ─────────────────────────────────────────────────────────────────────
        private static void SetIE11Mode()
        {
            try
            {
                string exeName = System.IO.Path.GetFileName(
                    System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

                using (var key = Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                    writable: true) ??
                    Registry.CurrentUser.CreateSubKey(
                    @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"))
                {
                    // 11001 = IE11 edge mode
                    key.SetValue(exeName, 11001, RegistryValueKind.DWord);
                }
            }
            catch
            {
                // Fails silently — chart may not render on first run,
                // restart the app and it will work.
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        //  FORM LOAD
        // ─────────────────────────────────────────────────────────────────────
        private void ReportsForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            LoadCounts();           // 1. Pull counts from SQLite
            UpdateSummaryCards();   // 2. Update your summary labels
            RenderChart();          // 3. Draw the pie chart
        }

        // ─────────────────────────────────────────────────────────────────────
        //  STEP 1 — LOAD STATUS COUNTS FROM SQLITE
        //  Uses your Database.GetConnection() — no connection string needed.
        //  Status values: 'Pending' | 'Repairing' | 'Diagnosing' | 'Completed'
        // ─────────────────────────────────────────────────────────────────────
        private void LoadCounts()
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        SELECT
                            COUNT(*)                                              AS Total,
                            SUM(CASE WHEN Status='Pending'    THEN 1 ELSE 0 END) AS Pending,
                            SUM(CASE WHEN Status='Repairing'  THEN 1 ELSE 0 END) AS Repairing,
                            SUM(CASE WHEN Status='Diagnosing' THEN 1 ELSE 0 END) AS Diagnosing,
                            SUM(CASE WHEN Status='Completed'  THEN 1 ELSE 0 END) AS Completed
                        FROM Repairs;";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            _total = r["Total"] != DBNull.Value ? Convert.ToInt32(r["Total"]) : 0;
                            _pending = r["Pending"] != DBNull.Value ? Convert.ToInt32(r["Pending"]) : 0;
                            _repairing = r["Repairing"] != DBNull.Value ? Convert.ToInt32(r["Repairing"]) : 0;
                            _diagnosing = r["Diagnosing"] != DBNull.Value ? Convert.ToInt32(r["Diagnosing"]) : 0;
                            _completed = r["Completed"] != DBNull.Value ? Convert.ToInt32(r["Completed"]) : 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load report data:\n" + ex.Message,
                                "Database Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────────────
        //  STEP 2 — UPDATE YOUR SUMMARY LABELS
        //  Change lblTotal / lblPending / lblCompleted
        //  to your actual label names in the designer.
        // ─────────────────────────────────────────────────────────────────────
        private void UpdateSummaryCards()
        {
            if (lblTotal != null)
                lblTotal.Text = _total.ToString();
            if (lblPending != null)
                lblPending.Text = _pending.ToString();
            if (lblCompleted != null)
                lblCompleted.Text = _completed.ToString();
        }

        // ─────────────────────────────────────────────────────────────────────
        //  STEP 3 — RENDER PIE CHART USING WebBrowser + Chart.js 2.9.4
        //  Chart.js v2 is used because it supports IE11 (WebBrowser engine).
        //  Make sure your WebBrowser control is named: webBrowser1
        // ─────────────────────────────────────────────────────────────────────
        private void RenderChart()
        {
            if (webBrowser1 == null)
            {
                MessageBox.Show("WebBrowser control not found. Please ensure webBrowser1 is added to the form.",
                                "Control Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            string today = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            string html = $@"
<!DOCTYPE html>
<html>
<head>
<meta http-equiv='X-UA-Compatible' content='IE=Edge' />
<meta charset='UTF-8'>
<script src='https://cdn.jsdelivr.net/npm/chart.js@2.9.4/dist/Chart.min.js'></script>
<style>
  * {{ margin:0; padding:0; box-sizing:border-box; }}
 
  html, body {{
    width: 100%;
    height: 100%;
    overflow: hidden;
    background: #f4f6f9;
    font-family: 'Segoe UI', sans-serif;
  }}
 
  .wrapper {{
    width: 100%;
    height: 100%;
    display: -ms-flexbox;
    display: flex;
    -ms-flex-direction: column;
    flex-direction: column;
    padding: 14px 18px;
  }}
 
  .card-title {{
    font-size: 14px;
    font-weight: 700;
    color: #1e293b;
    margin-bottom: 2px;
  }}
 
  .card-sub {{
    font-size: 11px;
    color: #94a3b8;
    margin-bottom: 10px;
  }}
 
  /* Chart fills remaining space */
  .chart-area {{
    -ms-flex: 1;
    flex: 1;
    position: relative;
    min-height: 0;
  }}
 
  canvas {{
    position: absolute;
    top: 0; left: 0;
    width: 100% !important;
    height: 100% !important;
  }}
 
  /* Legend pinned at bottom */
  .legend {{
    padding-top: 10px;
    display: -ms-flexbox;
    display: flex;
    -ms-flex-direction: column;
    flex-direction: column;
    gap: 6px;
  }}
 
  .legend-row {{
    display: -ms-flexbox;
    display: flex;
    -ms-flex-align: center;
    align-items: center;
    gap: 8px;
  }}
 
  .legend-dot {{
    width: 11px; height: 11px;
    border-radius: 50%;
    display: inline-block;
    -ms-flex-negative: 0;
    flex-shrink: 0;
  }}
 
  .legend-label {{
    font-size: 12px;
    color: #475569;
    -ms-flex: 1;
    flex: 1;
  }}
 
  .legend-bar-wrap {{
    -ms-flex: 2;
    flex: 2;
    background: #e2e8f0;
    border-radius: 4px;
    height: 6px;
    overflow: hidden;
  }}
 
  .legend-bar {{
    height: 100%;
    border-radius: 4px;
  }}
 
  .legend-count {{
    font-size: 12px;
    font-weight: 700;
    color: #1e293b;
    min-width: 24px;
    text-align: right;
  }}
 
  .total-row {{
    margin-top: 8px;
    padding-top: 8px;
    border-top: 1px solid #e2e8f0;
    display: -ms-flexbox;
    display: flex;
    -ms-flex-pack: justify;
    justify-content: space-between;
    font-size: 12px;
    color: #64748b;
  }}
 
  .total-row span {{
    font-weight: 700;
    color: #1e293b;
  }}
 
  .timestamp {{
    font-size: 10px;
    color: #b0bec5;
    text-align: right;
    margin-top: 6px;
  }}
</style>
</head>
<body>
 
<div class='wrapper'>
  <div class='card-title'>Repair Status Breakdown</div>
  <div class='card-sub'>All repairs &middot; as of today</div>
 
  <div class='chart-area'>
    <canvas id='chart'></canvas>
  </div>
 
  <div class='legend'>
    <div class='legend-row'>
      <span class='legend-dot' style='background:#4ade80'></span>
      <span class='legend-label'>Completed</span>
      <div class='legend-bar-wrap'>
        <div class='legend-bar' style='width:{Pct(_completed)}%;background:#4ade80'></div>
      </div>
      <span class='legend-count'>{_completed}</span>
    </div>
    <div class='legend-row'>
      <span class='legend-dot' style='background:#60a5fa'></span>
      <span class='legend-label'>Repairing</span>
      <div class='legend-bar-wrap'>
        <div class='legend-bar' style='width:{Pct(_repairing)}%;background:#60a5fa'></div>
      </div>
      <span class='legend-count'>{_repairing}</span>
    </div>
    <div class='legend-row'>
      <span class='legend-dot' style='background:#fbbf24'></span>
      <span class='legend-label'>Pending</span>
      <div class='legend-bar-wrap'>
        <div class='legend-bar' style='width:{Pct(_pending)}%;background:#fbbf24'></div>
      </div>
      <span class='legend-count'>{_pending}</span>
    </div>
    <div class='legend-row'>
      <span class='legend-dot' style='background:#f87171'></span>
      <span class='legend-label'>Diagnosing</span>
      <div class='legend-bar-wrap'>
        <div class='legend-bar' style='width:{Pct(_diagnosing)}%;background:#f87171'></div>
      </div>
      <span class='legend-count'>{_diagnosing}</span>
    </div>
  </div>
 
  <div class='total-row'>
    Total Repairs <span>{_total}</span>
  </div>
 
  <div class='timestamp'>Generated: {today}</div>
</div>
 
<script>
  var ctx = document.getElementById('chart').getContext('2d');
  new Chart(ctx, {{
    type: 'doughnut',
    data: {{
      labels: ['Completed', 'Repairing', 'Pending', 'Diagnosing'],
      datasets: [{{
        data: [{_completed}, {_repairing}, {_pending}, {_diagnosing}],
        backgroundColor: [
          'rgba(74,  222, 128, 0.88)',
          'rgba(96,  165, 250, 0.88)',
          'rgba(251, 191,  36, 0.88)',
          'rgba(248, 113, 113, 0.88)'
        ],
        borderColor: '#ffffff',
        borderWidth: 3,
        hoverBackgroundColor: [
          'rgba(34,  197,  94, 1)',
          'rgba(59,  130, 246, 1)',
          'rgba(245, 158,  11, 1)',
          'rgba(239,  68,  68, 1)'
        ]
      }}]
    }},
    options: {{
      responsive: true,
      maintainAspectRatio: false,
      cutoutPercentage: 55,
      legend: {{ display: false }},
      tooltips: {{
        callbacks: {{
          label: function(item, data) {{
            var total = data.datasets[0].data.reduce(function(a,b){{return a+b;}},0);
            var val   = data.datasets[0].data[item.index];
            var pct   = total > 0 ? ((val/total)*100).toFixed(1) : 0;
            return '  ' + data.labels[item.index] + ': ' + val + ' (' + pct + '%)';
          }}
        }},
        backgroundColor: '#1e293b',
        titleFontColor:  '#94a3b8',
        bodyFontColor:   '#f1f5f9'
      }},
      animation: {{ duration: 0 }}
    }}
  }});
</script>
</body>
</html>";

            // ── WebBrowser: just set DocumentText (no async needed) ───────────
            webBrowser1.DocumentText = html;

        }

        // ─────────────────────────────────────────────────────────────────────
        //  HELPER — bar width percentage
        // ─────────────────────────────────────────────────────────────────────
        private double Pct(int count)
            => _total > 0 ? Math.Round((count / (double)_total) * 100, 1) : 0;

        // ─────────────────────────────────────────────────────────────────────
        //  EXPORT CSV BUTTON
        //  Wire to: btnExportCSV.Click += btnExportCSV_Click
        // ─────────────────────────────────────────────────────────────────────
        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "CSV Files (*.csv)|*.csv";
                dlg.FileName = $"RepairReport_{DateTime.Now:yyyyMMdd_HHmm}.csv";

                if (dlg.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var repairs = DatabaseService.GetRepairs();

                    if (repairs == null)
                    {
                        MessageBox.Show("No repair data available to export.",
                                        "Export CSV",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        return;
                    }

                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine("RepairID,CustomerID,DeviceID,TechnicianID,Status,Issue,Cost,DateReceived,DateUpdated");

                    foreach (var rep in repairs)
                    {
                        // BUG FIX: Proper CSV field escaping (escape quotes by doubling them)
                        string issueField = rep.Issue ?? "";
                        if (issueField.Contains(",") || issueField.Contains("\"") || issueField.Contains("\n"))
                        {
                            issueField = "\"" + issueField.Replace("\"", "\"\"") + "\"";
                        }

                        sb.AppendLine(string.Join(",",
                            rep.RepairID,
                            rep.CustomerID,
                            rep.DeviceID,
                            rep.TechnicianID,
                            rep.Status,
                            issueField,
                            rep.Cost.ToString("F2"),
                            rep.DateReceived.ToString("yyyy-MM-dd"),
                            rep.DateUpdated?.ToString("yyyy-MM-dd") ?? ""
                        ));
                    }

                    System.IO.File.WriteAllText(dlg.FileName, sb.ToString());

                    MessageBox.Show("Report exported successfully!",
                                    "Export CSV",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export failed:\n" + ex.Message,
                                    "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }
        // ── Stubs for Designer.cs event wiring ──────────────────────────
        // These match what ReportsForm.Designer.cs is looking for.

        private void ReportsForm_Load_1(object sender, EventArgs e)
        {
            // Already handled by ReportsForm_Load above — leave this empty
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            // Leave empty — panel paint handled automatically
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            // Old chart click — no longer needed
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // Route to the CSV export method
            btnExportCSV_Click(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Start();
            // Refresh chart data on timer if needed
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            LoadCounts();
            UpdateSummaryCards();
            RenderChart();
        }

        private void lblCompleted_Click(object sender, EventArgs e)
        {

        }
        

        private void lblDateTime_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }

}