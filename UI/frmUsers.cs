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
    public partial class frmUsers : Form
    {
        userBLL u = new userBLL();
        userDAL dal = new userDAL();

        public frmUsers()
        {
            InitializeComponent();
        }

        private void FillDGVUsers()
        {
            dgvUsers.ClearSelection();
            dgvUsers.DataSource = null;
            string sql = "SELECT * FROM tbl_users where first_name like '%" + txtSearch.Text.Trim() + "%' or last_name like '"+txtSearch.Text.Trim()+"' ";
            DataTable dt = dal.Select(sql);
            dgvUsers.DataSource = dt;
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            FillDGVUsers();
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(txtFName.Text.Length == 0)
            {
                ep.SetError(txtFName, "Required missing field...");
                txtFName.Focus();
                return;
            }

            if (txtLName.Text.Length == 0)
            {
                ep.SetError(txtLName, "Required missing field...");
                txtLName.Focus();
                return;
            }

            if (txtUsername.Text.Length == 0)
            {
                ep.SetError(txtUsername, "Required missing field...");
                txtUsername.Focus();
                return;
            }

            if (txtPassword.Text.Length == 0)
            {
                ep.SetError(txtPassword, "Required missing field...");
                txtPassword.Focus();
                return;
            }

            // Getting Data from UI
            u.first_name = txtFName.Text.Trim();
            u.last_name = txtLName.Text.Trim();
            u.email = txtEmail.Text.Trim();
            u.username = txtUsername.Text.Trim();
            u.password = txtPassword.Text.Trim();
            u.contact = txtContact.Text.Trim();
            u.address = txtAddress.Text.Trim();
            u.gender = cmbGender.Text.Trim();
            u.user_type = cmbUserType.Text.Trim();
            u.added_date = DateTime.Now;

            //Getting Username of the logged in user
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = dal.GetIDFromUsername(loggedUser);

            u.added_by = usr.id;

            //Inserting Data into Database
            bool result = dal.Insert(u);

            //If data is successfully inserted then the success will be true else it will be false
            if(result == true)
            {
                //Data successfully inserted
                MessageBox.Show("Insert Done...");
                ClearData();
                FillDGVUsers();
            }
            else
            {
                //Failed to insert data
                MessageBox.Show("Insert Failed...");
            }
        }

        private void ClearData()
        {
            btnAdd.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            txtUserID.Clear();
            txtAddress.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtFName.Clear();
            txtLName.Clear();
            txtPassword.Clear();
            txtUsername.Clear();
            cmbGender.Text = "";
            cmbUserType.Text = "";
            txtFName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //FillDGVUsers();


            string keyword = txtSearch.Text.Trim();

            //Check if keyword has value or not
            if(keyword != null)
            {
                DataTable dt = dal.Search(keyword);
                dgvUsers.DataSource = dt;
            }
            else
            {
                string sql = "SELECT * FROM tbl_users";
                DataTable dt = dal.Select(sql);
                dgvUsers.DataSource = dt;
            }
        }

        private void dgvUsers_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowindex = e.RowIndex;
            txtUserID.Text = dgvUsers.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtFName.Text = dgvUsers.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtLName.Text = dgvUsers.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvUsers.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtUsername.Text = dgvUsers.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtPassword.Text = dgvUsers.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtContact.Text = dgvUsers.Rows[e.RowIndex].Cells[6].Value.ToString();
            txtAddress.Text = dgvUsers.Rows[e.RowIndex].Cells[7].Value.ToString();
            cmbGender.Text = dgvUsers.Rows[e.RowIndex].Cells[8].Value.ToString();
            cmbUserType.Text = dgvUsers.Rows[e.RowIndex].Cells[9].Value.ToString();
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtFName.Text.Length == 0)
            {
                ep.SetError(txtFName, "Required missing field...");
                txtFName.Focus();
                return;
            }

            if (txtLName.Text.Length == 0)
            {
                ep.SetError(txtLName, "Required missing field...");
                txtLName.Focus();
                return;
            }

            if (txtUsername.Text.Length == 0)
            {
                ep.SetError(txtUsername, "Required missing field...");
                txtUsername.Focus();
                return;
            }

            if (txtPassword.Text.Length == 0)
            {
                ep.SetError(txtPassword, "Required missing field...");
                txtPassword.Focus();
                return;
            }

            if (MessageBox.Show("You want to do this?", "Asking", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Get the Value from User UI
                u.id = Convert.ToInt32(txtUserID.Text.Trim());
                u.first_name = txtFName.Text.Trim();
                u.last_name = txtLName.Text.Trim();
                u.email = txtEmail.Text.Trim();
                u.username = txtUsername.Text.Trim();
                u.password = txtPassword.Text.Trim();
                u.contact = txtContact.Text.Trim();
                u.address = txtAddress.Text.Trim();
                u.gender = cmbGender.Text.Trim();
                u.user_type = cmbUserType.Text.Trim();
                u.added_date = DateTime.Now;
                u.added_by = 1;

                //Update Data into Database
                bool result = dal.Update(u);

                //If data is updated successfully then the value of result wwill be true else it will be false
                if (result == true)
                {
                    //Data successfully updated
                    MessageBox.Show("Done update...");
                    ClearData();
                    FillDGVUsers();
                }
                else
                {
                    //Failed to update data
                    MessageBox.Show("Update Failed...");
                    ClearData();
                    FillDGVUsers();
                }
            }
            else
            {
                ClearData();
                FillDGVUsers();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("You want to do this?", "Asking",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Get the UseID from dgvUsers
                u.id = Convert.ToInt32(txtUserID.Text.Trim());

                bool result = dal.Delete(u);

                //If data is deleted then result will be true else false
                if (result == true)
                {
                    //User deleted
                    MessageBox.Show("Done delete...");
                    ClearData();
                    FillDGVUsers();
                }
                else
                {
                    //Delete failed
                    MessageBox.Show("Delete failed...");
                    ClearData();
                    FillDGVUsers();
                }
            }
            else
            {
                ClearData();
                FillDGVUsers();
            }
        }
    }
}
