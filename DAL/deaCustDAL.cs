using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class deaCustDAL
    {

        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        SqlConnection conn = new SqlConnection(myconnstring);
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adapter;

        #region SELECT for deadler and customer
        public DataTable Select()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = string.Format(@"SELECT * FROM tbl_dea_cust");
                cmd = new SqlCommand(query, conn);
                adapter = new SqlDataAdapter(cmd);
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

        #region INSERT for dealer and customer
        public bool Insert(deaCustBLL dc)
        {
            bool result = false;
            conn.Open();
            try
            {
                string query = string.Format(@"INSERT INTO tbl_dea_cust (type, name, email, contact, address, added_date, added_by) VALUES (@type, @name, @email, @contact, @address, @added_date, @added_by)");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@type", dc.type);
                cmd.Parameters.AddWithValue("@name", dc.name);
                cmd.Parameters.AddWithValue("@email", dc.email);
                cmd.Parameters.AddWithValue("@contact", dc.contact);
                cmd.Parameters.AddWithValue("@address", dc.address);
                cmd.Parameters.AddWithValue("@added_date", dc.added_date);
                cmd.Parameters.AddWithValue("@added_by", dc.added_by);
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

        #region UPDATE for dealer and customer
        public bool Update(deaCustBLL dc)
        {
            bool result = false;
            conn.Open();
            try
            {
                string query = string.Format(@"UPDATE tbl_dea_cust SET type = @type, name = @name, email = @email, contact = @contact, address = @address, added_date = @added_date, added_by = @added_by where id = @id");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@type", dc.type);
                cmd.Parameters.AddWithValue("@name", dc.name);
                cmd.Parameters.AddWithValue("@email", dc.email);
                cmd.Parameters.AddWithValue("@contact", dc.contact);
                cmd.Parameters.AddWithValue("@address", dc.address);
                cmd.Parameters.AddWithValue("@added_date", dc.added_date);
                cmd.Parameters.AddWithValue("@added_by", dc.added_by);
                cmd.Parameters.AddWithValue("@id", dc.id);
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

        #region DELETE for dealer and customer
        public bool Delete(deaCustBLL dc)
        {
            bool result = false;
            conn.Open();
            try
            {
                string query = string.Format(@"DELETE FROM tbl_dea_cust WHERE id = @id");
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", dc.id);
                int rows = cmd.ExecuteNonQuery();
                if(rows > 0)
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

        #region SEARCH for dealer and customer
        public DataTable Search(string keywords)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(myconnstring);
            conn.Open();
            try
            {
                string query = string.Format(@"select * from tbl_dea_cust where id like '%" + keywords + "%' or name like '%" + keywords + "%' or type like '%" + keywords + "%' ");
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

        #region SEARCH DEALER or CUSTOMER for TRANSACTION
        public deaCustBLL SearchDealerCustomerForTransaction(string keyword)
        {
            deaCustBLL dc = new deaCustBLL();

            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                string query = string.Format(@"SELECT name, email, contact, address FROM tbl_dea_cust WHERE id LIKE '%"+keyword+"%' OR name LIKE '%"+keyword+"%'");
                adapter = new SqlDataAdapter(query, conn);
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dc.name = dt.Rows[0]["name"].ToString();
                    dc.email = dt.Rows[0]["email"].ToString();
                    dc.contact = dt.Rows[0]["contact"].ToString();
                    dc.address = dt.Rows[0]["address"].ToString();
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


            return dc;
        }
        #endregion

        #region Get ID of Dealer or Customer based on NAME
        public deaCustBLL GetDeaCustIDOfName(string name)
        {
            deaCustBLL dc = new deaCustBLL();
            DataTable dt = new DataTable();
            conn.Open();
            string query = string.Format(@"SELECT id FROM tbl_dea_cust WHERE name = '"+name+"' ");
            try
            {
                cmd = new SqlCommand(query, conn);
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    dc.id = Convert.ToInt32(dt.Rows[0][0].ToString());
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

            return dc;
        }
        #endregion
    }
}
