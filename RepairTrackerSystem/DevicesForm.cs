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
    public partial class DevicesForm : Form
    {
        public DevicesForm()
        {
            InitializeComponent();
        }

        private void DevicesForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            
            btnAddDevice.Click += BtnAddDevice_Click;
            dgvDevices.CellDoubleClick += DgvDevices_CellDoubleClick;
            dgvDevices.KeyDown += DgvDevices_KeyDown;

            // Add search box setup
            textBox1.Text = "Search devices...";
            textBox1.ForeColor = Color.Gray;
            textBox1.TextChanged += TextBox1_TextChanged;
            textBox1.Enter += TextBox1_Enter;
            textBox1.Leave += TextBox1_Leave;

            LoadDevices();
        }

        private void LoadDevices(string filter = null)
        {
            dgvDevices.Rows.Clear();
            var devices = DatabaseService.GetDevices();

            // ✅ Search across ALL fields: ID, Type, Brand, Model, Owner
            if (!string.IsNullOrWhiteSpace(filter))
            {
                devices = devices.Where(d =>
                    d.ID.ToLower().Contains(filter.ToLower()) ||
                    d.Type.ToLower().Contains(filter.ToLower()) ||
                    d.Brand.ToLower().Contains(filter.ToLower()) ||
                    d.Model.ToLower().Contains(filter.ToLower()) ||
                    DatabaseService.GetCustomer(d.CustomerID)?.Name?.ToLower().Contains(filter.ToLower()) == true
                ).ToList();
            }

            foreach (var d in devices)
            {
                var owner = DatabaseService.GetCustomer(d.CustomerID)?.Name ?? "";
                dgvDevices.Rows.Add(d.ID, d.Type, d.Brand, d.Model, owner);
            }
        }

        private void BtnAddDevice_Click(object sender, EventArgs e)
        {
            using (var f = new Form())
            {
                f.Text = "Add Device";
                f.Size = new Size(360, 340);
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;

                var lblType = new Label { Text = "Type", Location = new Point(15, 15), AutoSize = true };
                var txtType = new TextBox { Location = new Point(15, 38), Width = 315 };

                var lblBrand = new Label { Text = "Brand", Location = new Point(15, 75), AutoSize = true };
                var txtBrand = new TextBox { Location = new Point(15, 98), Width = 315 };

                var lblModel = new Label { Text = "Model", Location = new Point(15, 135), AutoSize = true };
                var txtModel = new TextBox { Location = new Point(15, 158), Width = 315 };

                var lblOwner = new Label { Text = "Owner", Location = new Point(15, 195), AutoSize = true };
                var cbOwner = new ComboBox { Location = new Point(15, 218), Width = 315, DropDownStyle = ComboBoxStyle.DropDownList };

                cbOwner.Items.AddRange(DatabaseService.GetCustomers().Cast<object>().ToArray());

                var btnSave = new Button { Text = "Save", Location = new Point(165, 260), Width = 75 };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(250, 260), Width = 75 };

                btnSave.Click += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtType.Text) || cbOwner.SelectedItem == null)
                    {
                        MessageBox.Show("Type and Owner are required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var d = new Device
                    {
                        Type = txtType.Text.Trim(),
                        Brand = txtBrand.Text.Trim(),
                        Model = txtModel.Text.Trim(),
                        CustomerID = ((Customer)cbOwner.SelectedItem).ID
                    };
                    DatabaseService.AddDevice(d);
                    f.DialogResult = DialogResult.OK;
                    f.Close();
                };

                btnCancel.Click += (s, ev) => f.Close();

                f.Controls.AddRange(new Control[] { lblType, txtType, lblBrand, txtBrand, lblModel, txtModel, lblOwner, cbOwner, btnSave, btnCancel });

                if (f.ShowDialog(this) == DialogResult.OK)
                    LoadDevices();
            }
        }

        private void DgvDevices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var id = dgvDevices.Rows[e.RowIndex].Cells[0].Value?.ToString();
            var dev = DatabaseService.GetDevice(id);
            if (dev == null) return;

            using (var f = new Form())
            {
                f.Text = "Edit Device";
                f.Size = new Size(360, 300);
                var lblType = new Label { Text = "Type", Location = new Point(10, 20) };
                var txtType = new TextBox { Location = new Point(10, 40), Width = 320, Text = dev.Type };
                var lblBrand = new Label { Text = "Brand", Location = new Point(10, 75) };
                var txtBrand = new TextBox { Location = new Point(10, 95), Width = 320, Text = dev.Brand };
                var lblModel = new Label { Text = "Model", Location = new Point(10, 130) };
                var txtModel = new TextBox { Location = new Point(10, 150), Width = 320, Text = dev.Model };
                var lblOwner = new Label { Text = "Owner", Location = new Point(10, 185) };
                var cbOwner = new ComboBox { Location = new Point(10, 205), Width = 320 };
                cbOwner.Items.AddRange(DatabaseService.GetCustomers().Cast<object>().ToArray());
                cbOwner.SelectedItem = DatabaseService.GetCustomer(dev.CustomerID);
                var btnSave = new Button { Text = "Save", Location = new Point(170, 235), Width = 75 };
                var btnCancel = new Button { Text = "Cancel", Location = new Point(255, 235), Width = 75 };
                btnSave.Click += (s, ev) =>
                {
                    dev.Type = txtType.Text.Trim(); dev.Brand = txtBrand.Text.Trim(); dev.Model = txtModel.Text.Trim(); dev.CustomerID = ((Customer)cbOwner.SelectedItem).ID; DatabaseService.UpdateDevice(dev); f.DialogResult = DialogResult.OK; f.Close();
                };
                btnCancel.Click += (s, ev) => f.Close();
                f.Controls.AddRange(new Control[] { lblType, txtType, lblBrand, txtBrand, lblModel, txtModel, lblOwner, cbOwner, btnSave, btnCancel });
                if (f.ShowDialog(this) == DialogResult.OK)
                    LoadDevices();
            }
        }

        private void DgvDevices_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dgvDevices.SelectedRows.Count > 0)
            {
                var id = dgvDevices.SelectedRows[0].Cells[0].Value?.ToString();
                var dev = DatabaseService.GetDevice(id);
                if (dev != null)
                {
                    var ok = MessageBox.Show($"Delete device {dev.ID}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (ok == DialogResult.Yes)
                    {
                        DatabaseService.DeleteDevice(dev.ID);
                        LoadDevices();
                    }
                }
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            string filter = textBox1.Text.Trim();
            if (filter == "Search devices...") filter = "";

            LoadDevices(string.IsNullOrEmpty(filter) ? null : filter);
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search devices...")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Search devices...";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        }

        private void lblDateTime_Click(object sender, EventArgs e) { }
    }
}
