using System.Collections.Generic;

namespace SIUGJ.Models.ServiceSIUGJ
{
    public class UsersTaskResponseRespObj

    {
        public string result { get; set; }
        public string msgErr { get; set; }
        public string numTasks { get; set; }
        public List<Task> tasks { get; set; }

        public UsersTaskResponseRespObj(string result, string msgErr)
        {
            this.result = result;
            this.msgErr = msgErr;
        }
    }

}
