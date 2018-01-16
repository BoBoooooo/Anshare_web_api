using api.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web.Http;
using System.Linq;
using System.Data;
using Model;
using System;

namespace api.Controllers
{
    public class LoginController : ApiController
    {
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
                    AuthInfo authInfo = new AuthInfo
                    {
                        IsAdmin = true,
                        Roles = new List<string> { "admin", "owner" },
                        UserName = user.UserName,
                        TimeStamp = DateTime.Now.ToString()

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
