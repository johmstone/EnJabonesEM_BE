using ET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CostaRicaDataDAL
    {

        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());

        public List<CostaRicaDistrics> Districts(int CantonID, int ProvinceID)
        {
            List<CostaRicaDistrics> List = new List<CostaRicaDistrics>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadCRDataDistrics]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@CantonID", CantonID);
                SqlCmd.Parameters.AddWithValue("@ProvinceID", ProvinceID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new CostaRicaDistrics
                        {
                            CostaRicaID = Convert.ToInt32(dr["CostaRicaID"]),
                            DistrictID = Convert.ToInt32(dr["DistrictID"]),
                            District = dr["District"].ToString(),
                            GAMFlag = Convert.ToBoolean(dr["GAMFlag"])
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

        public List<CostaRicaCantons> Cantons(int ProvinceID)
        {
            List<CostaRicaCantons> newList = new List<CostaRicaCantons>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadCRDataCantons]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@ProvinceID", ProvinceID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new CostaRicaCantons
                        {
                            CantonID = Convert.ToInt32(dr["CantonID"]),
                            Canton = dr["Canton"].ToString()
                        };
                        newList.Add(detail);
                    }
                }

                //foreach(var item in newList)
                //{
                //    item.Districts = Districts(item.CantonID, ProvinceID);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return newList;
        }

        public List<CostaRicaProvinces> Provinces()
        {
            List<CostaRicaProvinces> newList = new List<CostaRicaProvinces>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadCRDataProvinces]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new CostaRicaProvinces
                        {
                            ProvinceID = Convert.ToInt32(dr["ProvinceID"]),
                            Province = dr["Province"].ToString()
                        };
                        newList.Add(detail);
                    }
                }

                //foreach (var item in newList)
                //{
                //    item.Cantons = Cantons(item.ProvinceID);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return newList;
        }
    }
}
