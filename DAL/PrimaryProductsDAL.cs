using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class PrimaryProductsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());
        public List<PrimaryProduct> List()
        {
            List<PrimaryProduct> List = new List<PrimaryProduct>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadPrimaryProducts]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new PrimaryProduct
                        {
                            PrimaryProductID = Convert.ToInt32(dr["PrimaryProductID"]),
                            Name = dr["Name"].ToString(),
                            Description = dr["Description"].ToString(),
                            Technique = dr["Technique"].ToString(),
                            PhotoURL = dr["PhotoURL"].ToString(),
                            BrochureURL = dr["BrochureURL"].ToString(),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            VisibleFlag = Convert.ToBoolean(dr["VisibleFlag"])
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
