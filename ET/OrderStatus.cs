using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class OrderStatus
    {
        [Key]
        public int StatusID { get; set; }

        public string Status { get; set; }

        public string ActionType { get; set; }
    }
}
