using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class OrderStatus
    {
        [Key]
        public int StatusID { get; set; }

        public string InternalStatus { get; set; }

        public int ExternalStatusID { get; set; }

        public string ExternalStatus { get; set; }
    }
}
