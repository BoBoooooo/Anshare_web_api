using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Model;
namespace api.Controllers
{
    public class DeptComboDB
    {
        static DBEntities db = new DBEntities();

        public static IList<Tree> returnParentTreeModel()
        {

            IList<Tree> t = new List<Tree>();
            IEnumerable<Tree> temp = null;
            string sql = "SELECT Dept.ID AS id ,Dept.DeptName AS text,'00000000-0000-0001-0000-000000000000' AS parentID,NULL AS checked FROM  Dept   WHERE  ParentId='00000000-0000-0000-0000-000000000000'and IsDeleted=0";
            //string sql = "SELECT * FROM (SELECT Dept.ID as id ,DeptName as text,parentId,r.checked FROM Dept left join (select DeptID AS checked From UserDept WHERE UserID='" + ID + "') AS r ON Dept.Id=r.checked) AS s WHERE  parentId='00000000-0000-0000-0000-000000000000'";
            temp = db.Database.SqlQuery<Tree>(sql);
            return temp.ToList();
        }

        public static bool isHaveChild(Guid id)
        {
            bool flag = false;
            IEnumerable<Guid> temp = null;
            string sql = "select id from Dept where parentId='" + id + "'";
            temp = db.Database.SqlQuery<Guid>(sql);
            if (temp.FirstOrDefault() != null)
            {
                flag = true;
            }
            return flag;

        }
        public static IList<Tree> getChild(Guid pid)
        {
            IList<Tree> t = new List<Tree>();
            IEnumerable<Tree> temp = null;
            string sql = "SELECT Dept.ID AS id,DeptName AS text,parentID,Null AS checked FROM  Dept  WHERE  ParentId='" + pid + "'and IsDeleted=0 order by Rank asc";
            //string sql = "SELECT * FROM (SELECT ID as id ,DeptName as text,parentId,r.checked FROM Dept left join (select DeptID AS checked From UserDept WHERE UserID='" + ID + "') AS r ON Dept.Id=r.checked) AS s WHERE  parentId='" + pid + "'";
            temp = db.Database.SqlQuery<Tree>(sql);
            return temp.ToList();
        }
    }
}