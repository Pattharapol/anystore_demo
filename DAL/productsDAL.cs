using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class productsDAL
    {

        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        SqlConnection conn = new SqlConnection(myconnstring);

        #region Select Product
        public DataTable Select()
        {
            conn.Open();
            DataTable dt = new DataTable();

            try
            {
                string query = string.Format(@"select * from tbl_products");
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }
        #endregion

        #region Insert Product
        public bool Insert(productsBLL p)
        {
            bool result = false;
            conn.Open();
            try
            {
                string query = string.Format(@"insert into tbl_products (name, category, description, rate, qty, added_date, added_by) values (@name, @category, @description, @rate, @qty, @added_date, @added_by)");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", p.Name);
                cmd.Parameters.AddWithValue("@category", p.category);
                cmd.Parameters.AddWithValue("@description", p.description);
                cmd.Parameters.AddWithValue("@rate", p.rate);
                cmd.Parameters.AddWithValue("@qty", p.qty);
                cmd.Parameters.AddWithValue("@added_date", p.added_date);
                cmd.Parameters.AddWithValue("@added_by", p.added_by);
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        #endregion

        #region Update Product
        public bool Update(productsBLL p)
        {
            bool result = false;
            conn.Open();
            try
            {
                string query = string.Format(@"UPDATE tbl_products SET name = @name, category = @category, description = @description, rate = @rate, qty = @qty, added_date = @added_date, added_by = @added_by WHERE id = @id");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", p.Name);
                cmd.Parameters.AddWithValue("@category", p.category);
                cmd.Parameters.AddWithValue("@description", p.description);
                cmd.Parameters.AddWithValue("@rate", p.rate);
                cmd.Parameters.AddWithValue("@qty", p.qty);
                cmd.Parameters.AddWithValue("@added_date", p.added_date);
                cmd.Parameters.AddWithValue("@added_by", p.added_by);
                cmd.Parameters.AddWithValue("@id", p.id);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        #endregion

        #region Delete Product
        public bool Delete(productsBLL p)
        {
            bool result = false;
            conn.Open();
            try
            {
                string query = string.Format(@"DELETE FROM tbl_products WHERE id = @id");
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", p.id);
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        #endregion

        #region Search Product
        public DataTable Search(string keywords)
        {
            DataTable dt = new DataTable();
            string query = string.Format(@"SELECT * FROM tbl_products WHERE name like '%"+keywords.Trim()+"%' OR description like '%"+keywords.Trim()+"%' ");
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        #endregion

        #region SEARCH PRODUCT for TRANSACTION
        public productsBLL SearchProductForTransaction(string keyword)
        {
            productsBLL pd = new productsBLL();
            DataTable dt = new DataTable();
            conn.Open();
            try
            {
                string query = string.Format(@"SELECT name, qty, rate FROM tbl_products WHERE name like '%" + keyword + "%' OR id LIKE '%" + keyword + "%'");
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    pd.Name = dt.Rows[0]["name"].ToString();
                    pd.qty = decimal.Parse(dt.Rows[0]["qty"].ToString());
                    pd.rate = decimal.Parse(dt.Rows[0]["rate"].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return pd;
        }
        #endregion

        #region Search PRODUCTID for TRANSACTION
        public productsBLL SearchProductID(string productName)
        {
            conn.Open();
            productsBLL p = new productsBLL();
            DataTable dt = new DataTable();
            string query = string.Format(@"SELECT id FROM tbl_products WHERE name = '"+productName+"' ");
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    p.id = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return p;
        }
        #endregion

    }
}
