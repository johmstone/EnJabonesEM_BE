using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public bool ActiveFlag { get; set; }
    }
}
