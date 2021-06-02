using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;

namespace DAL
{
    public class FacturationInfoDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());
        public List<FacturatioInfo> List(int UserID)
        {
            List<FacturatioInfo> List = new List<FacturatioInfo>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspReadFacturationInfoByUser]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@UserID", UserID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new FacturatioInfo
                        {
                            FacturationInfoID = Convert.ToInt32(dr["FacturationInfoID"]),
                            UserID = UserID,
                            IdentityType = dr["IdentityType"].ToString(),
                            IdentityID = dr["IdentityID"].ToString(),
                            FullName = dr["FullName"].ToString(),
                            PhoneNumber = Convert.ToInt32(dr["PhoneNumber"]),
                            CostaRicaID = Convert.ToInt32(dr["CostaRicaID"]),
                            ProvinceID = Convert.ToInt32(dr["ProvinceID"]),
                            Province = dr["Province"].ToString(),
                            CantonID = Convert.ToInt32(dr["CantonID"]),
                            Canton = dr["Canton"].ToString(),
                            DistrictID = Convert.ToInt32(dr["DistrictID"]),
                            District = dr["District"].ToString(),
                            Street = dr["Street"].ToString(),
                            PrimaryFlag = Convert.ToBoolean(dr["PrimaryFlag"])                            
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

        public bool Update(FacturatioInfo Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspUpdateFacturationInfo]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);
                SqlCmd.Parameters.AddWithValue("@ActionType", Detail.ActionType);
                SqlCmd.Parameters.AddWithValue("@FacturationInfoID", Detail.FacturationInfoID);

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
        public bool AddNew(FacturatioInfo Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspAddFacturationInfo]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);
                SqlCmd.Parameters.AddWithValue("@UserID", Detail.UserID);
                SqlCmd.Parameters.AddWithValue("@IdentityType", Detail.IdentityType);
                SqlCmd.Parameters.AddWithValue("@IdentityID", Detail.IdentityID);
                SqlCmd.Parameters.AddWithValue("@FullName", Detail.FullName);
                SqlCmd.Parameters.AddWithValue("@PhoneNumber", Detail.PhoneNumber);
                SqlCmd.Parameters.AddWithValue("@CostaRicaID", Detail.CostaRicaID);
                SqlCmd.Parameters.AddWithValue("@Street", Detail.Street);

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
