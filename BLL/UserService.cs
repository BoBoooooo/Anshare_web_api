using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
namespace BLL
{
    public class UserService : BaseService<Users>, IUserService
    {
        public Users FindByUserName(string userName)
        {
            using (var db = new DBEntities())
            {
                var temp = from a in db.Users where a.UserName == userName select a;
                return temp.FirstOrDefault();
            }
        }
     
    }
}
