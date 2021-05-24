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

        public AccessRights ValidationRights(AccessRightsRequest model)
        {
            return WDAL.ValidationRights(model);
        }

        public List<WebDirectory> List(int AppID)
        {
            return WDAL.List(AppID);
        }

        public bool AddNew(WebDirectory detail, string insertuser)
        {
            return WDAL.AddNew(detail, insertuser);
        }

        public bool Update(WebDirectory detail, string insertuser)
        {
            return WDAL.Update(detail, insertuser);
        }
    }
}
