using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class DeliveryAddressDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());
        public List<DeliveryAddress> List(int UserID)
        {
            List<DeliveryAddress> List = new List<DeliveryAddress>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspReadDeliveryAddressByUser]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@UserID", UserID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new DeliveryAddress
                        {
                            DeliveryAddressID = Convert.ToInt32(dr["DeliveryAddressID"]),
                            UserID = UserID,
                            ContactName = dr["ContactName"].ToString(),
                            CostaRicaID = Convert.ToInt32(dr["CostaRicaID"]),
                            ProvinceID = Convert.ToInt32(dr["ProvinceID"]),
                            Province = dr["Province"].ToString(),
                            CantonID = Convert.ToInt32(dr["CantonID"]),
                            Canton = dr["Canton"].ToString(),
                            DistrictID = Convert.ToInt32(dr["DistrictID"]),
                            District = dr["District"].ToString(),
                            Street = dr["Street"].ToString(),
                            PhoneNumber = Convert.ToInt32(dr["PhoneNumber"]),
                            PrimaryFlag = Convert.ToBoolean(dr["PrimaryFlag"]),
                            GAMFlag = Convert.ToBoolean(dr["GAMFlag"]),
                            ActiveFlag = true
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

        public bool Update(DeliveryAddress Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspUpdateDeliveryAddress]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);
                SqlCmd.Parameters.AddWithValue("@ActionType", Detail.ActionType);
                SqlCmd.Parameters.AddWithValue("@DeliveryAddressID", Detail.DeliveryAddressID);
                SqlCmd.Parameters.AddWithValue("@ContactName", Detail.ContactName);
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
        public bool AddNew(DeliveryAddress Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspAddDeliveryAddress]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);
                SqlCmd.Parameters.AddWithValue("@UserID", Detail.UserID);
                SqlCmd.Parameters.AddWithValue("@ContactName", Detail.ContactName);
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
