using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class IngredientUse
    {
        [Key]
        public int IngUseID { get; set; }

        public int IngredientID { get; set; }

        public int UseID { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
