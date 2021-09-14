using AnyStore.BLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmCategories : Form
    {
        categoriesBLL c = new categoriesBLL();
        categoriesDAL dal = new categoriesDAL();
        userDAL udal = new userDAL();
        string title = "C# dev by TIK";

        public frmCategories()
        {
            InitializeComponent();
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text.Length == 0)
            {
                ep.SetError(txtTitle, "Required messing field...");
                txtTitle.Focus();
                return;
            }

            if (txtDescription.Text.Length == 0)
            {
                ep.SetError(txtDescription, "Required messing field...");
                txtDescription.Focus();
                return;
            }

            c.description = txtDescription.Text;
            c.title = txtTitle.Text;
            c.added_date = DateTime.Now;

            //Getting ID
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            c.added_by = usr.id;

            bool result = dal.Insert(c);
            if (result == true)
            {
                MessageBox.Show("New Category has been saved successfully...", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clear();
                FillGrid();
            }
            else
            {
                MessageBox.Show("Category save Failed...", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Clear()
        {
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            txtCategoryID.Clear();
            txtTitle.Clear();
            txtDescription.Clear();
            txtSearch.Clear();
            txtTitle.Focus();
        }

        private void FillGrid()
        {
            dgvCategories.DataSource = null;
            DataTable dt = dal.Select();
            if(dt != null)
            {
                if(dt.Rows.Count > 0)
                {
                    dgvCategories.DataSource = dt;

                    dgvCategories.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvCategories.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvCategories.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvCategories.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvCategories.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                else
                {
                    dgvCategories.DataSource = null;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void dgvCategories_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;

            int rowindex = e.RowIndex;
            if(dgvCategories != null)
            {
                if(dgvCategories.Rows.Count > 0)
                {
                    if(dgvCategories.SelectedRows.Count == 1)
                    {
                        //ทั้งสองแบบ ใช้งานได้ไม่ต่างกัน
                        txtCategoryID.Text = dgvCategories.Rows[e.RowIndex].Cells[0].Value.ToString();
                        txtTitle.Text = dgvCategories.Rows[e.RowIndex].Cells[1].Value.ToString();
                        txtDescription.Text = dgvCategories.Rows[e.RowIndex].Cells[2].Value.ToString();

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
            if (txtTitle.Text.Length == 0)
            {
                ep.SetError(txtTitle, "Required messing field...");
                txtTitle.Focus();
                return;
            }

            if (txtDescription.Text.Length == 0)
            {
                ep.SetError(txtDescription, "Required messing field...");
                txtDescription.Focus();
                return;
            }

            if (MessageBox.Show("Are you sure You want to update this?", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                c.id = Int32.Parse(txtCategoryID.Text);
                c.title = txtTitle.Text;
                c.description = txtDescription.Text;
                c.added_date = DateTime.Now;

                //Getting ID
                string loggedUser = frmLogin.loggedIn;
                userBLL usr = udal.GetIDFromUsername(loggedUser);
                c.added_by = usr.id;

                bool result = dal.Update(c);
                if (result == true)
                {
                    MessageBox.Show("Category has been updated successfully...", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    FillGrid();
                }
                else
                {
                    MessageBox.Show("Category update Failed...", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear();
                }
            }
            else
            {
                Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure You want to delete this?", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                c.id = Int32.Parse(txtCategoryID.Text);

                bool result = dal.Delete(c);
                if (result == true)
                {
                    MessageBox.Show("Category has been deleted successfully...", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    FillGrid();
                }
                else
                {
                    MessageBox.Show("Category delete failed...", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                }
            }
            else
            {
                Clear();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtSearch.Text.Trim();
            dgvCategories.DataSource = null;
            if(keywords != null)
            {
                DataTable dt = dal.Search(keywords);
                dgvCategories.DataSource = dt;
            }
            else
            {
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
        }
    }
}
