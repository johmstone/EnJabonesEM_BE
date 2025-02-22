﻿using System;
using System.Collections.Generic;
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

        public bool AddOrder(NewOrder Order)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspAddOrder]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@OrderID", Order.OrderID);
                SqlCmd.Parameters.AddWithValue("@OrderDetails", Order.OrderDetails);

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

        public List<Order> OrderList(SearchOrder Details)
        {
            List<Order> List = new List<Order>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspReadOrders]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@UserID", Details.UserID);
                SqlCmd.Parameters.AddWithValue("@ExternalStatusID", Details.ExternalStatusID);
                SqlCmd.Parameters.AddWithValue("@StartDate", Details.StartDate);
                SqlCmd.Parameters.AddWithValue("@EndDate", Details.EndDate);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new Order
                        {
                            OrderID = dr["OrderID"].ToString(),
                            UserID = Convert.ToInt32(dr["UserID"]),
                            FullName = dr["FullName"].ToString(),
                            OrderType = dr["OrderType"].ToString(),
                            OrderDate = Convert.ToDateTime(dr["OrderDate"]),
                            DeliveryID = Convert.ToInt32(dr["DeliveryID"]),
                            DeliveryAddressID = Convert.ToInt32(dr["DeliveryAddressID"]),
                            FacturationInfoID = Convert.ToInt32(dr["FacturationInfoID"]),
                            OrderDetails = dr["OrderDetails"].ToString(),
                            Discount = Convert.ToDecimal(dr["Discount"]),
                            TotalCart = Convert.ToDecimal(dr["TotalCart"]),
                            TotalDelivery = Convert.ToDecimal(dr["TotalDelivery"]),
                            StatusID = Convert.ToInt32(dr["StatusID"]),
                            InternalStatus = dr["InternalStatus"].ToString(),
                            ExternalStatus = dr["ExternalStatus"].ToString(),
                            ProofPayment = dr["ProofPayment"].ToString(),
                            OrderVerified = Convert.ToBoolean(dr["OrderVerified"]),
                            ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                            InsertDate = Convert.ToDateTime(dr["InsertDate"])
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

        
        public Order OrderDetail(string OrderID)
        {
            Order Details = new Order();

            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspReadOrders]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@OrderID", OrderID);

                //EXEC Command
                using (var dr = SqlCmd.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Details.OrderID = dr["OrderID"].ToString();
                        Details.UserID = Convert.ToInt32(dr["UserID"]);
                        Details.FullName = dr["FullName"].ToString();
                        Details.OrderType = dr["OrderType"].ToString();
                        Details.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                        Details.DeliveryID = Convert.ToInt32(dr["DeliveryID"]);
                        Details.DeliveryAddressID = Convert.ToInt32(dr["DeliveryAddressID"]);
                        Details.FacturationInfoID = Convert.ToInt32(dr["FacturationInfoID"]);
                        Details.OrderDetails = dr["OrderDetails"].ToString();
                        Details.Discount = Convert.ToDecimal(dr["Discount"]);
                        Details.TotalCart = Convert.ToDecimal(dr["TotalCart"]);
                        Details.TotalDelivery = Convert.ToDecimal(dr["TotalDelivery"]);
                        Details.StatusID = Convert.ToInt32(dr["StatusID"]);
                        Details.InternalStatus = dr["InternalStatus"].ToString();
                        Details.ExternalStatus = dr["ExternalStatus"].ToString();
                        Details.ProofPayment = dr["ProofPayment"].ToString();
                        Details.OrderVerified = Convert.ToBoolean(dr["OrderVerified"]);
                        Details.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                        Details.InsertDate = Convert.ToDateTime(dr["InsertDate"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            return Details;
        }

        public bool UpdateOrder(Order Details, string InsertUser)
        {
            bool rpta;
            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspUpdateOrder]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //Insert Parameters
                SqlCmd.Parameters.AddWithValue("@OrderID", Details.OrderID);
                SqlCmd.Parameters.AddWithValue("@InsertUser", InsertUser);
                SqlCmd.Parameters.AddWithValue("@ActionType", Details.ActionType);
                SqlCmd.Parameters.AddWithValue("@StatusID", Details.StatusID);
                SqlCmd.Parameters.AddWithValue("@OrderVerified", Details.OrderVerified);

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

        public List<OrderStatus> Statuses(string StatusType)
        {
            List<OrderStatus> List = new List<OrderStatus>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspReadOrderStatus]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@StatusType", StatusType);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new OrderStatus
                        {
                            StatusID = Convert.ToInt32(dr["StatusID"]),
                            InternalStatus = dr["InternalStatus"].ToString(),
                            ExternalStatusID = Convert.ToInt32(dr["ExternalStatusID"]),
                            ExternalStatus = dr["ExternalStatus"].ToString(),
                            QtyOrders = Convert.ToInt32(dr["QtyOrders"])
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

        public List<OrderStatus> OrderSummary(SearchOrder Details)
        {
            List<OrderStatus> List = new List<OrderStatus>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspReadOrderSummary]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@UserID", Details.UserID);
                SqlCmd.Parameters.AddWithValue("@StartDate", Details.StartDate);
                SqlCmd.Parameters.AddWithValue("@EndDate", Details.EndDate);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new OrderStatus
                        {
                            StatusID = Convert.ToInt32(dr["StatusID"]),
                            InternalStatus = dr["InternalStatus"].ToString(),
                            ExternalStatusID = Convert.ToInt32(dr["ExternalStatusID"]),
                            ExternalStatus = dr["ExternalStatus"].ToString(),
                            QtyOrders = Convert.ToInt32(dr["QtyOrders"])
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

        public List<OrderHistory> OrderHistory(OrderHistoryRequest Model)
        {
            List<OrderHistory> List = new List<OrderHistory>();

            try
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                SqlCon.Open();
                var SqlCmd = new SqlCommand("[sal].[uspReadOrderHistory]", SqlCon)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlCmd.Parameters.AddWithValue("@OrderID", Model.OrderID);
                SqlCmd.Parameters.AddWithValue("@ActivityType", Model.ActivityType);

                using (var dr = SqlCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var detail = new OrderHistory
                        {
                            StatusID = Convert.ToInt32(dr["StatusID"]),
                            InternalStatus = dr["InternalStatus"].ToString(),
                            ExternalStatusID = Convert.ToInt32(dr["ExternalStatusID"]),
                            ExternalStatus = dr["ExternalStatus"].ToString(),
                            OrderVerified = Convert.ToBoolean(dr["OrderVerified"]),
                            ActivityDate = Convert.ToDateTime(dr["ActivityDate"]),
                            InsertUser = dr["InsertUser"].ToString()
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
