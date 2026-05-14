using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RepairTrackerSystem.Core;

namespace RepairTrackerSystem
{
    public partial class RepairsFormcs : Form
    {
        public RepairsFormcs()
        {
            InitializeComponent();
        }

        private void RepairsFormcs_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            cbStatus.SelectedIndexChanged += CbStatus_SelectedIndexChanged;
            txtSearchRepair.TextChanged += TxtSearchRepair_TextChanged;
            dgvCustomers.CellContentClick += dgvCustomers_CellContentClick;

            txtSearchRepair.Text = "Search repairs...";
            txtSearchRepair.ForeColor = Color.Gray;
            txtSearchRepair.Enter += TxtSearchRepair_Enter;
            txtSearchRepair.Leave += TxtSearchRepair_Leave;

            cbStatus.Items.Clear();
            cbStatus.Items.Add("All");
            cbStatus.Items.Add("Pending");
            cbStatus.Items.Add("Diagnosing");
            cbStatus.Items.Add("Repairing");
            cbStatus.Items.Add("Fixed");
            cbStatus.Items.Add("Released");
            cbStatus.SelectedIndex = 0;

            LoadRepairs();
        }

        private void LoadRepairs(string filter = null, string status = null)
        {
            dgvCustomers.Rows.Clear();

            if (!dgvCustomers.Columns.Contains("colEdit"))
            {
                dgvCustomers.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "colEdit",
                    HeaderText = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true
                });
            }

            if (!dgvCustomers.Columns.Contains("colDelete"))
            {
                dgvCustomers.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "colDelete",
                    HeaderText = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true
                });
            }

            var list = DatabaseService.GetRepairs(filter, status);

            foreach (var r in list)
            {
                var customer = DatabaseService.GetCustomer(r.CustomerID)?.Name ?? "";
                var device = DatabaseService.GetDevice(r.DeviceID)?.ToString() ?? "";
                var technician = DatabaseService.GetTechnician(r.TechnicianID)?.Name ?? "";

                dgvCustomers.Rows.Add(
                    r.RepairID,
                    customer,
                    device,
                    technician,
                    r.Status,
                    r.Cost
                );
            }
        }

        private void TxtSearchRepair_TextChanged(object sender, EventArgs e)
        {
            string filter = txtSearchRepair.Text.Trim();
            if (filter == "Search repairs...") filter = "";

            string status = cbStatus.SelectedItem?.ToString();
            if (status == "All") status = null;

            LoadRepairs(filter, status);
        }

        private void CbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = txtSearchRepair.Text.Trim();
            if (filter == "Search repairs...") filter = "";

            string status = cbStatus.SelectedItem?.ToString();
            if (status == "All") status = null;

            LoadRepairs(filter, status);
        }

        private void TxtSearchRepair_Enter(object sender, EventArgs e)
        {
            if (txtSearchRepair.Text == "Search repairs...")
            {
                txtSearchRepair.Text = "";
                txtSearchRepair.ForeColor = Color.Black;
            }
        }

        private void TxtSearchRepair_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchRepair.Text))
            {
                txtSearchRepair.Text = "Search repairs...";
                txtSearchRepair.ForeColor = Color.Gray;
            }
        }

        private void btnAddRepair_Click(object sender, EventArgs e)
        {
            var f = new AddRepairForm();
            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadRepairs();
                Form1.Instance?.RefreshDashboard();
            }
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var grid = sender as DataGridView;
            string id = grid.Rows[e.RowIndex].Cells[0].Value?.ToString();
            var repair = DatabaseService.GetRepair(id);
            if (repair == null) return;

            if (grid.Columns[e.ColumnIndex].Name == "colEdit")
            {
                ShowEditPickerDialog(repair);
            }
            else if (grid.Columns[e.ColumnIndex].Name == "colDelete")
            {
                if (MessageBox.Show($"Delete repair {repair.RepairID}?", "Confirm",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DatabaseService.DeleteRepair(repair.RepairID);
                    LoadRepairs();
                    Form1.Instance?.RefreshDashboard();
                }
            }
        }

        private void ShowEditPickerDialog(Repair repair)
        {
            var pickForm = new Form
            {
                Text = "What do you want to edit?",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(280, 230), // ✅ smaller since one less button
                MaximizeBox = false,
                MinimizeBox = false
            };

            var btnStatus = new Button
            {
                Text = "Status",
                Location = new Point(70, 20),
                Width = 130,
                Height = 35
            };
            var btnCost = new Button
            {
                Text = "Cost",
                Location = new Point(70, 65),
                Width = 130,
                Height = 35
            };
            var btnTechnician = new Button
            {
                Text = "Technician",
                Location = new Point(70, 110),
                Width = 130,
                Height = 35
            };

            btnStatus.Click += (s, e) => { pickForm.Close(); EditRepairStatus(repair); };
            btnCost.Click += (s, e) => { pickForm.Close(); EditRepairSingleField(repair, "Cost"); };
            btnTechnician.Click += (s, e) => { pickForm.Close(); EditRepairTechnician(repair); };

            pickForm.Controls.AddRange(new Control[]
            {
        btnStatus, btnCost, btnTechnician
            });

            pickForm.ShowDialog(this);
        }

        private void EditRepairStatus(Repair repair)
        {
            using (var f = new Form())
            {
                f.Text = "Edit Status";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.Size = new Size(360, 180);
                f.MaximizeBox = false;

                var lbl = new Label { Text = "Status", Location = new Point(10, 15) };
                var cb = new ComboBox
                {
                    Location = new Point(10, 35),
                    Width = 320,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                cb.Items.AddRange(new object[]
                {
                    "Pending", "Repairing", "Completed"
                });
                cb.SelectedItem = repair.Status;

                var btnSave = new Button { Text = "Save", Location = new Point(170, 100), Width = 75 };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(255, 100), Width = 75 };

                btnSave.Click += (s, ev) =>
                {
                    repair.Status = cb.SelectedItem?.ToString();
                    repair.DateUpdated = DateTime.Now;
                    DatabaseService.UpdateRepair(repair);
                    f.DialogResult = DialogResult.OK;
                    f.Close();
                };
                btnCancel.Click += (s, ev) => f.Close();

                f.Controls.AddRange(new Control[] { lbl, cb, btnSave, btnCancel });

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    LoadRepairs();
                    Form1.Instance?.RefreshDashboard();
                }
            }
        }

        private void EditRepairSingleField(Repair repair, string field)
        {
            using (var f = new Form())
            {
                f.Text = $"Edit {field}";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.Size = new Size(360, 150);
                f.MaximizeBox = false;

                var lbl = new Label { Text = field, Location = new Point(10, 15) };
                var txt = new TextBox
                {
                    Location = new Point(10, 35),
                    Width = 320,
                    // ✅ Pre-fill with current value
                    Text = field == "Issue" ? repair.Issue ?? "" : repair.Cost.ToString()
                };

                var btnSave = new Button { Text = "Save", Location = new Point(170, 75), Width = 75 };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(255, 75), Width = 75 };

                btnSave.Click += (s, ev) =>
                {
                    if (field == "Cost")
                    {
                        string raw = txt.Text.Trim().Replace(",", "");

                        if (!decimal.TryParse(raw, System.Globalization.NumberStyles.Any,
                                              System.Globalization.CultureInfo.InvariantCulture, out decimal cost))
                        {
                            MessageBox.Show("Please enter a valid number for Cost.", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        repair.Cost = cost;
                    }

                    repair.DateUpdated = DateTime.Now;
                    DatabaseService.UpdateRepair(repair);
                    f.DialogResult = DialogResult.OK;
                    f.Close();
                };
                btnCancel.Click += (s, ev) => f.Close();

                f.Controls.AddRange(new Control[] { lbl, txt, btnSave, btnCancel });

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    LoadRepairs();
                    Form1.Instance?.RefreshDashboard();
                }
            }
        }

        private void EditRepairTechnician(Repair repair)
        {
            using (var f = new Form())
            {
                f.Text = "Edit Technician";
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.Size = new Size(360, 150);
                f.MaximizeBox = false;

                var lbl = new Label { Text = "Technician", Location = new Point(10, 15) };
                var cb = new ComboBox
                {
                    Location = new Point(10, 35),
                    Width = 320,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                var technicians = DatabaseService.GetTechnicians();
                foreach (var t in technicians)
                    cb.Items.Add(t);

                // ✅ Pre-select current technician
                cb.SelectedItem = technicians.FirstOrDefault(t => t.ID == repair.TechnicianID);

                var btnSave = new Button { Text = "Save", Location = new Point(170, 75), Width = 75 };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(255, 75), Width = 75 };

                btnSave.Click += (s, ev) =>
                {
                    if (cb.SelectedItem is Technician selected)
                    {
                        repair.TechnicianID = selected.ID;
                        repair.DateUpdated = DateTime.Now;
                        DatabaseService.UpdateRepair(repair);
                        f.DialogResult = DialogResult.OK;
                        f.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please select a technician.", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                btnCancel.Click += (s, ev) => f.Close();

                f.Controls.AddRange(new Control[] { lbl, cb, btnSave, btnCancel });

                if (f.ShowDialog(this) == DialogResult.OK)
                {
                    LoadRepairs();
                    Form1.Instance?.RefreshDashboard();
                }
            }
        }

        private void UpdateStatus(string repairId, string newStatus)
        {
            var r = DatabaseService.GetRepair(repairId);
            if (r == null) return;
            r.Status = newStatus;
            r.DateUpdated = DateTime.Now;
            DatabaseService.UpdateRepair(r);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        }

        private void panelRepairs_Paint(object sender, PaintEventArgs e) { }
        private void cbStatus_SelectedIndexChanged_1(object sender, EventArgs e) { }

        private void lblDateTime_Click(object sender, EventArgs e)
        {

        }
    }
}