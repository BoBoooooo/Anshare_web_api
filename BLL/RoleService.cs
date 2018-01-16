using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
namespace BLL
{
    public class RoleService : BaseService<Role>, IRoleService
    {

        public Role FindAuthByID(Guid ID)
        {
            using (var db = new DBEntities())
            {
                var temp = from a in db.Role where a.ID == ID select a;
                return temp.FirstOrDefault();
            }
        }

        public Role FindByRoleName(string RoleName)
        {
            using (var db = new DBEntities())
            {
                var temp = from a in db.Role where a.RoleName == RoleName select a;
                return temp.FirstOrDefault();
            }
        }
    }
}
