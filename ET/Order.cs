﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ET
{

    public class StagingOrder
    {
        public string StagingOrderID { get; set; }

        public int UserID { get; set; }

        public int DeliveryID { get; set; }

        public string OrderDetails { get; set; }

        public DateTime OrderDate { get; set; }
    }

    public class NewOrder
    {
        public string OrderID { get; set; }

        public string OrderDetails { get; set; }

        public string EmailNotification { get; set; }

        public string PaymentMethod { get; set; }

        public string ProofPayment { get; set; }
    }

    public class SearchOrder
    {
        public string OrderID { get; set; }

        public int? UserID { get; set; }

        public int? ExternalStatusID { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class Order
    {
        [Key]
        public string OrderID { get; set; }

        public int UserID { get; set; }

        public string FullName { get; set; }

        public string OrderType { get; set; }

        public DateTime OrderDate { get; set; }

        public int DeliveryID { get; set; }

        public int DeliveryAddressID { get; set; }

        public int FacturationInfoID { get; set; }

        public string OrderDetails { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalCart { get; set; }

        public decimal TotalDelivery { get; set; }

        public int StatusID { get; set; }

        public string InternalStatus { get; set; }

        public string ExternalStatus { get; set; }

        public string ProofPayment { get; set; }

        public bool OrderVerified { get; set; }

        public bool ActiveFlag { get; set; }

        public DateTime InsertDate { get; set; }

        public string ActionType { get; set; }
    }
    
    public class OrderList
    {
        public List<Order> Orders { get; set; }

        public List<OrderStatus> Summary { get; set; }
    }
    public class OrderConfirmationRequest
    {
        [JsonProperty("ApiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("OrderID")]
        public string OrderID { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }
    }
    public class OrderConfirmationRequestEncrypted
    {
        public string Data { get; set; }
    }

    public class OrderHistory
    {
        public int StatusID { get; set; }

        public string InternalStatus { get; set; }

        public int ExternalStatusID { get; set; }

        public string ExternalStatus { get; set; }

        public bool OrderVerified { get; set; }

        public DateTime ActivityDate { get; set; }

        public string InsertUser { get; set; }
    }

    public class OrderHistoryRequest
    {
        public string OrderID { get; set; }

        public string ActivityType { get; set; }
    }
}
