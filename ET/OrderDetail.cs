using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class OrderDetail
    {
        [Key]
        public int DetailID { get; set; }

        public int OrderID { get; set; }

        public int ProductID { get; set; }

        public int Qty { get; set; }

        public string ActionType { get; set; }
    }
}
