using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class IngredientProperty
    {
        [Key]
        public int IngPropertyID { get; set; }

        public int IngredientID { get; set; }

        public int PropertyID { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
