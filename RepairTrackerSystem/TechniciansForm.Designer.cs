namespace RepairTrackerSystem
{
    partial class TechniciansForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvTech = new System.Windows.Forms.DataGridView();
            this.TechID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TechName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Specialty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Contact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddTech = new System.Windows.Forms.Button();
            this.searchTech = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTech)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTech
            // 
            this.dgvTech.AllowUserToAddRows = false;
            this.dgvTech.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvTech.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTech.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTech.BackgroundColor = System.Drawing.Color.White;
            this.dgvTech.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTech.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTech.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTech.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TechID,
            this.TechName,
            this.Specialty,
            this.Contact});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTech.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTech.EnableHeadersVisualStyles = false;
            this.dgvTech.GridColor = System.Drawing.Color.LightGray;
            this.dgvTech.Location = new System.Drawing.Point(10, 103);
            this.dgvTech.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvTech.Name = "dgvTech";
            this.dgvTech.ReadOnly = true;
            this.dgvTech.RowHeadersVisible = false;
            this.dgvTech.RowHeadersWidth = 51;
            this.dgvTech.RowTemplate.Height = 24;
            this.dgvTech.Size = new System.Drawing.Size(562, 244);
            this.dgvTech.TabIndex = 13;
            this.dgvTech.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTech_CellContentClick);
            // 
            // TechID
            // 
            this.TechID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TechID.HeaderText = "Tech ID";
            this.TechID.MinimumWidth = 6;
            this.TechID.Name = "TechID";
            this.TechID.ReadOnly = true;
            // 
            // TechName
            // 
            this.TechName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TechName.HeaderText = "Name";
            this.TechName.MinimumWidth = 6;
            this.TechName.Name = "TechName";
            this.TechName.ReadOnly = true;
            // 
            // Specialty
            // 
            this.Specialty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Specialty.HeaderText = "Specialty (Hardware/Software)";
            this.Specialty.MinimumWidth = 6;
            this.Specialty.Name = "Specialty";
            this.Specialty.ReadOnly = true;
            // 
            // Contact
            // 
            this.Contact.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Contact.HeaderText = "Contact";
            this.Contact.MinimumWidth = 6;
            this.Contact.Name = "Contact";
            this.Contact.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, -1);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(620, 40);
            this.panel1.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblDateTime);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(-1, -1);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(620, 40);
            this.panel2.TabIndex = 16;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // lblDateTime
            // 
            this.lblDateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.ForeColor = System.Drawing.Color.DimGray;
            this.lblDateTime.Location = new System.Drawing.Point(473, 15);
            this.lblDateTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(110, 13);
            this.lblDateTime.TabIndex = 19;
            this.lblDateTime.Text = "MM/dd/yyyy hh:mm tt";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label2.Location = new System.Drawing.Point(6, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 31);
            this.label2.TabIndex = 3;
            this.label2.Text = "TECHNICIANS";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Mongolian Baiti", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "TECHNICIANS";
            // 
            // btnAddTech
            // 
            this.btnAddTech.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnAddTech.FlatAppearance.BorderSize = 0;
            this.btnAddTech.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTech.ForeColor = System.Drawing.Color.White;
            this.btnAddTech.Location = new System.Drawing.Point(466, 12);
            this.btnAddTech.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAddTech.Name = "btnAddTech";
            this.btnAddTech.Size = new System.Drawing.Size(118, 27);
            this.btnAddTech.TabIndex = 16;
            this.btnAddTech.Text = "+ New Technician";
            this.btnAddTech.UseVisualStyleBackColor = false;
            this.btnAddTech.Click += new System.EventHandler(this.btnAddTech_Click);
            // 
            // searchTech
            // 
            this.searchTech.ForeColor = System.Drawing.SystemColors.GrayText;
            this.searchTech.Location = new System.Drawing.Point(20, 56);
            this.searchTech.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.searchTech.Name = "searchTech";
            this.searchTech.Size = new System.Drawing.Size(196, 20);
            this.searchTech.TabIndex = 17;
            this.searchTech.Text = "Search Tech...";
            this.searchTech.TextChanged += new System.EventHandler(this.searchTech_TextChanged);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnAddTech);
            this.panel3.Location = new System.Drawing.Point(0, 39);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(620, 50);
            this.panel3.TabIndex = 18;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TechniciansForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 449);
            this.Controls.Add(this.searchTech);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvTech);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "TechniciansForm";
            this.Text = "TechniciansForm";
            this.Load += new System.EventHandler(this.TechniciansForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTech)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvTech;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TechName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Specialty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contact;
        private System.Windows.Forms.DataGridViewTextBoxColumn TechID;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddTech;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox searchTech;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Timer timer1;
    }
}