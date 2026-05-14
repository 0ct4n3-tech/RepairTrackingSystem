using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RepairTrackerSystem.Core;


namespace RepairTrackerSystem
{
    public partial class TechniciansForm : Form
    {
        public TechniciansForm()
        {
            InitializeComponent();
        }

        private void TechniciansForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            // wire events
            dgvTech.CellDoubleClick += DgvTech_CellDoubleClick;
            dgvTech.KeyDown += DgvTech_KeyDown;

            // Placeholder setup
            searchTech.Text = "Search technicians...";
            searchTech.ForeColor = Color.Gray;
            searchTech.TextChanged += SearchTech_TextChanged;
            searchTech.Enter += SearchTech_Enter;
            searchTech.Leave += SearchTech_Leave;

            LoadTechnicians();
        }

        private void LoadTechnicians(string filter = null)
        {
            dgvTech.Rows.Clear();

            var technicians = DatabaseService.GetTechnicians();

            // ✅ Search across ALL fields: ID, Name, Specialty, Contact
            if (!string.IsNullOrWhiteSpace(filter))
            {
                technicians = technicians.Where(t =>
                    t.ID.ToLower().Contains(filter.ToLower()) ||
                    t.Name.ToLower().Contains(filter.ToLower()) ||
                    t.Specialty.ToLower().Contains(filter.ToLower()) ||
                    t.Contact.ToLower().Contains(filter.ToLower())
                ).ToList();
            }

            foreach (var t in technicians)
            {
                dgvTech.Rows.Add(t.ID, t.Name, t.Specialty, t.Contact);
            }
        }

        private void DgvTech_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var id = dgvTech.Rows[e.RowIndex].Cells[0].Value?.ToString();
            var tech = DatabaseService.GetTechnician(id);
            if (tech == null) return;

            using (var f = new Form())
            {
                f.Text = "Edit Technician";
                f.Size = new Size(360, 230);
                var lblName = new Label { Text = "Name", Location = new Point(10, 20) };
                var txtName = new TextBox { Location = new Point(10, 40), Width = 320, Text = tech.Name };
                var lblSpec = new Label { Text = "Specialty", Location = new Point(10, 75) };
                var txtSpec = new TextBox { Location = new Point(10, 95), Width = 320, Text = tech.Specialty };
                var lblContact = new Label { Text = "Contact", Location = new Point(10, 130) };
                var txtContact = new TextBox { Location = new Point(10, 150), Width = 320, Text = tech.Contact };
                var btnSave = new Button { Text = "Save", Location = new Point(170, 180), Width = 75 };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(255, 180), Width = 75 };
                btnSave.Click += (s, ev) => { tech.Name = txtName.Text.Trim(); tech.Specialty = txtSpec.Text.Trim(); tech.Contact = txtContact.Text.Trim(); DatabaseService.UpdateTechnician(tech); f.DialogResult = DialogResult.OK; f.Close(); };
                btnCancel.Click += (s, ev) => f.Close();
                f.Controls.AddRange(new Control[] { lblName, txtName, lblSpec, txtSpec, lblContact, txtContact, btnSave, btnCancel });
                if (f.ShowDialog(this) == DialogResult.OK)
                    LoadTechnicians();
            }
        }

        private void DgvTech_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dgvTech.SelectedRows.Count > 0)
            {
                var id = dgvTech.SelectedRows[0].Cells[0].Value?.ToString();
                var tech = DatabaseService.GetTechnician(id);
                if (tech != null)
                {
                    var ok = MessageBox.Show($"Delete technician {tech.Name}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ok == DialogResult.Yes)
                    {
                        DatabaseService.DeleteTechnician(tech.ID);
                        LoadTechnicians();
                    }
                }
            }
        }

        private void label11_Click(object sender, EventArgs e) { }
        private void dgvTech_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnAddTech_Click(object sender, EventArgs e)
        {
            ShowTechnicianDialog();
        }

        private void ShowTechnicianDialog(Technician existing = null)
        {
            var form = new Form();
            form.Text = existing == null ? "Add Technician" : "Edit Technician";
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ClientSize = new Size(360, 330);
            form.MaximizeBox = false;
            form.MinimizeBox = false;

            int left = 15;
            int width = 310;
            int top = 15;
            int space = 55;

            var lblID = new Label { Text = "Tech ID", Left = left, Top = top };
            var txtID = new TextBox { Left = left, Top = top + 20, Width = width };

            top += space;
            var lblName = new Label { Text = "Name", Left = left, Top = top };
            var txtName = new TextBox { Left = left, Top = top + 20, Width = width };

            top += space;
            var lblSpec = new Label { Text = "Specialty", Left = left, Top = top };
            var cmbSpec = new ComboBox
            {
                Left = left,
                Top = top + 20,
                Width = width,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSpec.Items.AddRange(new string[] { "Hardware", "Software" });

            top += space;
            var lblContact = new Label { Text = "Contact", Left = left, Top = top };
            var txtContact = new TextBox { Left = left, Top = top + 20, Width = width };

            var btnSave = new Button { Text = "Save", Width = 80, Left = 170, Top = 260 };
            var btnCancel = new Button { Text = "Cancel", Width = 80, Left = 260, Top = 260 };

            if (existing != null)
            {
                txtID.Text = existing.ID;
                txtID.Enabled = false;
                txtName.Text = existing.Name;
                cmbSpec.Text = existing.Specialty;
                txtContact.Text = existing.Contact;
            }

            btnSave.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtID.Text) ||
                    string.IsNullOrWhiteSpace(txtName.Text) ||
                    cmbSpec.SelectedIndex < 0)
                {
                    MessageBox.Show("Please complete all required fields.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (existing == null)
                {
                    var tech = new Technician
                    {
                        ID = txtID.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        Specialty = cmbSpec.Text,
                        Contact = txtContact.Text.Trim()
                    };
                    DatabaseService.AddTechnician(tech);
                }
                else
                {
                    existing.Name = txtName.Text.Trim();
                    existing.Specialty = cmbSpec.Text;
                    existing.Contact = txtContact.Text.Trim();
                    DatabaseService.UpdateTechnician(existing);
                }

                form.DialogResult = DialogResult.OK;
                form.Close();
                LoadTechnicians();
            };

            btnCancel.Click += (s, e) => form.Close();

            form.Controls.AddRange(new Control[]
            {
                lblID, txtID, lblName, txtName,
                lblSpec, cmbSpec, lblContact, txtContact,
                btnSave, btnCancel
            });

            form.ShowDialog(this);
        }

            private void SearchTech_TextChanged(object sender, EventArgs e)
        {
            string filter = searchTech.Text.Trim();
            if (filter == "Search technicians...") filter = "";

            LoadTechnicians(string.IsNullOrEmpty(filter) ? null : filter);
        }

        private void SearchTech_Enter(object sender, EventArgs e)
        {
            if (searchTech.Text == "Search technicians...")
            {
                searchTech.Text = "";
                searchTech.ForeColor = Color.Black;
            }
        }

        private void SearchTech_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTech.Text))
            {
                searchTech.Text = "Search technicians...";
                searchTech.ForeColor = Color.Gray;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e) { }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        }
    }
}
