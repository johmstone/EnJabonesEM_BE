using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ET;
using Newtonsoft.Json;

namespace DAL
{
    public class ProductsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());


        public IEnumerable<PrimaryProduct> List()
        {
            IEnumerable<PrimaryProduct> List;

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspReadProducts]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataReader reader = SqlCmd.ExecuteReader();
                StringBuilder sb = new StringBuilder();
                while (reader.Read())
                {
                    sb.Append(reader.GetSqlString(0).Value);
                }

                List = JsonConvert.DeserializeObject<List<PrimaryProduct>>(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return List;
        }

        public bool AddNew(Product Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspAddProduct]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@PrimaryProductID", Detail.PrimaryProductID);
                SqlCmd.Parameters.AddWithValue("@Qty", Detail.Qty);
                SqlCmd.Parameters.AddWithValue("@UnitID", Detail.UnitID);
                SqlCmd.Parameters.AddWithValue("@Price", Detail.Price);
                SqlCmd.Parameters.AddWithValue("@IVA", Detail.IVA);
                SqlCmd.Parameters.AddWithValue("@Discount", Detail.Discount);
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);

                //Exec Command
                SqlCmd.ExecuteNonQuery();

                rpta = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public bool Update(Product Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspUpdateProduct]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@ProductID", Detail.ProductID);
                SqlCmd.Parameters.AddWithValue("@ActionType", Detail.ActionType);
                SqlCmd.Parameters.AddWithValue("@Qty", Detail.Qty);
                SqlCmd.Parameters.AddWithValue("@UnitID", Detail.UnitID);
                SqlCmd.Parameters.AddWithValue("@Price", Detail.Price);
                SqlCmd.Parameters.AddWithValue("@IVA", Detail.IVA);
                SqlCmd.Parameters.AddWithValue("@Discount", Detail.Discount);
                SqlCmd.Parameters.AddWithValue("@VisibleFlag", Detail.VisibleFlag);
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);

                //Exec Command
                SqlCmd.ExecuteNonQuery();

                rpta = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }
    }
}
