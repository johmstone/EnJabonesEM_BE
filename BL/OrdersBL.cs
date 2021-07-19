using ET;
using DAL;
using System.Collections.Generic;

namespace BL
{
    public class OrdersBL
    {
        private OrdersDAL ODAL = new OrdersDAL();

        public string AddStagingOrder(StagingOrder Details)
        {
            return ODAL.AddStagingOrder(Details);
        }

        public StagingOrder SearchStaginOrder(string StagingOrderID)
        {
            return ODAL.SearchStaginOrder(StagingOrderID);
        }

        public bool AddOrder(NewOrder Order)
        {
            return ODAL.AddOrder(Order);
        }

        public bool UpdateOrder(Order Model, string InsertUser)
        {
            return ODAL.UpdateOrder(Model, InsertUser);
        }

        public List<Order> OrderList(SearchOrder Details)
        {
            return ODAL.OrderList(Details);
        }

        public List<OrderStatus> OrderSummary(SearchOrder Details)
        {
            return ODAL.OrderSummary(Details);
        }

        public Order OrderDetails(string OrderID)
        {
            return ODAL.OrderDetail(OrderID);
        }

        public List<OrderStatus> Statuses(string StatusType)
        {
            return ODAL.Statuses(StatusType);
        }
    }
}
