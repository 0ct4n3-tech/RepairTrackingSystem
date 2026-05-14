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
    public partial class AddRepairForm : Form
    {
        public AddRepairForm()
        {
            InitializeComponent();
            this.Load += AddRepairForm_Load;
            cbCustomer.SelectedIndexChanged += CbCustomer_SelectedIndexChanged;
        }

        private void cbTechnician_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cbCustomer.SelectedIndex = -1;
            cbDevice.SelectedIndex = -1;
            cbTechnician.SelectedIndex = -1;
            cbStatus.SelectedIndex = -1;
            txtCost.Clear();
        
    }

        private void AddRepairForm_Load(object sender, EventArgs e)
        {
            // populate comboboxes from DataStore
            cbCustomer.Items.Clear();
            cbCustomer.Items.AddRange(DatabaseService.GetCustomers().Cast<object>().ToArray());

            cbTechnician.Items.Clear();
            cbTechnician.Items.AddRange(DatabaseService.GetTechnicians().Cast<object>().ToArray());

            cbDevice.Items.Clear();
            cbDevice.Items.AddRange(DatabaseService.GetDevices().Cast<object>().ToArray());
        }

        private void CbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            // when customer changes, filter devices
            var sel = cbCustomer.SelectedItem as Customer;
            cbDevice.Items.Clear();
            if (sel == null)
            {
                cbDevice.Items.AddRange(DatabaseService.GetDevices().Cast<object>().ToArray());
                return;
            }
            var devs = DatabaseService.GetDevices(sel.ID).Cast<object>().ToArray();
            cbDevice.Items.AddRange(devs);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbCustomer.SelectedItem == null || cbDevice.SelectedItem == null ||
                cbTechnician.SelectedItem == null || string.IsNullOrWhiteSpace(cbStatus.Text))
            {
                MessageBox.Show("Please select customer, device, technician and status.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Default to 0 if empty or invalid — don't block the save
            decimal cost = 0;
            if (!string.IsNullOrWhiteSpace(txtCost.Text))
            {
                string raw = txtCost.Text.Trim().Replace(",", "");
                if (!decimal.TryParse(raw, System.Globalization.NumberStyles.Any,
                                      System.Globalization.CultureInfo.InvariantCulture, out cost))
                {
                    MessageBox.Show("Please enter a valid number for Cost.", "Invalid Input",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            var repair = new Repair
            {
                RepairID = DatabaseService.GenerateRepairId(),
                CustomerID = ((Customer)cbCustomer.SelectedItem).ID,
                DeviceID = ((Device)cbDevice.SelectedItem).ID,
                TechnicianID = ((Technician)cbTechnician.SelectedItem).ID,
                Status = cbStatus.Text,
                Issue = "",
                Cost = cost,
                DateReceived = DateTime.Now,
                DateUpdated = null
            };

            DatabaseService.AddRepair(repair);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void AddRepairForm_Load_1(object sender, EventArgs e)
        {

        }

        private void txtCost_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
