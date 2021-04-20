using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Use
    {
        [Key]
        public int UseID { get; set; }

        public string UseName { get; set; }

        public string ActionType { get; set; }
    }
}
