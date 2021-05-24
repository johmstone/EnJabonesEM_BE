using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class RolesDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());
        public List<Role> List()
        {
            List<Role> List = new List<Role>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspReadRoles]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Role
                        {
                            RoleID = Convert.ToInt32(dr["RoleID"]),
                            RoleName = dr["RoleName"].ToString(),
                            RoleDescription = dr["RoleDescription"].ToString()
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

        public bool AddNew(Role Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddRole]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter RoleName = new SqlParameter
                {
                    ParameterName = "@RoleName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 100,
                    Value = Detail.RoleName.Trim()
                };
                SqlCmd.Parameters.Add(RoleName);

                SqlParameter RoleDescription = new SqlParameter
                {
                    ParameterName = "@RoleDescription",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Detail.RoleDescription.Trim()
                };
                SqlCmd.Parameters.Add(RoleDescription);

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
