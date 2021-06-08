using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ET;
using DAL;

namespace BL
{
    public class PrimaryProductsBL
    {
        private PrimaryProductsDAL PPDAL = new PrimaryProductsDAL();

        public IEnumerable<PrimaryProduct> List()
        {
            return PPDAL.List();
        }

        public PrimaryProduct Details (int PrimaryProductID)
        {
            return PPDAL.Details(PrimaryProductID);
        }

        public List<Formula> Formula(int PrimaryProductID)
        {
            return PPDAL.Formula(PrimaryProductID);
        }

        public bool AddFormula(PrimaryProduct Detail, string InsertUser)
        {
            return PPDAL.AddFormula(Detail, InsertUser);
        }

        public bool UpdateFormula(PrimaryProduct Detail, string InsertUser)
        {
            return PPDAL.UpdateFormula(Detail, InsertUser);
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
