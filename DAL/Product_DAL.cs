using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ADO_Example.Models;

namespace ADO_Example.DAL
{
    public class Product_DAL
    {
        string con = ConfigurationManager.ConnectionStrings["adoConnectionString"].ToString();


        //GetAllProducts
        public List<Product> GetAllProducts()
        {
            List<Product> ProductList = new List<Product>();
            using (SqlConnection connection = new SqlConnection(con))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetAllProducts";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        ProductList.Add(new Product
                        {
                            ProuductID = Convert.ToInt32(dt.Rows[i]["ProductID"]),
                            ProductName = dt.Rows[i]["ProductName"].ToString(),
                            Price = Convert.ToDecimal(dt.Rows[i]["Price"]),
                            Qty = Convert.ToInt32(dt.Rows[i]["Qty"]),
                            Remarks = dt.Rows[i]["Remarks"].ToString()
                        });
                    }
                }



            }
            return ProductList;

        }


        //Get Products By ID

        public List<Product> GetAllProductByID(int productID)
        {
            List<Product> ProductList = new List<Product>();
            using (SqlConnection connection = new SqlConnection(con))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetProductByID";
                cmd.Parameters.AddWithValue("@ProductID", productID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ProductList.Add(new Product
                        {
                            ProuductID = Convert.ToInt32(dt.Rows[i]["ProductID"]),
                            ProductName = dt.Rows[i]["ProductName"].ToString(),
                            Price = Convert.ToDecimal(dt.Rows[i]["Price"]),
                            Qty = Convert.ToInt32(dt.Rows[i]["Qty"]),
                            Remarks = dt.Rows[i]["Remarks"].ToString()
                        });
                    }
                }



            }
            return ProductList;

        }

        //Update Products
        public bool UpdateProduct(Product product)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(con))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateProducts", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductID",product.ProuductID);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Qty", product.Qty);
                cmd.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                id = cmd.ExecuteNonQuery();
                connection.Close();


            }
            if (id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Insert Products
        public bool InsertProduct(Product product)
        {
            int id = 0;
            using(SqlConnection connection = new SqlConnection(con))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertProducts", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Qty", product.Qty);
                cmd.Parameters.AddWithValue("@Remarks", product.Remarks);

                connection.Open();
                id = cmd.ExecuteNonQuery();
                connection.Close();


            }
            if (id > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Delete Product
        public string DeleteProduct(int  productID)
        {
            string result ="";
            using (SqlConnection connection = new SqlConnection(con))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteProduct", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductID", productID);
                cmd.Parameters.Add("@OUTPUTMESSAGE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                connection.Open();
                cmd.ExecuteNonQuery();
                result = cmd.Parameters["@OUTPUTMESSAGE"].Value.ToString();
                connection.Close();
            }
            return result;
        }
    }
}