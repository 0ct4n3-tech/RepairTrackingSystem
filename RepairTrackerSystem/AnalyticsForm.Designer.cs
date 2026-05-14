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
            this.components = new System.ComponentModel.Container();
            this.btnAllReports = new System.Windows.Forms.Button();
            this.btnRepairsPerDay = new System.Windows.Forms.Button();
            this.btnIssueFrequency = new System.Windows.Forms.Button();
            this.btnTechnicianPerf = new System.Windows.Forms.Button();
            this.btnCostAnalysis = new System.Windows.Forms.Button();
            this.btnTrendAnalysis = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAllReports
            // 
            this.btnAllReports.BackColor = System.Drawing.Color.LightBlue;
            this.btnAllReports.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.btnAllReports.Location = new System.Drawing.Point(180, 120);
            this.btnAllReports.Name = "btnAllReports";
            this.btnAllReports.Size = new System.Drawing.Size(260, 57);
            this.btnAllReports.TabIndex = 1;
            this.btnAllReports.Text = "📊 Generate All Reports";
            this.btnAllReports.UseVisualStyleBackColor = false;
            this.btnAllReports.Click += new System.EventHandler(this.btnAllReports_Click);
            // 
            // btnRepairsPerDay
            // 
            this.btnRepairsPerDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.btnRepairsPerDay.Location = new System.Drawing.Point(42, 183);
            this.btnRepairsPerDay.Name = "btnRepairsPerDay";
            this.btnRepairsPerDay.Size = new System.Drawing.Size(260, 47);
            this.btnRepairsPerDay.TabIndex = 2;
            this.btnRepairsPerDay.Text = "📈 Repairs Per Day";
            this.btnRepairsPerDay.Click += new System.EventHandler(this.btnRepairsPerDay_Click);
            // 
            // btnIssueFrequency
            // 
            this.btnIssueFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIssueFrequency.Location = new System.Drawing.Point(42, 233);
            this.btnIssueFrequency.Name = "btnIssueFrequency";
            this.btnIssueFrequency.Size = new System.Drawing.Size(260, 47);
            this.btnIssueFrequency.TabIndex = 3;
            this.btnIssueFrequency.Text = "🔧 Issue Frequency";
            this.btnIssueFrequency.Click += new System.EventHandler(this.btnIssueFrequency_Click);
            // 
            // btnTechnicianPerf
            // 
            this.btnTechnicianPerf.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTechnicianPerf.Location = new System.Drawing.Point(320, 183);
            this.btnTechnicianPerf.Name = "btnTechnicianPerf";
            this.btnTechnicianPerf.Size = new System.Drawing.Size(260, 47);
            this.btnTechnicianPerf.TabIndex = 4;
            this.btnTechnicianPerf.Text = "👨‍🔧 Technician Performance";
            this.btnTechnicianPerf.Click += new System.EventHandler(this.btnTechnicianPerf_Click);
            // 
            // btnCostAnalysis
            // 
            this.btnCostAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCostAnalysis.Location = new System.Drawing.Point(320, 233);
            this.btnCostAnalysis.Name = "btnCostAnalysis";
            this.btnCostAnalysis.Size = new System.Drawing.Size(260, 47);
            this.btnCostAnalysis.TabIndex = 5;
            this.btnCostAnalysis.Text = "💰 Cost Analysis";
            this.btnCostAnalysis.Click += new System.EventHandler(this.btnCostAnalysis_Click);
            // 
            // btnTrendAnalysis
            // 
            this.btnTrendAnalysis.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrendAnalysis.Location = new System.Drawing.Point(42, 283);
            this.btnTrendAnalysis.Name = "btnTrendAnalysis";
            this.btnTrendAnalysis.Size = new System.Drawing.Size(260, 47);
            this.btnTrendAnalysis.TabIndex = 6;
            this.btnTrendAnalysis.Text = "📉 Trend Analysis";
            this.btnTrendAnalysis.Click += new System.EventHandler(this.btnTrendAnalysis_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.BackColor = System.Drawing.Color.LightGreen;
            this.btnOpenFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFolder.Location = new System.Drawing.Point(320, 283);
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
            this.lblStatus.Location = new System.Drawing.Point(39, 65);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.lblDateTime);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(620, 40);
            this.panel2.TabIndex = 8;
            // 
            // lblDateTime
            // 
            this.lblDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.ForeColor = System.Drawing.Color.DimGray;
            this.lblDateTime.Location = new System.Drawing.Point(483, 15);
            this.lblDateTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(110, 13);
            this.lblDateTime.TabIndex = 6;
            this.lblDateTime.Text = "MM/dd/yyyy hh:mm tt";
            this.lblDateTime.Click += new System.EventHandler(this.lblDateTime_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label11.Location = new System.Drawing.Point(11, 5);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 31);
            this.label11.TabIndex = 3;
            this.label11.Text = "Analytics";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // AnalyticsForm
            // 
            this.ClientSize = new System.Drawing.Size(620, 449);
            this.Controls.Add(this.panel2);
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
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label label11;
    }
}