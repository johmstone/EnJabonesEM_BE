using ET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DAL
{
    public class PrimaryProductsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());
        
        public IEnumerable<PrimaryProduct> List()
        {
            IEnumerable<PrimaryProduct> List;

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadPrimaryProducts]", SqlCon)
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

        public PrimaryProduct Details(int PrimaryProductID)
        {
            PrimaryProduct Details;

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadPrimaryProducts]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@PrimaryProductID", PrimaryProductID);

                var x = JsonConvert.DeserializeObject<List<PrimaryProduct>>((string)SqlCmd.ExecuteScalar());
                Details = x[0];
                Details.Formula = Formula(PrimaryProductID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return Details;
        }

        public List<Formula> Formula(int PrimaryProductID)
        {
            List<Formula> List = new List<Formula>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadFormulas]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@PrimaryProductID", PrimaryProductID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Formula
                        {
                            FormulaID = Convert.ToInt32(dr["FormulaID"]),
                            PrimaryProductID = Convert.ToInt32(dr["PrimaryProductID"]),
                            IngredientID = Convert.ToInt32(dr["IngredientID"]),
                            IngredientName = dr["IngredientName"].ToString(),
                            TypeName = dr["TypeName"].ToString(),
                            Qty = Convert.ToDecimal(dr["Qty"]),
                            UnitID = Convert.ToInt32(dr["UnitID"]),
                            UnitName = dr["UnitName"].ToString(),
                            Symbol = dr["Symbol"].ToString()
                        };
                        List.Add(detail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            return List;
        }

        public bool AddFormula(PrimaryProduct Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspAddFormulaProduct]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);
                SqlCmd.Parameters.AddWithValue("@JSON", JsonConvert.SerializeObject(Detail.Formula));
                
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

        public bool UpdateFormula(PrimaryProduct Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspUpdateFormulaProduct]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);
                SqlCmd.Parameters.AddWithValue("@JSON", JsonConvert.SerializeObject(Detail.Formula));

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

        public bool Update(PrimaryProduct Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspUpdatePrimaryProduct]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);
                SqlCmd.Parameters.AddWithValue("@ActionType", Detail.ActionType);
                SqlCmd.Parameters.AddWithValue("@PrimaryProductID", Detail.PrimaryProductID);
                SqlCmd.Parameters.AddWithValue("@Name", Detail.Name);
                SqlCmd.Parameters.AddWithValue("@Description", Detail.Description);
                SqlCmd.Parameters.AddWithValue("@Technique", Detail.Technique);
                SqlCmd.Parameters.AddWithValue("@PhotoURL", Detail.PhotoURL);
                SqlCmd.Parameters.AddWithValue("@BrochureURL", Detail.BrochureURL);
                SqlCmd.Parameters.AddWithValue("@VisibleFlag", Detail.VisibleFlag);

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

        public bool AddNew(PrimaryProduct Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspAddPrimaryProduct]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);
                SqlCmd.Parameters.AddWithValue("@Name", Detail.Name);
                SqlCmd.Parameters.AddWithValue("@Description", Detail.Description);
                SqlCmd.Parameters.AddWithValue("@Technique", Detail.Technique);
                SqlCmd.Parameters.AddWithValue("@PhotoURL", Detail.PhotoURL);
                SqlCmd.Parameters.AddWithValue("@BrochureURL", Detail.BrochureURL);
                SqlCmd.Parameters.AddWithValue("@VisibleFlag", Detail.VisibleFlag);

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
