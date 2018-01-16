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

    public class AdminController : ApiController
    {
        IUsersModelService usersmodel = new UsersModelService();
        IUserService user = new UserService();
        IRoleService role = new RoleService();
        IDeptService dept = new DeptService();

        /// <summary>
        /// 拉取用户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public AuthInfo UserInfo([FromBody]GetUserInfo obj)
        {
            if (obj.auth != null)
            {
                string secureKey = System.Configuration.ConfigurationManager.AppSettings["SecureKey"];
                AuthInfo authInfo = JWT.JsonWebToken.DecodeToObject<AuthInfo>(obj.auth, secureKey);
                if (authInfo != null)
                {
                    string username = authInfo.UserName;
                    Users temp = user.FindByUserName(username); //找到token对应的用户
                    if (temp != null)
                    {
                        Role r = role.FindAuthByID(new Guid(temp.RoleID.ToString()));//找到用户对应的role auth列表
                        if (r != null)
                        {
                            authInfo.Roles = r.RoleAuthName.Split(',').ToList();
                            authInfo.RealName = temp.RealName;
                        }

                    }
                }
                return authInfo;
            }

            return new AuthInfo();

        }

        public Result ChangePassword([FromBody]LoginRequest obj)
        {
            Users u = user.FindByUserName(obj.UserName);
            if (u != null)
            {
                if (u.Password != obj.Password)
                {

                    u.Password = obj.Password;
                    user.Update(u);
                    return new Result(true, "修改成功", null);
                }

                else
                {
                    return new Result(false, "新密码与原密码相同", null);
                }
            }

            else
            {
                return new Result(false, "用户名不存在", null);
            }

        }

        /// <summary>
        /// 拉取用户列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UsersModel> PullUserList()
        {
            IUsersModelService usersmodel = new UsersModelService();

            IEnumerable<UsersModel> temp = usersmodel.SqlQuery("select u.*,r.RoleName,t.DeptName from Users u join Role r on (u.RoleID = r.ID) join Dept t on (u.DeptID = t.ID) where u.IsDeleted = 0 order by u.Rank asc");


            return temp;
        }
        /// <summary>
        /// 拉取用户详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public UsersModel PullUsersDetail(string ID)
        {
            IEnumerable<UsersModel> temp = usersmodel.SqlQuery("select u.*,r.RoleName,t.DeptName from Users u join Role r on (u.RoleID = r.ID) join Dept t on (u.DeptID = t.ID) and u.ID = '" + ID + "' where u.IsDeleted = 0");
            return temp.FirstOrDefault();
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Result DelUser(string ID)
        {
            user.Delete(new Guid(ID));

            return new Result(true, "删除成功", null);
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public Result SaveNewUsers(Users temp)
        {
            try
            {
                Users u = user.FindByUserName(temp.UserName);
                if (u == null)
                {

                    temp.ID = Guid.NewGuid();
                    temp.IsDeleted = false;
                    user.Add(temp);
                    return new Result(true, "新增成功", null);
                }

                else {

                    return new Result(false, "用户名已存在", null);

                }


            }

            catch (Exception e)
            {
                return new Result(false, e.Message, null);
            }

        }
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public Result UpdateUsers(Users temp)
        {
            try
            {
                user.Update(temp);
                return new Result(true, "更新成功", null);
            }
            catch (Exception e)
            {
                return new Result(false, e.Message, null);
            }

        }



        /// <summary>
        /// 拉取角色列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Role> PullRoleList()
        {

            IEnumerable<Role> temp = role.SqlQuery("select * from Role where IsDeleted = 0 order by Rank asc");

            return temp;
        }



        /// <summary>
        /// 拉取角色详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Role PullRoleDetail(string ID)
        {
            IEnumerable<Role> temp = role.SqlQuery("select * from Role where IsDeleted = 0 and ID ='" + ID + "'");
            if (temp.Count() > 0)
            {
                return temp.FirstOrDefault();

            }
            else
                return new Role();
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Result DelRole(string ID)
        {

            role.Delete(new Guid(ID));

            return new Result(true, "删除成功", null);
        }
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public Result SaveNewRole(Role temp)
        {
            try
            {
                Role u = role.FindByRoleName(temp.RoleName);
                if (u == null)
                {

                    temp.ID = Guid.NewGuid();
                    temp.IsDeleted = false;
                   role.Add(temp);
                    return new Result(true, "新增成功", null);
                }

                else
                {

                    return new Result(false, "角色名已存在", null);

                }


            }

            catch (Exception e)
            {
                return new Result(false, e.Message, null);
            }

        }
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public Result UpdateRole(Role temp)
        {
            try
            {
                role.Update(temp);
                return new Result(true, "更新成功", null);
            }
            catch (Exception e)
            {
                return new Result(false, e.Message, null);
            }

        }

        /// <summary>
        /// 拉去部门树
        /// </summary>
        /// <returns></returns>
        public string PullDeptTree()
        {
            return DeptCombo.GetJson();
        }
        /// <summary>
        /// 拉取部门列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Dept> PullDeptList()
        {

            IEnumerable<Dept> temp = dept.SqlQuery("select * from Dept where IsDeleted = 0 order by Rank asc");

            return temp;
        }
        /// <summary>
        /// 拉取部门详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Dept PullDeptDetail(string ID)
        {
            IEnumerable<Dept> temp = dept.SqlQuery("select * from Dept where IsDeleted = 0 and ID ='" + ID + "'");
            if (temp.Count() > 0)
            {
                return temp.FirstOrDefault();

            }
            else
                return new Dept();
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Result DelDept(string ID)
        {

            dept.Delete(new Guid(ID));

            return new Result(true, "删除成功", null);
        }
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public Boolean SaveNewDept(Dept temp)
        {
            try
            {
                temp.ID = Guid.NewGuid();
                temp.IsDeleted = false;
                dept.Add(temp);
                return true;
            }

            catch (Exception e)
            {
                return false;
            }

        }
        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public Boolean UpdateDept(Dept temp)
        {
            try
            {
                dept.Update(temp);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

    }
}
