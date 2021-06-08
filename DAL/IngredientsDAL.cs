using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class IngredientsDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());
        public List<Ingredient> List()
        {
            List<Ingredient> List = new List<Ingredient>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadIngredients]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Ingredient
                        {
                            IngredientID = Convert.ToInt32(dr["IngredientID"]),
                            IngredientName = dr["IngredientName"].ToString(),
                            TypeID = Convert.ToInt32(dr["TypeID"]),
                            TypeName = dr["TypeName"].ToString(),
                            PhotoURL = dr["PhotoURL"].ToString()
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

        public bool AddNew(Ingredient Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspAddIngredient]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter IngredientName = new SqlParameter
                {
                    ParameterName = "@IngredientName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Detail.IngredientName.Trim()
                };
                SqlCmd.Parameters.Add(IngredientName);

                SqlParameter TypeID = new SqlParameter
                {
                    ParameterName = "@TypeID",
                    SqlDbType = SqlDbType.Int,
                    Value = Detail.TypeID
                };
                SqlCmd.Parameters.Add(TypeID);

                SqlParameter PhotoURL = new SqlParameter
                {
                    ParameterName = "@PhotoURL",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Detail.PhotoURL
                };
                SqlCmd.Parameters.Add(PhotoURL);

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

        public List<Unit> Units()
        {
            List<Unit> List = new List<Unit>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadUnits]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Unit
                        {
                            UnitID = Convert.ToInt32(dr["UnitID"]),
                            UnitName = dr["UnitName"].ToString(),
                            Symbol = dr["Symbol"].ToString()
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

        public bool AddNewUnit(Unit Detail, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspAddUnit]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlParameter UnitName = new SqlParameter
                {
                    ParameterName = "@UnitName",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Detail.UnitName.Trim()
                };
                SqlCmd.Parameters.Add(UnitName);

                SqlParameter Symbol = new SqlParameter
                {
                    ParameterName = "@Symbol",
                    SqlDbType = SqlDbType.VarChar,
                    Value = Detail.Symbol
                };
                SqlCmd.Parameters.Add(Symbol);

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

        public List<IngredientType> Types()
        {
            List<IngredientType> List = new List<IngredientType>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[config].[uspReadIngredientTypes]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new IngredientType
                        {
                            TypeID = Convert.ToInt32(dr["TypeID"]),
                            TypeName = dr["TypeName"].ToString()
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
