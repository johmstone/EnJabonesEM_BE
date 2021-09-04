using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ET
{
    public class PrimaryProduct
    {
        [Key]
        [JsonProperty("PrimaryProductID")]
        public int PrimaryProductID { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Technique")]
        public string Technique { get; set; }

        [JsonProperty("PhotoURL")]
        public string PhotoURL { get; set; }

        [JsonProperty("BrochureURL")]
        public string BrochureURL { get; set; }

        [JsonProperty("StrProperties")]
        public string StrProperties { get; set; }

        [JsonProperty("ActiveFlag")]
        public bool ActiveFlag { get; set; }

        [JsonProperty("VisibleFlag")]
        public bool VisibleFlag { get; set; }

        [JsonProperty("ActionType")]
        public string ActionType { get; set; }

        [JsonProperty("Products")]
        public List<Product> Products { get; set; }

        [JsonProperty("Formula")]
        public List<Formula> Formula { get; set; }

        [JsonProperty("Properties")]
        public List<Property> Properties { get; set; }
    }

    
}
