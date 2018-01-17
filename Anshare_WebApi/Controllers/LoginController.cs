using api.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.Http;
using System.Linq;
using System.Data;
using Model;
using System;
using IBLL;
using BLL;
namespace api.Controllers
{
    public class LoginController : ApiController
    {
        IRoleService role = new RoleService();

        public Result Post([FromBody]LoginRequest request)
        {
            Result rs = new Result();

            using (DBEntities db = new DBEntities())
            {

                var users = from p in db.Users
                            where p.UserName == request.UserName && p.Password == request.Password
                            select p;
                if (users.Count() > 0)
                {
                    Users user = users.FirstOrDefault();
                    //如果用户登录成功，则可以得到该用户的身份数据。当然实际开发中，这里需要在数据库中获得该用户的角色及权限
                    Role r = role.FindAuthByID(new Guid(user.RoleID.ToString()));//找到用户对应的role auth列表

                    if (r != null)
                    {
                        AuthInfo authInfo = new AuthInfo
                        {
                            RealName = user.RealName,
                            Roles = r.RoleAuthName.Split(',').ToList(),
                            UserName = user.UserName,
                            TimeStamp = DateTime.Now.ToString(),
                            RoleID = r.ID

                        };
                        try
                        {
                            //生成token,SecureKey是配置的web.config中，用于加密token的key，打死也不能告诉别人
                            byte[] key = Encoding.Default.GetBytes(ConfigurationManager.AppSettings["SecureKey"]);
                            //采用HS256加密算法
                            string token = JWT.JsonWebToken.Encode(authInfo, key, JWT.JwtHashAlgorithm.HS256);
                            rs.Token = token;
                            rs.Success = true;
                            rs.Message = "登录成功";

                        }
                        catch
                        {
                            rs.Success = false;
                            rs.Message = "登陆失败";
                        }
                    }

                    else {
                        rs.Success = false;
                        rs.Message = "登陆失败，请联系管理员重新分配该用户角色";
                    }
                    
             
                }
                else
                {
                    rs.Success = false;
                    rs.Message = "用户名或密码不正确";
                }
                return rs;
            }
               

        }



    
    }
}
