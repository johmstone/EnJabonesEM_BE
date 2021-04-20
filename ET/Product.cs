using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public int PrimaryProductID { get; set; }

        public decimal Qty { get; set; }

        public int UnitID { get; set; }

        public decimal Price{ get; set; }

        public decimal IVA { get; set; }

        public bool ActiveFlag { get; set; }

        public bool VisibleFlag { get; set; }

        public string ActionType { get; set; }
    }
}
