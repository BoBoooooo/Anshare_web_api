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
    public class AttendanceController : ApiController
    {
        AttendanceService attendance = new AttendanceService();
        IPersonModelService personmodel = new PersonModelService();
        IPersonService person = new PersonService();

        IAttendanceModelService attendancemodel = new AttendanceModelService();

        /// <summary>
        /// 拉取员工当日考勤列表
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult PullPersonAttenListWithToday(string pageSize,string pageNumber)
        {
            string sql = "select a.*,p.Name,p.No,t.DeptName from Attendance a  join Person p on (a.PersonId = p.ID ) join Dept t on (p.DeptID = t.ID)  and a.Date like '"+ DateTime.Now.ToString("yyyy-MM-dd") + "'";
            Object temp = personmodel.Pagination(sql, pageSize, pageNumber, new PersonModel());
            return Json<dynamic>(temp);
        }
        /// <summary>
        /// 拉取员工详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Attendance PullAttenDetailByPerson(string ID)
        {
            string sql = "select a.* from Attendance a  where a.IsDeleted = 0 and a.PersonId in (select ID from Person where IsDeleted = 0 and ID  = '" + ID + "')";
            IEnumerable<Attendance> temp = attendance.SqlQuery(sql);
            if (temp.Count() > 0)
            {
                return temp.FirstOrDefault();

            }

            else {
                return new Attendance();

            }
        }
        /// <summary>
        /// 删除员工当日考勤信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Result DelAtten(string ID)
        {
            attendance.Delete(new Guid(ID));

            return new Result(true, "删除成功", null);
        }
        /// <summary>
        /// 新增员工今日考勤信息
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public void NewAtten(Attendance temp)
        {

                    temp.ID = Guid.NewGuid();
                    temp.IsDeleted = false;
                    attendance.Add(temp);
      

        }
        /// <summary>
        /// 更新员工当日考勤信息
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public void UpdateAtten(Attendance temp)
        {
         
                attendance.Update(temp);
            
          

        }
        /// <summary>
        /// api接口
        /// </summary>
        /// <param name="temp"></param>
        public Result SaveAtten(Attendance temp)
        {
            string sql = "select * from Attendance   where IsDeleted = 0 and PersonId= '" + temp.PersonId + "' and Date like '%" + DateTime.Now.ToString("yyyy-MM-dd") + "%'";
            IEnumerable<Attendance> u = attendance.SqlQuery(sql);
            try
            {
                if (u.Count() == 0)
                {
                    NewAtten(temp);
                    return new Result(true, "新增成功", null);
                }
                else
                {
                    temp.ID = u.FirstOrDefault().ID;
                    attendance.Update(temp);
                    return new Result(true, "更新成功", null);
                }
            }
            catch (Exception e)
            {
                return new Result(false, e.Message, null);
            }
        }

        public IHttpActionResult PullPersonAttenByMonth(int No,int month) {
          Person per =  person.FindByNo(No);
            string start ="2018-"+ month.ToString() +"-01";
            string end = "2018-" + month.ToString() + "-31";

            if (per != null)
            {
                string sql = "select * from Attendance where IsDeleted = 0 and PersonId = '"+per.ID+ "' and Date >= "+start+" and Date <="+end;
                var temp = new { };
                return Json<dynamic>(temp);
            }
            else {
                var error = new { Success = false, Message="工号不存在"};.
                            return Json<dynamic>(error);

            }
     
        }

    }

}