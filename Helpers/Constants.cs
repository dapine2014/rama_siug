namespace SIUGJ.Helpers
{
    public static class Constants
    {
        public const string Encoding = "ISO-8859-1";
       /* public const string WsUrlConnection = "https://alambraqa8030.siugj.com/webServices/wsMovilServices.php";
        public const string SiugjWebHomeUrl = "https://alambraqa8030.siugj.com/principalPortal/inicio.php";*/
        
          public const string WsUrlConnection = "https://preproduccion-siugj.ramajudicial.gov.co/webServices/wsMovilServices.php";
          public const string SiugjWebHomeUrl = "https://preproduccion-siugj.ramajudicial.gov.co/principalPortal/inicio.php";
         
        
        public static readonly string[] CallWebServiceActions =
        {
            "autenticateUser",
            "usersAudiences"
        };

        public static readonly string[] NamespacePrefix =
        {
            "SOAP-ENV",
            "ns1"
        };

        public static readonly string[] NamespaceUri =
        {
            "http://schemas.xmlsoap.org/soap/envelope/",
            "http://tempuri.org/"
        };


        public static readonly string[] WriteLine =
        {
            "** Respuesta: ",
            "** Objeto result JSON: ",
            "** Respuesta UsersTask: ",
            "** Respuesta GetCities: ",
            "** Respuesta UsersGetCourts: ",
            "** XML a enviar: ",
            "** Respuesta UsersAudiences: "
        };

        public const string Actions = "https://preproduccion-siugj.ramajudicial.gov.co/webServices/wsMovilServices.php";
//        public const string Actions = "https://alambraqa8030.siugj.com/webServices/wsMovilServices.php";

        public const string PathResponce = "/SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:autenticateUserResponse";
        public const string PathResponceGetCourts = "/SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:usersGetCourtsResponse";
        public const string PathResponceGetCities = "/SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:getCitiesResponse";
        public const string PathResponceUsersTask = "/SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:usersTaskResponse";
        public const string PathResponceUsersAudiences = "/SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:usersAudiencesResponse";
        
        
        
           public const string UriWs =  "https://preproduccion-siugj.ramajudicial.gov.co/webServices/wsMovilServices.php";
                  public const string SiugJwebHomeUrl = "https://preproduccion-siugj.ramajudicial.gov.co/principalPortal/inicio.php";
                  public const string UrlConnection = "preproduccion-siugj.ramajudicial.gov.co"; 
                  public const string NameWebServices = "https://preproduccion-siugj.ramajudicial.gov.co/webServices";
         
        /*
        public const string UriWs =  "https://alambraqa8030.siugj.com/webServices/wsMovilServices.php";
        public const string SiugJwebHomeUrl = "https://alambraqa8030.siugj.com/principalPortal/inicio.php";
        public const string UrlConnection = "alambraqa8030.siugj.com"; 
        public const string NameWebServices = "https://alambraqa8030.siugj.com/webServices";*/
    }
}