using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class FacturatioInfo
    {
        [Key]
        public int FacturationInfoID { get; set; }

        public int UserID { get; set; }

        public string IdentityType { get; set; }

        public string IdentityID { get; set; }

        public string FullName { get; set; }

        public int PhoneNumber { get; set; }

        public string Email { get; set; }

        public int CostaRicaID { get; set; }

        public int ProvinceID { get; set; }

        public string Province { get; set; }

        public int CantonID { get; set; }

        public string Canton { get; set; }

        public int DistrictID { get; set; }

        public string District { get; set; }

        public string Street { get; set; }        

        public bool PrimaryFlag { get; set; }

        public string ActionType { get; set; }
    }
}
