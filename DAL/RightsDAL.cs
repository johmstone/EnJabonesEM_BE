using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class RightsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());

        public List<Rights> List(int RoleID)
        {
            List<Rights> List = new List<Rights>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspReadRights]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter pRoleID = new SqlParameter
                {
                    ParameterName = "@RoleID",
                    SqlDbType = SqlDbType.Int,
                    Value = RoleID
                };
                SqlCmd.Parameters.Add(pRoleID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Rights
                        {
                            WebID = Convert.ToInt32(dr["WebID"]),
                            AppID = Convert.ToInt32(dr["AppID"]),
                            RoleID = Convert.ToInt32(dr["RoleID"]),
                            DisplayName = dr["DisplayName"].ToString(),
                            RightID = Convert.ToInt32(dr["RightID"]),
                            ReadRight = Convert.ToBoolean(dr["ReadRight"]),
                            WriteRight = Convert.ToBoolean(dr["WriteRight"])
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

        public bool Update(Rights Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpsertRightsbyRole]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter WebID = new SqlParameter
                {
                    ParameterName = "@WebID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.WebID
                };
                SqlCmd.Parameters.Add(WebID);

                SqlParameter RoleID = new SqlParameter
                {
                    ParameterName = "@RoleID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.RoleID
                };
                SqlCmd.Parameters.Add(RoleID);

                SqlParameter RightID = new SqlParameter
                {
                    ParameterName = "@RightID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.RightID
                };
                SqlCmd.Parameters.Add(RightID);

                SqlParameter ReadRight = new SqlParameter
                {
                    ParameterName = "@Read",
                    SqlDbType = SqlDbType.Bit,
                    Value = Detail.ReadRight
                };
                SqlCmd.Parameters.Add(ReadRight);

                SqlParameter WriteRight = new SqlParameter
                {
                    ParameterName = "@Write",
                    SqlDbType = SqlDbType.Bit,
                    Value = Detail.WriteRight
                };
                SqlCmd.Parameters.Add(WriteRight);

                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = InsertUser
                };
                SqlCmd.Parameters.Add(ParInsertUser);

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
