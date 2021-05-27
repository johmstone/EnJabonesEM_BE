using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class CostaRicaDistrics
    {
        public int CostaRicaID { get; set; }

        public int DistrictID { get; set; }

        public string District { get; set; }
    }

    public class CostaRicaCantons
    {
        public int CantonID { get; set; }

        public string Canton { get; set; }

        //public List<CostaRicaDistrics> Districts { get; set; }
    }


    public class CostaRicaProvinces
    {
        public int ProvinceID { get; set; }

        public string Province { get; set; }

        //public List<CostaRicaCantons> Cantons { get; set; }
    }

}
