using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class DeliveryAddress
    {
        [Key]
        public int DeliveryAddressID { get; set; }

        public int UserID { get; set; }

        public int AddressID { get; set; }

        public string ContactName { get; set; }

        public bool PrimaryFlag { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
