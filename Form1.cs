using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Entity_Framework_CRUD
{
    public partial class Form1 : Form
    {
        CustomerNEW model = new CustomerNEW();
        public Form1()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            txtFirstName.Text = txtLastName.Text = txtCity.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();

            using (DB2Entities1 db = new DB2Entities1())
            {
                if (model.CustomerID == 0)//insert
                    db.CustomerNEW.Add(model);
                else //update
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            PopulateDataDridView();
            MessageBox.Show("Submitted Successfully");
        }

        void PopulateDataDridView()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using (DB2Entities1 db = new DB2Entities1())
            {
                dgvCustomer.DataSource = db.CustomerNEW.ToList<CustomerNEW>();
            }
            
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["CustomerID"].Value);
                using (DB2Entities1 db = new DB2Entities1())
                {
                    model = db.CustomerNEW.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txtFirstName.Text = model.FirstName;
                    txtLastName.Text = model.LastName;
                    txtCity.Text = model.City;
                    txtAddress.Text = model.Address;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to DELETE this customer?", "EF CRUD Operation", MessageBoxButtons.YesNo)==DialogResult.Yes)
                using (DB2Entities1 db = new DB2Entities1())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.CustomerNEW.Attach(model);
                    db.CustomerNEW.Remove(model);
                    db.SaveChanges();
                    PopulateDataDridView();
                    Clear();
                    MessageBox.Show("Delete Successfully");
                }
        }
    }
}
