using System;
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
    }

    public class SearchOrder
    {
        public string OrderID { get; set; }

        public int UserID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
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

        public string Status { get; set; }

        public string ProofPayment { get; set; }

        public bool OrderVerified { get; set; }

        public bool ActiveFlag { get; set; }

        public DateTime InsertDate { get; set; }

        public string ActionType { get; set; }
    }
}
