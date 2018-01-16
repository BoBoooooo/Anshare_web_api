using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
namespace BLL
{
    public class DeptService : BaseService<Dept>, IDeptService
    {
        public Dept FindByID(Guid ID)
        {
            using (var db = new DBEntities())
            {
                var temp = from a in db.Dept where a.ID == ID select a;
                return temp.FirstOrDefault();
            }
        }

        public Dept FindByDeptName(string DeptName)
        {
            using (var db = new DBEntities())
            {
                var temp = from a in db.Dept where a.DeptName == DeptName select a;
                return temp.FirstOrDefault();
            }
        }
    }
}
