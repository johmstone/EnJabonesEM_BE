using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class ResetPassword
    {
        [Key]
        public int RSID { get; set; }

        public string GUID { get; set; }

        public int UserID { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
