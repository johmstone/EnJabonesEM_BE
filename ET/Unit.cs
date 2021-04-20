using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Unit
    {
        [Key]
        public int UnitID { get; set; }

        public string UnitName { get; set; }

        public string Symbol { get; set; }

        public string ActionType { get; set; }
    }
}
