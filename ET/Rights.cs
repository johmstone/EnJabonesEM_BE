using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Rights
    {
        [Key]
        public int RightID { get; set; }

        public int WebID { get; set; }

        public int RoleID { get; set; }

        public bool ReadRight { get; set; }

        public bool WriteRight { get; set; }

        public string ActionType { get; set; }
    }

    public class AccessRights
    {
        public bool ReadRight { get; set; }

        public bool WriteRight { get; set; }
    }

    public class AccessRightsRequest
    {
        public int UserID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
