using System;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public string FullName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool EmailValidated { get; set; }

        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        public bool NeedResetPwd { get; set; }

        public bool Subscriber { get; set; }

        public bool ActiveFlag { get; set; }

        public DateTime LastActivityDate { get; set; }

        public string ActionType { get; set; }
    }
}
