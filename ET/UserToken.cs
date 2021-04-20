using System;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class UserToken
    {
        [Key]
        public string Token { get; set; }

        public int UserID { get; set; }

        public DateTime ExpiresDate { get; set; }

        public string ActionType { get; set; }
    }
}
