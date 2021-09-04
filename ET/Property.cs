using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Property
    {
        [Key]
        [JsonProperty("PropertyID")]
        public int PropertyID { get; set; }

        [JsonProperty("ProductPropertyID")]
        public int ProductPropertyID { get; set; }

        [JsonProperty("PropertyName")]
        public string PropertyName { get; set; }

        [JsonProperty("ActionType")]
        public string ActionType { get; set; }
    }
}
