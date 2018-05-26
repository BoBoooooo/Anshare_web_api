using api.Secure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;
using IBLL;
using api.Models;
using Model;
namespace api.Controllers
{
    [ApiAuthorize]

    public class PersonController : ApiController
    {
        IPersonService person = new PersonService();
        IPersonModelService personmodel = new PersonModelService();


        /// <summary>
        /// 拉取员工列表
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult PullUserList(string pageSize,string pageNumber,string criteria)
        {
            string sql = "";

            if (criteria!=null&&criteria!="")
                sql = "select u.*,t.DeptName from Person u join Dept t on (u.DeptID = t.ID) where u.IsDeleted = 0 and  (u.Name like '%" + criteria + "%' or u.No like '%" + criteria + "%') order by u.No asc";
            else
                sql = "select u.*,t.DeptName from Person u join Dept t on (u.DeptID = t.ID) where u.IsDeleted = 0 order by u.No asc";
            Object  temp =   personmodel.Pagination(sql,pageSize,pageNumber,new PersonModel());
            return Json<dynamic>(temp);
        }
        /// <summary>
        /// 拉取员工详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public PersonModel PullUsersDetail(string ID)
        {
            IEnumerable<PersonModel> temp = personmodel.SqlQuery("select u.*,t.DeptName from Person u join Dept t on (u.DeptID = t.ID) and u.ID = '" + ID + "' where u.IsDeleted = 0");
            return temp.FirstOrDefault();
        }
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Result DelUser(string ID)
        {
            person.Delete(new Guid(ID));

            return new Result(true, "删除成功", null);
        }
        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public Result SaveNewUsers(Person temp)
        {
            try
            {
                Person u = person.FindByNo(temp.No);
                if (u == null)
                {

                    temp.ID = Guid.NewGuid();
                    temp.IsDeleted = false;
                    person.Add(temp);
                    return new Result(true, "新增成功", null);
                }

                else
                {

                    return new Result(false, "工号已存在", null);

                }


            }

            catch (Exception e)
            {
                return new Result(false, e.Message, null);
            }

        }
        /// <summary>
        /// 更新员工
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public Result UpdateUsers(Person temp)
        {
            try
            {
                person.Update(temp);
                return new Result(true, "更新成功", null);
            }
            catch (Exception e)
            {
                return new Result(false, e.Message, null);
            }

        }

    }

}