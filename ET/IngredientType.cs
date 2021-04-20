using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class IngredientType
    {
        [Key]
        public int TypeID { get; set; }

        public string TypeName { get; set; }

        public string ActionType { get; set; }
    }
}
