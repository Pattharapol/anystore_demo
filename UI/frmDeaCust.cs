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
    public partial class frmDeaCust : Form
    {
        deaCustBLL dc = new deaCustBLL();
        deaCustDAL dcDal = new deaCustDAL();
        userDAL uDal = new userDAL();
        string title = "C# dev by TIK";

        public frmDeaCust()
        {
            InitializeComponent();
            dgvDeaCust.ClearSelection();
        }

        private void frmDeaCust_Load(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void FillGrid()
        {
            DataTable dt = dcDal.Select();
            if(dt != null)
            {
                if(dt.Rows.Count > 0)
                {
                    dgvDeaCust.DataSource = dt;
                }
            }
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;
            txtAddress.Clear();
            txtContactNo.Clear();
            txtDeaCustID.Clear();
            txtEmail.Clear();
            txtName.Clear();
            txtSearch.Clear();
            txtName.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dc.type = cmbDeaCust.Text.Trim();
            dc.name = txtName.Text.Trim();
            dc.email = txtEmail.Text.Trim();
            dc.address = txtAddress.Text.Trim();
            dc.contact = txtContactNo.Text.Trim();
            dc.added_date = DateTime.Now;

            //Getting ID
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = uDal.GetIDFromUsername(loggedUser);
            dc.added_by = usr.id;
            bool result = dcDal.Insert(dc);
            if(result == true)
            {
                MessageBox.Show("Inserted Successfully...", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnCancel_Click(sender, e);
                FillGrid();
            }
            else
            {
                MessageBox.Show("Insert failed...", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnCancel_Click(sender, e);
            }

        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            btnAdd.Enabled = false;
            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
            if(dgvDeaCust.Rows.Count > 0)
            {
                if(dgvDeaCust.SelectedRows.Count == 1)
                {
                    int rowindex = e.RowIndex;
                    txtDeaCustID.Text = dgvDeaCust.Rows[rowindex].Cells[0].Value.ToString();
                    cmbDeaCust.Text = dgvDeaCust.Rows[rowindex].Cells[1].Value.ToString();
                    txtName.Text = dgvDeaCust.Rows[rowindex].Cells[2].Value.ToString();
                    txtEmail.Text = dgvDeaCust.Rows[rowindex].Cells[3].Value.ToString();
                    txtContactNo.Text = dgvDeaCust.Rows[rowindex].Cells[4].Value.ToString();
                    txtAddress.Text = dgvDeaCust.Rows[rowindex].Cells[5].Value.ToString();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to update this?", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dc.id = int.Parse(txtDeaCustID.Text);
                dc.type = cmbDeaCust.Text.Trim();
                dc.name = txtName.Text.Trim();
                dc.email = txtEmail.Text.Trim();
                dc.contact = txtContactNo.Text.Trim();
                dc.address = txtAddress.Text.Trim();
                dc.added_date = DateTime.Now;

                //Getting ID
                string loggedUser = frmLogin.loggedIn;
                userBLL usr = uDal.GetIDFromUsername(loggedUser);
                dc.added_by = usr.id;
                bool result = dcDal.Update(dc);
                if(result == true)
                {
                    MessageBox.Show("Updated Successfully...", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnCancel_Click(sender, e);
                    FillGrid();
                }
                else
                {
                    MessageBox.Show("Update failed...", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnCancel_Click(sender, e);
                }
            }
            else
            {
                btnCancel_Click(sender, e);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this?", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dc.id = int.Parse(txtDeaCustID.Text.Trim());
                bool result = dcDal.Delete(dc);
                if(result == true)
                {
                    MessageBox.Show("Deleted Successfully...", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnCancel_Click(sender, e);
                    FillGrid();
                }
                else
                {
                    MessageBox.Show("Unabled to delete...", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnCancel_Click(sender, e);
                }
            }
            else
            {
                btnCancel_Click(sender, e);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtSearch.Text.Trim();
            if(string.IsNullOrEmpty(keywords))
            {
                DataTable dt = dcDal.Select();
                if (dt != null)
                {
                    if (dgvDeaCust.Rows.Count > 0)
                    {
                        dgvDeaCust.DataSource = dt;
                    }
                }
            }
            else
            {
                DataTable dt = dcDal.Search(keywords);
                if (dt != null)
                {
                    if (dgvDeaCust.Rows.Count > 0)
                    {
                        dgvDeaCust.DataSource = dt;
                    }
                }
            }
        }
    }
}
