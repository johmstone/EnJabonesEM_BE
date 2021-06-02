using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class PrimaryProductsBL
    {
        private PrimaryProductsDAL PPDAL = new PrimaryProductsDAL();

        public List<PrimaryProduct> List()
        {
            return PPDAL.List();
        }

        public bool Update(PrimaryProduct Detail, string InsertUser)
        {
            return PPDAL.Update(Detail, InsertUser);
        }

        public bool AddNew(PrimaryProduct Detail, string InsertUser)
        {
            return PPDAL.AddNew(Detail, InsertUser);
        }
    }
}
