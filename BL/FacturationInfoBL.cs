using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class FacturationInfoBL
    {
        private FacturationInfoDAL FDAL = new FacturationInfoDAL();

        public List<FacturatioInfo> List(int UserID)
        {
            return FDAL.List(UserID);
        }

        public bool Update(FacturatioInfo Detail, string InsertUser)
        {
            return FDAL.Update(Detail, InsertUser);
        }

        public bool AddNew(FacturatioInfo Detail, string InsertUser)
        {
            return FDAL.AddNew(Detail, InsertUser);
        }
    }
}
