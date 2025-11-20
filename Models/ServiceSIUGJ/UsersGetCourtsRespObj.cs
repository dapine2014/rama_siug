using System;
using System.Collections.Generic;
using System.Text;

namespace SIUGJ.Models.ServiceSIUGJ
{
    public class UsersGetCourtsRespObj
    {
        public string result { get; set; }
        public string msgErr { get; set; }
        public string numCourts { get; set; }
        public List<Court> courts { get; set; }

        public UsersGetCourtsRespObj(string result, string msgErr)
        {
            this.result = result;
            this.msgErr = msgErr;
        }
    }
}
