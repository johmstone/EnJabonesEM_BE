using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ET
{
    public class Product
    {
        [Key]
        [JsonProperty("ProductID")]
        public int ProductID { get; set; }

        [JsonProperty("PrimaryProductID")]
        public int PrimaryProductID { get; set; }

        [JsonProperty("Qty")]
        public decimal Qty { get; set; }

        [JsonProperty("UnitID")]
        public int UnitID { get; set; }

        [JsonProperty("UnitName")]
        public string UnitName { get; set; }

        [JsonProperty("Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [JsonProperty("IVA")]
        public decimal IVA { get; set; }

        [JsonProperty("Discount")]
        public decimal Discount { get; set; }

        [JsonProperty("ActiveFlag")]
        public bool ActiveFlag { get; set; }

        [JsonProperty("VisibleFlag")]
        public bool VisibleFlag { get; set; }

        [JsonProperty("ActionType")]
        public string ActionType { get; set; }

        

    }
}
