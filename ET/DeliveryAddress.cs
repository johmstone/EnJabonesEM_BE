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

        public int CostaRicaID { get; set; }

        public int ProvinceID { get; set; }

        public string Province { get; set; }

        public int CantonID { get; set; }

        public string Canton { get; set; }

        public int DistrictID { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public int PhoneNumber { get; set; }

        public string Notes { get; set; }

        public bool PrimaryFlag { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
