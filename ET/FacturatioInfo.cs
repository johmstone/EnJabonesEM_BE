using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class FacturatioInfo
    {
        [Key]
        public int FacturationInfoID { get; set; }

        public int UserID { get; set; }

        public string IdentityType { get; set; }

        public string FullName { get; set; }

        public int AddressID { get; set; }

        public bool PrimaryFlag { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
