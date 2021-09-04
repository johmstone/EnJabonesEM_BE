using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class PropertiesBL
    {
        private PropertiesDAL PDAL = new PropertiesDAL();

        public List<Property> List()
        {
            return PDAL.List();
        }

        public bool Disable(int ProductPropertyID, string InsertUser)
        {
            return PDAL.Disable(ProductPropertyID, InsertUser);
        }
    }
}
