using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using RepairTrackerSystem.Core;

namespace RepairTrackerSystem
{
    public partial class AnalyticsForm : Form
    {
        public AnalyticsForm()
        {
            InitializeComponent();
            this.Load += AnalyticsForm_Load;
        }

        private void AnalyticsForm_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Tick += (s, ev) => lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            timer1.Start();
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            CheckPythonInstallation();
        }

        // ─────────────────────────────────────────────────────────────────
        // CHECK PYTHON INSTALLATION
        // ─────────────────────────────────────────────────────────────────
        private void CheckPythonInstallation()
        {
            if (!PythonAnalyticsService.IsPythonInstalled())
            {
                lblStatus.Text = "⚠ Python not installed. Analytics features unavailable.";
                lblStatus.ForeColor = System.Drawing.Color.Orange;
                DisableAllButtons();
            }
            else
            {
                lblStatus.Text = "✓ Python environment ready";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // DISABLE ALL BUTTONS IF PYTHON NOT AVAILABLE
        // ─────────────────────────────────────────────────────────────────
        private void DisableAllButtons()
        {
            btnAllReports.Enabled = false;
            btnRepairsPerDay.Enabled = false;
            btnIssueFrequency.Enabled = false;
            btnTechnicianPerf.Enabled = false;
            btnCostAnalysis.Enabled = false;
            btnTrendAnalysis.Enabled = false;
        }

        // Update all button handlers to match new signature
        private void btnAllReports_Click(object sender, EventArgs e)
        {
            ExecuteAnalyticsReport(() =>
            {
                bool success = PythonAnalyticsService.GenerateAllReports(out string output, out string error);
                return (success, output, error);
            }, "All Analytics Reports");
        }

        // ─────────────────────────────────────────────────────────────────
        // BUTTON: REPAIRS PER DAY
        // ─────────────────────────────────────────────────────────────────
        private void btnRepairsPerDay_Click(object sender, EventArgs e)
        {
            using (var dialog = new InputDialog("Enter number of days (default: 30):"))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (int.TryParse(dialog.InputValue, out int days) && days > 0)
                    {
                        ExecuteAnalyticsReport(() =>
                        {
                            bool success = PythonAnalyticsService.GenerateRepairsPerDay(days, out string output, out string error);
                            return (success, output, error);
                        }, $"Repairs Per Day ({days} days)");
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid number of days.",
                            "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // BUTTON: ISSUE FREQUENCY
        // ─────────────────────────────────────────────────────────────────
        private void btnIssueFrequency_Click(object sender, EventArgs e)
        {
            using (var dialog = new InputDialog("Enter top N issues to show (default: 10):"))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (int.TryParse(dialog.InputValue, out int topN) && topN > 0)
                    {
                        ExecuteAnalyticsReport(() =>
                        {
                            bool success = PythonAnalyticsService.GenerateIssueFrequency(topN, out string output, out string error);
                            return (success, output, error);
                        }, $"Top {topN} Issues");
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid number.",
                            "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // BUTTON: TECHNICIAN PERFORMANCE
        // ─────────────────────────────────────────────────────────────────
        private void btnTechnicianPerf_Click(object sender, EventArgs e)
        {
            ExecuteAnalyticsReport(() =>
            {
                bool success = PythonAnalyticsService.GenerateTechnicianPerformance(out string output, out string error);
                return (success, output, error);
            }, "Technician Performance");
        }

        // ─────────────────────────────────────────────────────────────────
        // BUTTON: COST ANALYSIS
        // ─────────────────────────────────────────────────────────────────
        private void btnCostAnalysis_Click(object sender, EventArgs e)
        {
            ExecuteAnalyticsReport(() =>
            {
                bool success = PythonAnalyticsService.GenerateCostAnalysis(out string output, out string error);
                return (success, output, error);
            }, "Cost Analysis");
        }

        // ─────────────────────────────────────────────────────────────────
        // BUTTON: TREND ANALYSIS
        // ─────────────────────────────────────────────────────────────────
        private void btnTrendAnalysis_Click(object sender, EventArgs e)
        {
            ExecuteAnalyticsReport(() =>
            {
                bool success = PythonAnalyticsService.GenerateTrendAnalysis(out string output, out string error);
                return (success, output, error);
            }, "Trend Analysis");
        }

        // ─────────────────────────────────────────────────────────────────
        // EXECUTE ANALYTICS REPORT (COMMON LOGIC)
        // ─────────────────────────────────────────────────────────────────
        private void ExecuteAnalyticsReport(Func<(bool success, string output, string error)> reportGenerator, string reportName)
        {
            try
            {
                lblStatus.Text = $"Generating {reportName}...";
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                this.Refresh();

                var (success, output, error) = reportGenerator();

                if (success)
                {
                    lblStatus.Text = $"✓ {reportName} generated successfully!";
                    lblStatus.ForeColor = System.Drawing.Color.Green;

                    // Ask user if they want to open the output folder
                    if (MessageBox.Show(
                        "Report generated successfully!\n\nOpen output folder?",
                        "Success",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        OpenOutputFolder();
                    }
                }
                else
                {
                    lblStatus.Text = $"✗ Failed to generate {reportName}";
                    lblStatus.ForeColor = System.Drawing.Color.Red;

                    // Display actual Python error
                    string errorMessage = string.IsNullOrEmpty(error)
                        ? "Unknown error occurred. Check that Python packages are installed."
                        : error;

                    MessageBox.Show(
                        $"Failed to generate {reportName}.\n\nError Details:\n{errorMessage}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "✗ Error occurred";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Error: {ex.Message}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // BUTTON: OPEN OUTPUT FOLDER
        // ─────────────────────────────────────────────────────────────────
        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            OpenOutputFolder();
        }

        private void OpenOutputFolder()
        {
            try
            {
                string outputPath = PythonAnalyticsService.GetOutputFolderPath();

                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = outputPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open folder: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void lblDateTime_Click(object sender, EventArgs e)
        {

        }
    }

    // ─────────────────────────────────────────────────────────────────────
    // INPUT DIALOG HELPER
    // ─────────────────────────────────────────────────────────────────────
    public class InputDialog : Form
    {
        private TextBox txtInput;
        public string InputValue { get; set; }

        public InputDialog(string prompt)
        {
            this.Text = "Enter Value";
            this.Width = 400;
            this.Height = 150;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var label = new Label
            {
                Text = prompt,
                Left = 20,
                Top = 20,
                Width = 350,
                AutoSize = true
            };

            txtInput = new TextBox
            {
                Left = 20,
                Top = 50,
                Width = 350,
                Text = ""
            };

            var btnOk = new Button
            {
                Text = "OK",
                Left = 200,
                Top = 90,
                Width = 80,
                DialogResult = DialogResult.OK
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                Left = 290,
                Top = 90,
                Width = 80,
                DialogResult = DialogResult.Cancel
            };

            this.Controls.Add(label);
            this.Controls.Add(txtInput);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            InputValue = txtInput.Text;
            base.OnClosing(e);
        }
    }
}