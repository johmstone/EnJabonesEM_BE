using System.Collections.Generic;
using ET;
using DAL;

namespace BL
{
    public class RolesBL
    {
        private RolesDAL RDAL = new RolesDAL();

        public List<Role> List()
        {
            return RDAL.List();
        }

        public bool AddNew(Role detail, string insertuser)
        {
            return RDAL.AddNew(detail, insertuser);
        }
    }
}
