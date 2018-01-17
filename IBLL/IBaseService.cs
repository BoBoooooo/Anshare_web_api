using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
using System.Data;
using System.Data.Entity;

namespace IBLL
{
    public interface IBaseService<T> where T : class, new()
    {

        void Add(T item);
        void Update(T item);
        void Delete(Guid guid);
        void Delete(T item);
        void SqlCommand(string sql);
        T Find(Guid guid);
        IEnumerable<T> SqlQuery(string sql);
        DataTable SqlTable(Database database, string sql);

        Object Pagination(string sql, string pageSize, string pageIndex, T item);

    }
}
