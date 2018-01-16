using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace IBLL
{
    public interface IPersonService : IBaseService<Person>
    {
        Person FindByName(string Name);
        Person FindByNo(Nullable<int> No);

    }
}
