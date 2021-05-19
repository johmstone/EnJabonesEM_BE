using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class UserDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());

        public bool CheckUserEmailAvailability(string Email)
        {
            bool rpta;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspCheckUserEmailAvailability]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParUserName = new SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = Email.Trim()
                };
                SqlCmd.Parameters.Add(ParUserName);

                rpta = Convert.ToBoolean(SqlCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public bool AddUser(User NewUser, string InsertUser)
        {
            bool rpta;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddUser]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParInsertUser = new SqlParameter
                {
                    ParameterName = "@InsertUser",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = InsertUser.Trim()
                };
                SqlCmd.Parameters.Add(ParInsertUser);

                SqlParameter ParFullName = new SqlParameter
                {
                    ParameterName = "@FullName",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = NewUser.FullName.Trim()
                };
                SqlCmd.Parameters.Add(ParFullName);

                SqlParameter ParEmail = new SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.VarChar,
                    Value = NewUser.Email.Trim()
                };
                SqlCmd.Parameters.Add(ParEmail);

                SqlParameter ParPassword = new SqlParameter
                {
                    ParameterName = "@Password",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = NewUser.Password.Trim()
                };
                SqlCmd.Parameters.Add(ParPassword);

                SqlParameter ParRoleID = new SqlParameter
                {
                    ParameterName = "@RoleID",
                    SqlDbType = SqlDbType.Int,
                    Value = NewUser.RoleID
                };
                SqlCmd.Parameters.Add(ParRoleID);

                //EXEC Command
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

        public User Login(UserLogin User)
        {
            User LoginUser = new User();

            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            try
            {

                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspLoginUser]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter ParUserName = new SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = User.Email.Trim()
                };
                SqlCmd.Parameters.Add(ParUserName);

                SqlParameter ParPassword = new SqlParameter
                {
                    ParameterName = "@Password",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = User.Password.Trim()
                };
                SqlCmd.Parameters.Add(ParPassword);

                //EXEC Command
                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        LoginUser.UserID = Convert.ToInt32(dr["UserID"]);
                        LoginUser.NeedResetPwd = Convert.ToBoolean(dr["NeedResetPwd"]);
                        LoginUser.EmailValidated = Convert.ToBoolean(dr["EmailValidated"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return LoginUser;
        }

        public bool AddLogin(LoginRecord Login)
        {
            bool rpta;
            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspAddLogin]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@UserID", Login.UserID);
                SqlCmd.Parameters.AddWithValue("@IP", Login.IP);
                SqlCmd.Parameters.AddWithValue("@Country", Login.Country);
                SqlCmd.Parameters.AddWithValue("@Region", Login.Region);
                SqlCmd.Parameters.AddWithValue("@City", Login.City);

                rpta = Convert.ToBoolean(SqlCmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }

        public User Details(int UserID)
        {
            var Detail = new User();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspReadUsers]", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter pUserID = new SqlParameter
                {
                    ParameterName = "@UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = UserID
                };
                SqlCmd.Parameters.Add(pUserID);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Detail.UserID = Convert.ToInt32(dr["UserID"]);
                        Detail.RoleID = Convert.ToInt32(dr["RoleID"]);
                        Detail.FullName = dr["FullName"].ToString();
                        Detail.Email = dr["Email"].ToString();
                        Detail.EmailValidated = Convert.ToBoolean(dr["EmailValidated"]);
                        Detail.Subscriber = Convert.ToBoolean(dr["Subscriber"]);
                        Detail.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        Detail.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                        Detail.RoleName = dr["RoleName"].ToString();
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

        public User DetailsbyEmail(string Email)
        {
            var Detail = new User();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspReadUsers]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@Email", Email);                

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Detail.UserID = Convert.ToInt32(dr["UserID"]);
                        Detail.RoleID = Convert.ToInt32(dr["RoleID"]);
                        Detail.FullName = dr["FullName"].ToString();
                        Detail.Email = dr["Email"].ToString();
                        Detail.EmailValidated = Convert.ToBoolean(dr["EmailValidated"]);
                        Detail.Subscriber = Convert.ToBoolean(dr["Subscriber"]);
                        Detail.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        Detail.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
                        Detail.RoleName = dr["RoleName"].ToString();
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

        public InfoEmailValidation ValidationEmailRequest(string Email)
        {
            var Detail = new InfoEmailValidation();

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspGenerateEmailValidationToken]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@Email", Email);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Detail.EVToken = dr["EVToken"].ToString();
                        Detail.UserID = Convert.ToInt32(dr["UserID"]);
                        Detail.FullName = dr["FullName"].ToString();                        
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

        public int ValidateEmail(string EVToken)
        {
            int result = 0;

            try
            {
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[adm].[uspValidateUserEmail]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@EVToken", EVToken);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        result = Convert.ToInt32(dr["Result"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return result;
        }
    }
}
