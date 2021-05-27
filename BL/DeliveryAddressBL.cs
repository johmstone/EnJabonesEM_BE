using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
using DAL;

namespace BL
{
    public class DeliveryAddressBL
    {
        private DeliveryAddressDAL DADAL = new DeliveryAddressDAL();

        public List<DeliveryAddress> List(int UserID)
        {
            return DADAL.List(UserID);
        }
    }
}
