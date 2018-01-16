using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
namespace BLL
{
    public class PersonService : BaseService<Person>, IPersonService
    {
        public Person FindByName(string Name)
        {
            using (var db = new DBEntities())
            {
                var temp = from a in db.Person where a.Name == Name select a;
                return temp.FirstOrDefault();
            }
        }
        public Person FindByNo(Nullable<int> No)
        {
            using (var db = new DBEntities())
            {
                var temp = from a in db.Person where a.No == No select a;
                return temp.FirstOrDefault();
            }
        }
    }
}
