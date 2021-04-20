using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class RightsbyRole
    {
        [Key]
        public int RightID { get; set; }

        public int WebID { get; set; }

        public int RoleID { get; set; }

        public bool Read { get; set; }

        public bool Write { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
