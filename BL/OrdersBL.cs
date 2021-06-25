using ET;
using DAL;

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
    }
}
