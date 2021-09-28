using System.Collections.Generic;
using DAL;
using ET;

namespace BL
{
    public class CostaRicaDataBL
    {
        private CostaRicaDataDAL CRDAL = new CostaRicaDataDAL();

        public List<CostaRicaProvinces> Provinces()
        {
            return CRDAL.Provinces();
        }
        public List<CostaRicaCantons> Cantons()
        {
            return CRDAL.Cantons();
        }
        public List<CostaRicaDistrics> Districts()
        {
            return CRDAL.Districts();
        }
    }
}
