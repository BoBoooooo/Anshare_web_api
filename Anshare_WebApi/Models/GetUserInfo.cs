using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    /// <summary>
    /// 表示jwt的payload
    /// </summary>
    public class GetUserInfo
    {
        /// <summary>
        /// token
        /// </summary>
        public string auth { get; set; }
   

    }
}