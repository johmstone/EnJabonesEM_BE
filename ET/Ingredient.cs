using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Ingredient
    {
        [Key]
        public int IngredientID { get; set; }

        public string IngredientName { get; set; }

        public int TypeID { get; set; }

        public string PhotoURL { get; set; }

        public string ActionType { get; set; }
    }
}
