using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ET
{
    public class Unit
    {
        [Key]
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
