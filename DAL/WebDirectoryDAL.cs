using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class WebDirectoryDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());

        // Menu
        public List<WebDirectory> Menu(WebDirectoryRequest Model)
        {
            List<WebDirectory> List = new List<WebDirectory>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspWebDirectorybyUser]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@AppID", Model.AppID);
                SqlCmd.Parameters.AddWithValue("@UserID", Model.UserID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new WebDirectory
                        {
                            AppID = Convert.ToInt32(dr["AppID"]),
                            WebID = Convert.ToInt32(dr["WebID"]),
                            Controller = dr["Controller"].ToString(),
                            Action = dr["Action"].ToString(),
                            DisplayName = dr["DisplayName"].ToString(),
                            Parameter = dr["Parameter"].ToString(),
                            Order = Convert.ToInt32(dr["Order"])
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
