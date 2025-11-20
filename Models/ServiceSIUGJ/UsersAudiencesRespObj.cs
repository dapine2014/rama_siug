using System;
using System.Collections.Generic;
using System.Text;

namespace SIUGJ.Models.ServiceSIUGJ
{
    public class UsersAudiencesRespObj
    {        
        public string result { get; set; }
        public string msgErr { get; set; }
        public string numAudiences { get; set; }
        public List<Audience> audiences { get; set; }

        public UsersAudiencesRespObj(string result, string msgErr)
        {
            this.result = result;
            this.msgErr = msgErr;
        }
    }



}
