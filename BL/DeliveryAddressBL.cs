using System.Collections.Generic;
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

        public bool Update(DeliveryAddress Detail, string InsertUser)
        {
            return DADAL.Update(Detail, InsertUser);
        }

        public bool AddNew(DeliveryAddress Detail, string InsertUser)
        {
            return DADAL.AddNew(Detail, InsertUser);
        }
    }
}
