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


        public AccessRights ValidationRights(AccessRightsRequest model)
        {
            var Detail = new AccessRights();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspValidationRight]", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pUserName = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = model.UserID
                };
                SqlCmd.Parameters.Add(pUserName);

                SqlParameter pController = new SqlParameter
                {
                    ParameterName = "@Controller",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = model.Controller
                };
                SqlCmd.Parameters.Add(pController);

                SqlParameter pAction = new SqlParameter
                {
                    ParameterName = "@Action",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = model.Action
                };
                SqlCmd.Parameters.Add(pAction);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Detail.ReadRight = Convert.ToBoolean(dr["ReadRight"]);
                        Detail.WriteRight = Convert.ToBoolean(dr["WriteRight"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return Detail;
        }

        public List<WebDirectory> List(int AppID)
        {
            List<WebDirectory> List = new List<WebDirectory>();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspReadWebDirectory]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@AppID", AppID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new WebDirectory
                        {
                            WebID = Convert.ToInt32(dr["WebID"]),
                            AppID = AppID,
                            Controller = dr["Controller"].ToString(),
                            Action = dr["Action"].ToString(),
                            PublicMenu = Convert.ToBoolean(dr["PublicMenu"]),
                            AdminMenu = Convert.ToBoolean(dr["AdminMenu"]),
                            DisplayName = dr["DisplayName"].ToString(),
                            Parameter = dr["Parameter"].ToString(),
                            Order = Convert.ToInt32(dr["Order"]),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"])
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

        public bool AddNew(WebDirectory Detail, string InsertUser)
        {
            bool rpta = false;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddWebDirectory]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter AppID = new SqlParameter
                {
                    ParameterName = "@AppID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.AppID
                };
                SqlCmd.Parameters.Add(AppID);

                SqlParameter Controller = new SqlParameter
                {
                    ParameterName = "@Controller",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.Controller
                };
                SqlCmd.Parameters.Add(Controller);

                SqlParameter Action = new SqlParameter
                {
                    ParameterName = "@Action",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.Action
                };
                SqlCmd.Parameters.Add(Action);

                SqlParameter PublicMenu = new SqlParameter
                {
                    ParameterName = "@PublicMenu",
                    SqlDbType = SqlDbType.Bit,
                    Value = Detail.PublicMenu
                };
                SqlCmd.Parameters.Add(PublicMenu);

                SqlParameter AdminMenu = new SqlParameter
                {
                    ParameterName = "@AdminMenu",
                    SqlDbType = SqlDbType.Bit,
                    Value = Detail.AdminMenu
                };
                SqlCmd.Parameters.Add(AdminMenu);

                SqlParameter DisplayName = new SqlParameter
                {
                    ParameterName = "@DisplayName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.DisplayName
                };
                SqlCmd.Parameters.Add(DisplayName);

                SqlParameter Parameter = new SqlParameter
                {
                    ParameterName = "@Parameter",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.Parameter
                };
                SqlCmd.Parameters.Add(Parameter);

                SqlParameter Order = new SqlParameter
                {
                    ParameterName = "@Order",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.Order
                };
                SqlCmd.Parameters.Add(Order);

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

        public bool Update(WebDirectory Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspUpdateWebDirectory]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter pWebID = new SqlParameter
                {
                    ParameterName = "@WebID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.WebID
                };
                SqlCmd.Parameters.Add(pWebID);

                SqlParameter pActionType = new SqlParameter
                {
                    ParameterName = "@ActionType",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 10,
                    Value = Detail.ActionType
                };
                SqlCmd.Parameters.Add(pActionType);

                SqlParameter AppID = new SqlParameter
                {
                    ParameterName = "@AppID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.AppID
                };
                SqlCmd.Parameters.Add(AppID);

                SqlParameter Controller = new SqlParameter
                {
                    ParameterName = "@Controller",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.Controller
                };
                SqlCmd.Parameters.Add(Controller);

                SqlParameter Action = new SqlParameter
                {
                    ParameterName = "@Action",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.Action
                };
                SqlCmd.Parameters.Add(Action);

                SqlParameter PublicMenu = new SqlParameter
                {
                    ParameterName = "@PublicMenu",
                    SqlDbType = SqlDbType.Bit,
                    Value = Detail.PublicMenu
                };
                SqlCmd.Parameters.Add(PublicMenu);

                SqlParameter AdminMenu = new SqlParameter
                {
                    ParameterName = "@AdminMenu",
                    SqlDbType = SqlDbType.Bit,
                    Value = Detail.AdminMenu
                };
                SqlCmd.Parameters.Add(AdminMenu);

                SqlParameter DisplayName = new SqlParameter
                {
                    ParameterName = "@DisplayName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.DisplayName
                };
                SqlCmd.Parameters.Add(DisplayName);

                SqlParameter Parameter = new SqlParameter
                {
                    ParameterName = "@Parameter",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Detail.Parameter
                };
                SqlCmd.Parameters.Add(Parameter);

                SqlParameter Order = new SqlParameter
                {
                    ParameterName = "@Order",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.Order
                };
                SqlCmd.Parameters.Add(Order);

                SqlParameter pActiveFlag = new SqlParameter
                {
                    ParameterName = "@ActiveFlag",
                    SqlDbType = SqlDbType.Bit,
                    Value = Detail.ActiveFlag
                };
                SqlCmd.Parameters.Add(pActiveFlag);

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
