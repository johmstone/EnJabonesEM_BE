using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Formula
    {
        [Key]
        public int FormulaID { get; set; }

        public int PrimeryProductID { get; set; }

        public int IngredientID { get; set; }

        public decimal Qty { get; set; }

        public int UnitID { get; set; }

        public string ActionType { get; set; }
    }
}
