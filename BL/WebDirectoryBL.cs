using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class WebDirectoryBL
    {
        private WebDirectoryDAL WDAL = new WebDirectoryDAL();

        public List<WebDirectory> Menu(WebDirectoryRequest Model)
        {
            return WDAL.Menu(Model);
        }
    }
}
