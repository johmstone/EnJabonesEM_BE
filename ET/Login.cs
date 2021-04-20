using System;
using System.ComponentModel.DataAnnotations;

namespace ET
{
    public class Login
    {
        [Key]
        public int LoginID { get; set; }

        public int UserID { get; set; }

        public string FullName { get; set; }

        public string IP { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public DateTime LoginDate { get; set; }
    }
}
