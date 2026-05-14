using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RepairTrackerSystem.Core;

namespace RepairTrackerSystem
{
    public partial class CustomersForm : Form
    {
        public CustomersForm()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timerClock.Start();
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            btnAddCustomer.Click += BtnAddCustomer_Click;

            txtSearch1.TextChanged += TxtSearch1_TextChanged;
            txtSearch1.Text = "Search customers...";
            txtSearch1.ForeColor = Color.Gray;
            txtSearch1.Enter += TxtSearch1_Enter;
            txtSearch1.Leave += TxtSearch1_Leave;

            this.BackColor = Color.White;
            label11.Text = "CUSTOMERS";

            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(36, 86, 139);
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvCustomers.EnableHeadersVisualStyles = false;
            dgvCustomers.RowTemplate.Height = 28;
            dgvCustomers.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;

            LoadCustomers();
        }

        private void LoadCustomers(string filter = null)
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

            var customers = DatabaseService.GetCustomers();

            // ✅ Search across ALL fields: ID, Name, Contact, Address
            if (!string.IsNullOrWhiteSpace(filter))
            {
                customers = customers.Where(c =>
                    c.ID.ToLower().Contains(filter.ToLower()) ||
                    c.Name.ToLower().Contains(filter.ToLower()) ||
                    c.Contact.ToLower().Contains(filter.ToLower()) ||
                    c.Address.ToLower().Contains(filter.ToLower())
                ).ToList();
            }

            foreach (var c in customers)
                dgvCustomers.Rows.Add(c.ID, c.Name, c.Contact, c.Address);
        }

        private void TxtSearch1_Enter(object sender, EventArgs e)
        {
            if (txtSearch1.Text == "Search customers...")
            {
                txtSearch1.Text = "";
                txtSearch1.ForeColor = Color.Black;
            }
        }

        private void TxtSearch1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch1.Text))
            {
                txtSearch1.Text = "Search customers...";
                txtSearch1.ForeColor = Color.Gray;
            }
        }

        private void TxtSearch1_TextChanged(object sender, EventArgs e)
        {
            string filter = txtSearch1.Text.Trim();
            if (filter == "Search customers...") filter = "";

            LoadCustomers(string.IsNullOrEmpty(filter) ? null : filter);
        }

        private void BtnAddCustomer_Click(object sender, EventArgs e)
        {
            ShowCustomerDialog();
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var grid = sender as DataGridView;
            if (grid == null) return;

            var id = grid.Rows[e.RowIndex].Cells[0].Value?.ToString();
            var customer = DatabaseService.GetCustomer(id);

            if (grid.Columns[e.ColumnIndex].Name == "colEdit")
            {
                if (customer != null)
                    ShowCustomerDialog(customer);
            }
            else if (grid.Columns[e.ColumnIndex].Name == "colDelete")
            {
                if (customer != null)
                {
                    var ok = MessageBox.Show($"Delete {customer.Name}?", "Confirm",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (ok == DialogResult.Yes)
                    {
                        DatabaseService.DeleteCustomer(customer.ID);
                        LoadCustomers();
                    }
                }
            }
        }

        private void ShowCustomerDialog(Customer existing = null)
        {
            if (existing == null)
            {
                // ADD new customer
                var form = new Form();
                form.Text = "Add Customer";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.Size = new Size(360, 260);

                var lblName = new Label { Text = "Name", Location = new Point(10, 20) };
                var txtName = new TextBox { Location = new Point(10, 40), Width = 320 };
                var lblContact = new Label { Text = "Contact", Location = new Point(10, 75) };
                var txtContact = new TextBox { Location = new Point(10, 95), Width = 320 };
                var lblAddress = new Label { Text = "Address", Location = new Point(10, 130) };
                var txtAddress = new TextBox { Location = new Point(10, 150), Width = 320 };
                var btnSave = new Button { Text = "Save", Location = new Point(170, 190), Width = 75 };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(255, 190), Width = 75 };

                btnSave.Click += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Name is required", "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DatabaseService.AddCustomer(new Customer
                    {
                        Name = txtName.Text.Trim(),
                        Contact = txtContact.Text.Trim(),
                        Address = txtAddress.Text.Trim()
                    });

                    form.DialogResult = DialogResult.OK;
                    form.Close();
                    LoadCustomers();
                };

                btnCancel.Click += (s, e) => form.Close();

                form.Controls.AddRange(new System.Windows.Forms.Control[]
                {
                    lblName, txtName, lblContact, txtContact,
                    lblAddress, txtAddress, btnSave, btnCancel
                });

                form.ShowDialog();
                return;
            }

            // EDIT — ask which field to edit
            var pickForm = new Form();
            pickForm.Text = "What do you want to edit?";
            pickForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            pickForm.StartPosition = FormStartPosition.CenterParent;
            pickForm.Size = new Size(280, 200);
            pickForm.MaximizeBox = false;
            pickForm.MinimizeBox = false;

            var btnEditName = new Button
            {
                Text = "Name",
                Location = new Point(70, 20),
                Width = 130,
                Height = 35
            };
            var btnEditContact = new Button
            {
                Text = "Contact",
                Location = new Point(70, 65),
                Width = 130,
                Height = 35
            };
            var btnEditAddress = new Button
            {
                Text = "Address",
                Location = new Point(70, 110),
                Width = 130,
                Height = 35
            };

            btnEditName.Click += (s, e) =>
            {
                pickForm.Close();
                EditSingleField(existing, "Name");
            };

            btnEditContact.Click += (s, e) =>
            {
                pickForm.Close();
                EditSingleField(existing, "Contact");
            };

            btnEditAddress.Click += (s, e) =>
            {
                pickForm.Close();
                EditSingleField(existing, "Address");
            };

            pickForm.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                btnEditName, btnEditContact, btnEditAddress
            });

            pickForm.ShowDialog(this);
        }

        private void EditSingleField(Customer existing, string field)
        {
            var fresh = DatabaseService.GetCustomer(existing.ID);
            if (fresh == null) return;

            var form = new Form();
            form.Text = $"Edit {field}";
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterParent;
            form.Size = new Size(360, 150);
            form.MaximizeBox = false;
            form.MinimizeBox = false;

            var lbl = new Label
            {
                Text = field,
                Location = new Point(10, 15)
            };

            var txt = new TextBox
            {
                Location = new Point(10, 35),
                Width = 320,
                Text = field == "Name" ? fresh.Name ?? "" :
                       field == "Contact" ? fresh.Contact ?? "" :
                                            fresh.Address ?? ""
            };

            var btnSave = new Button { Text = "Save", Location = new Point(170, 75), Width = 75 };
            var btnCancel = new Button { Text = "Cancel", Location = new Point(255, 75), Width = 75 };

            btnSave.Click += (s, e) =>
            {
                if (field == "Name" && string.IsNullOrWhiteSpace(txt.Text))
                {
                    MessageBox.Show("Name cannot be empty.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (field == "Name") fresh.Name = txt.Text.Trim();
                if (field == "Contact") fresh.Contact = txt.Text.Trim();
                if (field == "Address") fresh.Address = txt.Text.Trim();

                DatabaseService.UpdateCustomer(fresh);

                form.DialogResult = DialogResult.OK;
                form.Close();
                LoadCustomers();
            };

            btnCancel.Click += (s, e) => form.Close();

            form.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lbl, txt, btnSave, btnCancel
            });

            form.ShowDialog(this);
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        }

        private void label11_Click(object sender, EventArgs e) { }
        private void panelCustomers_Paint(object sender, PaintEventArgs e) { }
        private void panelHeader_Paint(object sender, PaintEventArgs e) { }
        private void panelCustomers_Paint_1(object sender, PaintEventArgs e) { }
        private void lblDateTime_Click(object sender, EventArgs e) { }
        private void txtSearch_TextChanged(object sender, EventArgs e) { }
    }
}