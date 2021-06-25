using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ET;

namespace DAL
{
    public class OrdersDAL
    {
        private SqlConnection SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_Connection"].ToString());

        public string AddStagingOrder(StagingOrder Detail)
        {
            string rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspAddStagingOrder]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@UserID", Detail.UserID);
                SqlCmd.Parameters.AddWithValue("@DeliveryID", Detail.DeliveryID);
                SqlCmd.Parameters.AddWithValue("@OrderDetails", Detail.OrderDetails);

                //Exec Command
                rpta = SqlCmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return rpta;
        }
        public StagingOrder SearchStaginOrder(string StagingOrderID)
        {
            StagingOrder StageOrder = new StagingOrder();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspReadStagingOrder]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@StagingOrderID", StagingOrderID);

                //EXEC Command
                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        StageOrder.StagingOrderID = dr["StageOrderID"].ToString();
                        StageOrder.UserID = Convert.ToInt32(dr["UserID"]);
                        StageOrder.DeliveryID = Convert.ToInt32(dr["DeliveryID"]);
                        StageOrder.OrderDetails = dr["OrderDetails"].ToString();
                        StageOrder.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return StageOrder;
        }
    }
}
