using System;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Token
    {
        [Key]
        public string TokenID { get; set; }

        public int UserID { get; set; }

        public DateTime ExpiresDate { get; set; }
    }
}
