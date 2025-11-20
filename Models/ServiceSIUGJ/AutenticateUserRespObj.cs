namespace SIUGJ.Models.ServiceSIUGJ
{
    public class AutenticateUserRespObj

    {
        public string result { get; set; }
        public string msgErr { get; set; }
        public string tokenAccess { get; set; }

        public string userName { get; set; }

        public AutenticateUserRespObj() { }

        public AutenticateUserRespObj(string result, string msgErr)
        {
            this.result = result;
            this.msgErr = msgErr;
        }

        public AutenticateUserRespObj(string result, string msgErr, string tokenAccess, string userName) : this(result, msgErr)
        {
            this.tokenAccess = tokenAccess;
            this.userName = userName;
        }
    }

}
