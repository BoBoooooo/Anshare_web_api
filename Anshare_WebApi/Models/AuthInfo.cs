using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    /// <summary>
    /// 表示jwt的payload
    /// </summary>
    public class AuthInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 角色列表，可以用于记录该用户的角色,相当于claims的概念(如不清楚什么事claim，请google一下基于声明的权限控制)
        /// </summary>
        public List<string> Roles { get; set; }
        /// <summary>
        /// 记录角色ID
        /// </summary>
        public Guid RoleID { get; set; }
        /// <summary>
        /// 权限返回标识码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }

    }
}