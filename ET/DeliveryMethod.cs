using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class DeliveryMethod
    {
        [Key]
        public int DeliveryID { get; set; }

        public string Method { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
