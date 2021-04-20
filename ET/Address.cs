using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class Address
    {
        [Key]
        public int AddressID { get; set; }

        public string Province { get; set; }

        public string Canton { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public int PhoneNumber { get; set; }

        public string Notes { get; set; }

        public bool ActiveFlag { get; set; }

        public string ActionType { get; set; }
    }
}
