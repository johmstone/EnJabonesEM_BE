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
                            AddressID = Convert.ToInt32(dr["AddressID"]),
                            ContactName = dr["ContactName"].ToString(),
                            CostaRicaID = Convert.ToInt32(dr["CostaRicaID"]),
                            ProvinceID = Convert.ToInt32(dr["ProvinceID"]),
                            Province = dr["Province"].ToString(),
                            CantonID = Convert.ToInt32(dr["CantonID"]),
                            Canton = dr["Canton"].ToString(),
                            DistrictID = Convert.ToInt32(dr["DistrictID"]),
                            District = dr["District"].ToString(),
                            PhoneNumber = Convert.ToInt32(dr["PhoneNumber"]),
                            Notes = dr["Notes"].ToString(),
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
    }
}
