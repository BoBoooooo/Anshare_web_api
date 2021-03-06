﻿using api.Secure;
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
        public IHttpActionResult PullPersonAttenListWithToday(string pageSize, string pageNumber, string criteria)
        {
            string sql = "";
            if (criteria != null && criteria != "")
               
                 sql = "select a.*,p.Name,p.No,t.DeptName from Attendance a  join Person p on (a.PersonId = p.ID ) join Dept t on (p.DeptID = t.ID)  and a.Date like '%" + DateTime.Now.ToString("yyyy-MM-dd") + "%' and  (p.Name like '%" + criteria + "%' or p.No like '%" + criteria + "%') ";
            else
            sql = "select a.*,p.Name,p.No,t.DeptName from Attendance a  join Person p on (a.PersonId = p.ID ) join Dept t on (p.DeptID = t.ID)  and a.Date like '%" + DateTime.Now.ToString("yyyy-MM-dd") + "%'";
            Object temp = personmodel.Pagination(sql, pageSize, pageNumber, new PersonModel());
            return Json<dynamic>(temp);
        }
        /// <summary>
        /// 拉取员工详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AttendanceModel PullAttenDetailByPerson(string ID)
        {
            string sql = "select a.*,p.No from Attendance a join Person p on (a.PersonId  = p.ID)  where a.IsDeleted = 0 and a.ID = '"+ID+"'";
            IEnumerable<AttendanceModel> temp = attendancemodel.SqlQuery(sql);
            if (temp.Count() > 0)
            {
                return temp.FirstOrDefault();

            }

            else {
                return new AttendanceModel();

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
        public void UpdateAtten(AttendanceModel temp)
        {
            string sql = "select * from Person where No =" + temp.No;

            Person p = person.SqlQuery(sql).FirstOrDefault();


            Attendance aaa = new Attendance();
            aaa.ID = temp.ID;
            aaa.StartTime = temp.StartTime;
            aaa.EndTime = temp.EndTime;
            aaa.IsDeleted = temp.IsDeleted;
            aaa.PersonId = p.ID;
            aaa.Vacation = temp.Vacation;
            aaa.Vacation_Reason = temp.Vacation_Reason;
            aaa.Date = temp.Date;
            attendance.Update(aaa);



        }
        /// <summary>
        /// api接口
        /// </summary>
        /// <param name="temp"></param>
        public Result SaveAtten(AttendanceModel temp)
        {


            string sql2 = "select * from Person where No =" + temp.No;

            Person p = person.SqlQuery(sql2).FirstOrDefault();



            string sql = "select * from Attendance   where IsDeleted = 0 and PersonId= '" + p.ID + "' and Date like '%" + DateTime.Now.ToString("yyyy-MM-dd") + "%'";
            IEnumerable<Attendance> u = attendance.SqlQuery(sql);
            try
            {
                if (u.Count() == 0)
                {

                    Attendance aaa = new Attendance();
                    aaa.ID = Guid.NewGuid();
                    aaa.StartTime = temp.StartTime;
                    aaa.EndTime = temp.EndTime;
                    aaa.IsDeleted = temp.IsDeleted;
                    aaa.PersonId = p.ID;
                    aaa.Vacation = temp.Vacation;
                    aaa.Vacation_Reason = temp.Vacation_Reason;
                    aaa.Date = temp.Date;
                    NewAtten(aaa);
                    return new Result(true, "新增成功", null);
                }
                else
                {

                    Attendance aaa = new Attendance();
                    aaa.ID = temp.ID;
                    aaa.StartTime = temp.StartTime;
                    aaa.EndTime = temp.EndTime;
                    aaa.IsDeleted = temp.IsDeleted;
                    aaa.PersonId = p.ID;
                    aaa.Vacation = temp.Vacation;
                    aaa.Vacation_Reason = temp.Vacation_Reason;
                    aaa.Date = temp.Date;


                    temp.ID = u.FirstOrDefault().ID;
                    attendance.Update(aaa);
                    return new Result(true, "更新成功", null);
                }
            }
            catch (Exception e)
            {
                return new Result(false, e.Message, null);
            }
        }

        public IHttpActionResult PullPersonAttenByMonth(int No, int month) {
            Person per = person.FindByNo(No);
            string start = "2018-" + month.ToString() + "-01";
            string end = "2018-" + month.ToString() + "-31";

            if (per != null)
            {
                string sql = "select * from Attendance where IsDeleted = 0 and PersonId = '" + per.ID + "' and Date >= " + start + " and Date <=" + end;
                var temp = new { };
                return Json<dynamic>(temp);
            }
            else {
                var error = new { Success = false, Message = "工号不存在" };
                            return Json<dynamic>(error);

            }

        }
        /// <summary>
        /// 查询个人月报
        /// </summary>
        /// <param name="name"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public IHttpActionResult SearchPersonByMonth(string name ,string month) {
            string sql = "select a.*,p.Name,p.No from Attendance a join Person p on (a.PersonId = p.ID) where  a.IsDeleted =0  and  a.Date like '%"+month+"%' and (p.Name like '%"+name+"%' or p.No like '%"+name+"%')";

            int ealry_later = 0; int vaction = 0; int normal = 0;

            IEnumerable<AttendanceModel> temp =attendancemodel.SqlQuery(sql);
            foreach (AttendanceModel a in temp) {
                if (a.Vacation == 1) {
                    vaction++;
                }
                else {
                    if (string.Compare(a.EndTime, "17:30:00") < 0 || string.Compare(a.StartTime, "8:30:00") > 0)
                    {
                        ealry_later++;
                        a.Vacation = 2;
                    }         
                   else{
                        normal++;
                    }
                }
            }
            foreach (AttendanceModel a in temp)
            {
                a.vaction = vaction.ToString();
                a.normal = normal.ToString();
                a.ealry_later = ealry_later.ToString();
                
            }
    

            return Json<dynamic>(temp);
        }



        public IHttpActionResult SearchPersonByYear(string name)
        {
            string sql = "select a.*,p.Name,p.No from Attendance a join Person p on (a.PersonId = p.ID) where  a.IsDeleted =0  and  a.Date like '%2018%' and (p.Name like '%" + name + "%' or p.No like '%" + name + "%')";

            int ealry_later = 0; int vaction = 0; int normal = 0;

            IEnumerable<AttendanceModel> temp = attendancemodel.SqlQuery(sql);
            foreach (AttendanceModel a in temp)
            {
                if (a.Vacation == 1)
                {
                    vaction++;
                }
                else
                {
                    if (string.Compare(a.EndTime, "17:30:00") < 0 || string.Compare(a.StartTime, "8:30:00") > 0)
                    {
                        ealry_later++;
                        a.Vacation = 2;
                    }
                    else
                    {
                        normal++;
                    }
                }
            }
            foreach (AttendanceModel a in temp)
            {
                a.vaction = vaction.ToString();
                a.normal = normal.ToString();
                a.ealry_later = ealry_later.ToString();

            }


            return Json<dynamic>(temp);
        }

    }
}
