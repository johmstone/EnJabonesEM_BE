﻿using System.Collections.Generic;
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
        public List<CostaRicaCantons> Cantons(int ProvinceID)
        {
            return CRDAL.Cantons(ProvinceID);
        }
        public List<CostaRicaDistrics> Districts(int CantonID, int ProvinceID)
        {
            return CRDAL.Districts(CantonID, ProvinceID);
        }
    }
}
