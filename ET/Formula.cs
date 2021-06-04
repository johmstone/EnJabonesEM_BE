using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ET
{
    public class Formula
    {
        [Key]
        [JsonProperty("FormulaID")]
        public int FormulaID { get; set; }

        [JsonProperty("PrimaryProductID")]
        public int PrimaryProductID { get; set; }

        [JsonProperty("IngredientID")]
        public int IngredientID { get; set; }

        [JsonProperty("IngredientName")]
        public string IngredientName { get; set; }

        [JsonProperty("TypeName")]
        public string TypeName { get; set; }

        [JsonProperty("Qty")]
        public decimal Qty { get; set; }

        [JsonProperty("UnitID")]
        public int UnitID { get; set; }

        [JsonProperty("UnitName")]
        public string UnitName { get; set; }

        [JsonProperty("Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("ActionType")]
        public string ActionType { get; set; }
    }
}
