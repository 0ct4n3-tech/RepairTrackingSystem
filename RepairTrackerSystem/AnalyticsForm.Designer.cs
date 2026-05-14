namespace RepairTrackerSystem
{
    partial class AnalyticsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnAllReports;
        private System.Windows.Forms.Button btnRepairsPerDay;
        private System.Windows.Forms.Button btnIssueFrequency;
        private System.Windows.Forms.Button btnTechnicianPerf;
        private System.Windows.Forms.Button btnCostAnalysis;
        private System.Windows.Forms.Button btnTrendAnalysis;
        private System.Windows.Forms.Button btnOpenFolder;

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
            this.btnAllReports = new System.Windows.Forms.Button();
            this.btnRepairsPerDay = new System.Windows.Forms.Button();
            this.btnIssueFrequency = new System.Windows.Forms.Button();
            this.btnTechnicianPerf = new System.Windows.Forms.Button();
            this.btnCostAnalysis = new System.Windows.Forms.Button();
            this.btnTrendAnalysis = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAllReports
            // 
            this.btnAllReports.BackColor = System.Drawing.Color.LightBlue;
            this.btnAllReports.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.btnAllReports.Location = new System.Drawing.Point(180, 82);
            this.btnAllReports.Name = "btnAllReports";
            this.btnAllReports.Size = new System.Drawing.Size(260, 57);
            this.btnAllReports.TabIndex = 1;
            this.btnAllReports.Text = "📊 Generate All Reports";
            this.btnAllReports.UseVisualStyleBackColor = false;
            this.btnAllReports.Click += new System.EventHandler(this.btnAllReports_Click);
            // 
            // btnRepairsPerDay
            // 
            this.btnRepairsPerDay.Location = new System.Drawing.Point(42, 145);
            this.btnRepairsPerDay.Name = "btnRepairsPerDay";
            this.btnRepairsPerDay.Size = new System.Drawing.Size(260, 47);
            this.btnRepairsPerDay.TabIndex = 2;
            this.btnRepairsPerDay.Text = "📈 Repairs Per Day";
            this.btnRepairsPerDay.Click += new System.EventHandler(this.btnRepairsPerDay_Click);
            // 
            // btnIssueFrequency
            // 
            this.btnIssueFrequency.Location = new System.Drawing.Point(42, 195);
            this.btnIssueFrequency.Name = "btnIssueFrequency";
            this.btnIssueFrequency.Size = new System.Drawing.Size(260, 47);
            this.btnIssueFrequency.TabIndex = 3;
            this.btnIssueFrequency.Text = "🔧 Issue Frequency";
            this.btnIssueFrequency.Click += new System.EventHandler(this.btnIssueFrequency_Click);
            // 
            // btnTechnicianPerf
            // 
            this.btnTechnicianPerf.Location = new System.Drawing.Point(320, 145);
            this.btnTechnicianPerf.Name = "btnTechnicianPerf";
            this.btnTechnicianPerf.Size = new System.Drawing.Size(260, 47);
            this.btnTechnicianPerf.TabIndex = 4;
            this.btnTechnicianPerf.Text = "👨‍🔧 Technician Performance";
            this.btnTechnicianPerf.Click += new System.EventHandler(this.btnTechnicianPerf_Click);
            // 
            // btnCostAnalysis
            // 
            this.btnCostAnalysis.Location = new System.Drawing.Point(320, 195);
            this.btnCostAnalysis.Name = "btnCostAnalysis";
            this.btnCostAnalysis.Size = new System.Drawing.Size(260, 47);
            this.btnCostAnalysis.TabIndex = 5;
            this.btnCostAnalysis.Text = "💰 Cost Analysis";
            this.btnCostAnalysis.Click += new System.EventHandler(this.btnCostAnalysis_Click);
            // 
            // btnTrendAnalysis
            // 
            this.btnTrendAnalysis.Location = new System.Drawing.Point(42, 245);
            this.btnTrendAnalysis.Name = "btnTrendAnalysis";
            this.btnTrendAnalysis.Size = new System.Drawing.Size(260, 47);
            this.btnTrendAnalysis.TabIndex = 6;
            this.btnTrendAnalysis.Text = "📉 Trend Analysis";
            this.btnTrendAnalysis.Click += new System.EventHandler(this.btnTrendAnalysis_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.BackColor = System.Drawing.Color.LightGreen;
            this.btnOpenFolder.Location = new System.Drawing.Point(320, 245);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(260, 47);
            this.btnOpenFolder.TabIndex = 7;
            this.btnOpenFolder.Text = "📁 Open Output Folder";
            this.btnOpenFolder.UseVisualStyleBackColor = false;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblStatus.Location = new System.Drawing.Point(12, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(64, 17);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Analytics";
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // AnalyticsForm
            // 
            this.ClientSize = new System.Drawing.Size(620, 449);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnAllReports);
            this.Controls.Add(this.btnRepairsPerDay);
            this.Controls.Add(this.btnIssueFrequency);
            this.Controls.Add(this.btnTechnicianPerf);
            this.Controls.Add(this.btnCostAnalysis);
            this.Controls.Add(this.btnTrendAnalysis);
            this.Controls.Add(this.btnOpenFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "AnalyticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Analytics";
            this.Load += new System.EventHandler(this.AnalyticsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblStatus;
    }
}