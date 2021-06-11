using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
using DAL;

namespace BL
{
    public class ProductsBL
    {
        private ProductsDAL PDAL = new ProductsDAL();

        public IEnumerable<PrimaryProduct> List()
        {
            return PDAL.List();
        }

        public bool AddNew(Product product, string insertuser)
        {
            return PDAL.AddNew(product, insertuser);
        }

        public bool Update(Product product, string insertuser)
        {
            return PDAL.AddNew(product, insertuser);
        }

    }
}
