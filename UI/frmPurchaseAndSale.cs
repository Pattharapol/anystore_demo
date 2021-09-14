using AnyStore.BLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmPurchaseAndSale : Form
    {
        // this is for Add Data to DGV
        DataTable transactionDT = new DataTable();

        public frmPurchaseAndSale()
        {
            InitializeComponent();
        }

        private void frmPurchaseAndSale_Load(object sender, EventArgs e)
        {
            //Get the transactionType value from frmUserDashboard
            string type = frmUserDashBoard.transactionType;
            lblTop.Text = type;

            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Rate");
            transactionDT.Columns.Add("Qty");
            transactionDT.Columns.Add("Total");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtDCSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtDCSearch.Text.Trim();
            if(string.IsNullOrEmpty(keyword))
            {
                txtDCName.Clear();
                txtDCEmail.Clear();
                txtDCContact.Clear();
                txtDCAddress.Clear();
                return;
            }

            deaCustDAL dcDAL = new deaCustDAL();
            deaCustBLL dcBLL = new deaCustBLL();
            dcBLL = dcDAL.SearchDealerCustomerForTransaction(keyword);
            if(dcBLL != null)
            {
                txtDCName.Text = dcBLL.name;
                txtDCEmail.Text = dcBLL.email;
                txtDCContact.Text = dcBLL.contact;
                txtDCAddress.Text = dcBLL.address;
            }
        }

        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtProductSearch.Text.Trim();
                if (string.IsNullOrEmpty(keyword))
                {
                    txtProductInventory.Clear();
                    txtProductName.Clear();
                    txtProductQty.Clear();
                    txtProductRate.Clear();
                    return;
                }

                productsDAL pdDAL = new productsDAL();
                productsBLL pdBLL = pdDAL.SearchProductForTransaction(keyword);
                if (pdBLL != null)
                {
                    txtProductName.Text = pdBLL.Name;
                    txtProductRate.Text = pdBLL.rate.ToString();
                    txtProductInventory.Text = pdBLL.qty.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnADD_Click(object sender, EventArgs e)
        {
            string productName = txtProductName.Text;
            decimal Rate = decimal.Parse(txtProductRate.Text);
            decimal Qty = decimal.Parse(txtProductQty.Text);
            decimal Total = Rate * Qty;

            decimal subTotal = decimal.Parse(txtSubtotal.Text);
            subTotal = subTotal + Total;

            if(productName == "")
            {
                ep.SetError(txtProductName, "Required Field...");
                txtProductName.Focus();
                return;
            }

            if(Qty == 0)
            {
                ep.SetError(txtProductQty, "Required Field...");
                txtProductQty.Focus();
                return;
            }

            transactionDT.Rows.Add(productName, Rate, Qty, Total);
            dgvProducts.DataSource = transactionDT;
            dgvProducts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvProducts.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvProducts.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvProducts.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            txtSubtotal.Text = subTotal.ToString();

            txtProductInventory.Clear();
            txtProductName.Clear();
            txtProductQty.Clear();
            txtProductRate.Clear();
            txtProductSearch.Clear();
            txtProductSearch.Focus();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            string value = txtDiscount.Text.Trim();
            if(value == "")
            {
                MessageBox.Show("Please Add Discount First..");
                txtDiscount.Focus();
                return;
            }
            else
            {
                decimal subTotal = decimal.Parse(txtSubtotal.Text);
                decimal discount = decimal.Parse(value);

                decimal grandTotal = ((100 - discount) / 100) * subTotal;

                txtGrandTotal.Text = grandTotal.ToString("0.00");
            }
        }

        private void txtVAT_TextChanged(object sender, EventArgs e)
        {
            string check = txtGrandTotal.Text.Trim();
            if(check == "")
            {
                MessageBox.Show("Please Calculate discount and set the GrandTotal First...!");
                txtDiscount.Focus();
                return;
            }
            else
            {
                decimal previousGrandTotal = decimal.Parse(txtGrandTotal.Text.Trim());
                decimal vat = decimal.Parse(txtVAT.Text.Trim());
                decimal grandTotal = ((100 + vat) / 100) * previousGrandTotal;

                txtGrandTotal.Text = grandTotal.ToString("0.00");
            }
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            decimal grandTotal = decimal.Parse(txtGrandTotal.Text.Trim());
            decimal paidAmount = decimal.Parse(txtPaidAmount.Text.Trim());

            decimal returnAmount = paidAmount - grandTotal;
            //if(returnAmount < 0)
            //{
            //    MessageBox.Show("Please Paid more or equal to GrandTotal...");
            //    txtPaidAmount.Focus();
            //    txtPaidAmount.SelectAll();
            //    return;
            //}

            txtReturnAMount.Text = returnAmount.ToString("0.00");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Insert into Transaction First

            transactionDAL tranDAL = new transactionDAL();
            transactionDetailDAL tranDetailDAL = new transactionDetailDAL();
            deaCustDAL dcDAL = new deaCustDAL();
            deaCustBLL dcBLL = new deaCustBLL();

            //Check If returnAmount < 0
            decimal returnAmount = decimal.Parse(txtReturnAMount.Text.Trim());
            if(returnAmount < 0)
            {
                MessageBox.Show("Please pay more or equal to GrandTotal...");
                txtPaidAmount.Focus();
                txtPaidAmount.SelectAll();
                return;
            }

            // Get the value from PurchaseSaleFOrm first...
            transactionsBLL transaction = new transactionsBLL();
            transaction.type = lblTop.Text;

            

            //Get the ID of Dealer or Customer
            //Get the name of the Dealer or Customer
            string deaCustomer = txtDCName.Text;
            
            dcBLL = dcDAL.GetDeaCustIDOfName(deaCustomer);
            transaction.dea_cust_id = dcBLL.id;
            transaction.grandTotal = decimal.Parse(txtGrandTotal.Text.Trim());
            transaction.transaction_date = DateTime.Now;
            transaction.tax = decimal.Parse(txtVAT.Text.Trim());
            transaction.discount = decimal.Parse(txtDiscount.Text.Trim());


            //Get the Username of Loggedin user
            string username = frmLogin.loggedIn;
            userDAL udal = new userDAL();
            userBLL usr = udal.GetIDFromUsername(username);
            transaction.added_by = usr.id;

            // COde to Insert Transaction and TransactionDetails
            bool success = false;
            int transactionID = 1;
            bool result1 = tranDAL.Insert(transaction, out transactionID);
            if(result1 == true)
            {
                //USer for loop to insert transactionDetails
                for (int i = 0; i < transactionDT.Rows.Count; i++)
                {
                    transactionDetailsBLL transactionDetails = new transactionDetailsBLL();

                    //Get the ProductID
                    productsDAL pdal = new productsDAL();
                    productsBLL pbal = pdal.SearchProductID(txtProductName.Text);

                    transactionDetails.product_id = pbal.id;
                    transactionDetails.rate = decimal.Parse(dgvProducts.Rows[i].Cells[1].Value.ToString());
                    transactionDetails.qty = decimal.Parse(dgvProducts.Rows[i].Cells[2].Value.ToString());
                    transactionDetails.total = decimal.Parse(dgvProducts.Rows[i].Cells[3].Value.ToString());
                    transactionDetails.dea_cust_id = dcBLL.id;
                    transactionDetails.added_date = DateTime.Now;
                    transactionDetails.added_by = usr.id;

                    bool result2 = tranDetailDAL.Insert(transactionDetails);
                    success = result1 && result2;
                    if (success == true)
                    {
                        // Transaction Conplete
                        MessageBox.Show("Conplete");
                    }
                    else
                    {
                        // Transaction Failed
                        MessageBox.Show("Failed");
                    }
                }
            }
        }
    }
}
