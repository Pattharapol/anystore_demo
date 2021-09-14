using AnyStore.BLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmProducts : Form
    {
        categoriesDAL cdal = new categoriesDAL();
        productsDAL pdal = new productsDAL();
        productsBLL p = new productsBLL();
        userDAL udal = new userDAL();
        string title = "C# dev by TIK";

        public frmProducts()
        {
            InitializeComponent();
        }

        private void frmProducts_Load(object sender, EventArgs e)
        {
            FillGrid();
            DataTable cat_dt = cdal.Select();
            cmbCategory.DataSource = cat_dt;
            cmbCategory.DisplayMember = "title";
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtDescription.Clear();
            txtName.Clear();
            txtProductID.Clear();
            txtRate.Clear();
            txtSearch.Clear();
            cmbCategory.Text = "";
            txtName.Focus();
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            p.Name = txtName.Text.Trim();
            p.category = cmbCategory.Text.Trim();
            p.description = txtDescription.Text.Trim();
            p.rate = decimal.Parse(txtRate.Text.Trim());
            p.qty = 0;
            p.added_date = DateTime.Now;

            //Getting ID of logged in
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            p.added_by = usr.id;

            bool result = pdal.Insert(p);
            if(result == true)
            {
                MessageBox.Show("Product Added Successfully...");
                btnCancel_Click(sender, e);
                FillGrid();
            }
            else
            {
                MessageBox.Show("Failed to Add New Product...");
                btnCancel_Click(sender, e);
                FillGrid();
            }
        }

        private void FillGrid()
        {
            dgvProducts.DataSource = null;
            DataTable dt = pdal.Select();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dgvProducts.DataSource = dt;
                    dgvProducts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvProducts.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvProducts.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                else
                {
                    dgvProducts.DataSource = null;
                }
            }
        }

        private void dgvProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            int rowindex = e.RowIndex;
            if (dgvProducts != null)
            {
                if (dgvProducts.Rows.Count > 0)
                {
                    if (dgvProducts.SelectedRows.Count == 1)
                    {
                        //ทั้งสองแบบ ใช้งานได้ไม่ต่างกัน
                        txtProductID.Text = dgvProducts.Rows[rowindex].Cells[0].Value.ToString();
                        txtName.Text = dgvProducts.Rows[rowindex].Cells[1].Value.ToString();
                        cmbCategory.Text = dgvProducts.Rows[rowindex].Cells[2].Value.ToString();
                        txtDescription.Text = dgvProducts.Rows[rowindex].Cells[3].Value.ToString();
                        txtRate.Text = dgvProducts.Rows[rowindex].Cells[4].Value.ToString();

                        //txtCategoryID.Text = dgvCategories.CurrentRow.Cells[0].Value.ToString();
                        //txtTitle.Text = dgvCategories.CurrentRow.Cells[1].Value.ToString();
                        //txtDescription.Text = dgvCategories.CurrentRow.Cells[2].Value.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Please Try again...", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please Try again...", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please Try again...", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            p.id = Int32.Parse(txtProductID.Text);
            p.Name = txtName.Text.Trim();
            p.description = txtDescription.Text.Trim();
            p.rate = decimal.Parse(txtRate.Text.Trim());
            p.category = cmbCategory.Text;
            p.added_date = DateTime.Now;

            //Getting ID of logged user
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            p.added_by = usr.id;

            bool result = pdal.Update(p);
            if(result == true)
            {
                MessageBox.Show("Product has been updated successfully...", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnCancel_Click(sender, e);
                FillGrid();
            }
            else
            {
                MessageBox.Show("Please Try again...", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnCancel_Click(sender, e);
                FillGrid();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            p.id = Int32.Parse(txtProductID.Text);

            bool result = pdal.Delete(p);
            if(result == true)
            {
                MessageBox.Show("Product has been deleted successfully...", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnCancel_Click(sender, e);
                FillGrid();
            }
            else
            {
                MessageBox.Show("Please Try again...", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnCancel_Click(sender, e);
                FillGrid();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvProducts.DataSource = null;
            DataTable dt = pdal.Search(txtSearch.Text.Trim());
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dgvProducts.DataSource = dt;
                    dgvProducts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvProducts.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvProducts.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvProducts.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

            }
        }
    }
}
