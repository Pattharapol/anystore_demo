using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class categoriesDAL
    {
        //Static string method for Database Connection string
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        SqlConnection conn = new SqlConnection(myconnstring);


        #region Select Method
        public DataTable Select()
        {
            DataTable dt = new DataTable();
            
            try
            {
                conn.Open();
                string query = "SELECT * FROM tbl_categories";
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

        #region Insert New Category
        public bool Insert(categoriesBLL c)
        {
            bool result = false;

            string query = "INSERT INTO tbl_categories (title, description, added_date, added_by) VALUES (@title, @description, @added_date, @added_by)";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", c.title);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
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

        #region Update Method
        public bool Update(categoriesBLL c)
        {
            bool result = false;

            string query = "UPDATE tbl_categories SET title = @title, description = @description, added_date = @added_date, added_by = @added_by WHERE id = @id";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", c.title);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);
                cmd.Parameters.AddWithValue("@id", c.id);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
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

        #region Delete Category
        public bool Delete(categoriesBLL c)
        {
            bool result = false;

            string query = "DELETE FROM tbl_categories WHERE id = @id";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", c.id);

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

        #region Search Functionlity
        public DataTable Search(string keywords)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(myconnstring);
            conn.Open();
            try
            {
                string query = string.Format(@"select * from tbl_categories where id like '%"+keywords+"%' or title like '%"+keywords+"%' or description like '%"+keywords+"%' ");
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

    }
}
