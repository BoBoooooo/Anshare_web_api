using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class Result
    {
       public  Result(bool Success,string Message,string Token) {
            this.Success = Success;
            this.Message = Message;
            this.Token = Token;
        }

        public Result()
        {
           
        }
        public bool Success { get; set; }
        public string Message { get; set; }

        public string Token { get; set; }
    }
}