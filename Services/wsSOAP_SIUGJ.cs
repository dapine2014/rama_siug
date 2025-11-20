using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Threading.Tasks;
using System.Net.Http;
using Fizzler;
using System.Collections.Generic;
using Microsoft.Maui.Controls.Shapes;
using Newtonsoft.Json;
using SIUGJ.Models.ServiceSIUGJ;
using System.Threading;
using SIUGJ.Helpers;
using BruTile.Wms;
using System.Text;

namespace SIUGJ.Services
{
    public class wsSOAP_SIUGJ
    {
        public AutenticateUserRespObj AutenticateUser(string urlAutentication, string token)
        {
            //Creamos el SoapEnvelope
            Dictionary<string, string> valuesToSet = new Dictionary<string, string>
            {
                { "urlAutentication", urlAutentication },
                { "token", token }
            };
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(ServiceSIUGJ.autenticateUser, valuesToSet);
            //Console.WriteLine("** XML a enviar: " + soapEnvelopeXml.OuterXml);


            //Llamada al servicio
            string soapResult = CallWebService(soapEnvelopeXml, Helpers.Settings.WSUrlConnection,
                Constants.CallWebServiceActions[0], Constants.Encoding);
            Console.WriteLine(Constants.WriteLine[0] + soapResult);

            //Extraemos respuesta
            var nsmgrResponse = new XmlNamespaceManager(soapEnvelopeXml.NameTable);
            nsmgrResponse.AddNamespace(Constants.NamespacePrefix[0], Constants.NamespaceUri[0]);
            nsmgrResponse.AddNamespace(Constants.NamespacePrefix[1], Constants.NamespaceUri[1]);
            var jsonStringResponse = GetJsonResponseFromString(soapResult, Constants.PathResponce, nsmgrResponse);
            AutenticateUserRespObj responseObj =
                JsonConvert.DeserializeObject<AutenticateUserRespObj>(jsonStringResponse);
            Console.WriteLine(Constants.WriteLine[1] + responseObj.result);

            return responseObj;
        }

        public UsersTaskResponseRespObj UsersTask(string stringTokenAccess, string token, int taskStatus)
        {
            //Creamos el SoapEnvelope
            Dictionary<string, string> valuesToSet = new Dictionary<string, string>
            {
                { "stringTokenAccess", stringTokenAccess },
                { "token", token },
                { "taskStatus", taskStatus.ToString() }
            };
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(ServiceSIUGJ.usersTask, valuesToSet);

            //Llamada al servicio
            string soapResult = CallWebService(soapEnvelopeXml, Helpers.Settings.WSUrlConnection, "usersTask",
                Constants.Encoding);
            Console.WriteLine(Constants.WriteLine[2] + soapResult);

            //Extraemos respuesta
            var nsmgrResponse = new XmlNamespaceManager(soapEnvelopeXml.NameTable);
            nsmgrResponse.AddNamespace(Constants.NamespacePrefix[0], Constants.NamespaceUri[0]);
            nsmgrResponse.AddNamespace(Constants.NamespacePrefix[1], Constants.NamespaceUri[1]);
            var jsonStringResponse =
                GetJsonResponseFromString(soapResult, Constants.PathResponceUsersTask, nsmgrResponse);
            UsersTaskResponseRespObj responseObj =
                JsonConvert.DeserializeObject<UsersTaskResponseRespObj>(jsonStringResponse);
            Console.WriteLine(Constants.WriteLine[1] + responseObj.result + " / numTasks=" + responseObj.numTasks);

            return responseObj;
        }

        public GetCitiesRespObj GetCities(string stringTokenAccess, string token)
        {
            //Creamos el SoapEnvelope
            Dictionary<string, string> valuesToSet = new Dictionary<string, string>
            {
                { "stringTokenAccess", stringTokenAccess },
                { "token", token }
            };
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(ServiceSIUGJ.getCities, valuesToSet);

            //Llamada al servicio
            string soapResult = CallWebService(soapEnvelopeXml, Helpers.Settings.WSUrlConnection, "getCities",
                Constants.Encoding);
            Console.WriteLine(Constants.WriteLine[3] + soapResult);

            //Extraemos respuesta
            var nsmgrResponse = new XmlNamespaceManager(soapEnvelopeXml.NameTable);
            nsmgrResponse.AddNamespace(Constants.NamespacePrefix[0], Constants.NamespaceUri[0]);
            nsmgrResponse.AddNamespace(Constants.NamespacePrefix[1], Constants.NamespaceUri[1]);
            var jsonStringResponse =
                GetJsonResponseFromString(soapResult, Constants.PathResponceGetCities, nsmgrResponse);
            GetCitiesRespObj responseObj = JsonConvert.DeserializeObject<GetCitiesRespObj>(jsonStringResponse);
            Console.WriteLine(Constants.WriteLine[1] + responseObj.result + " / numTasks=" + responseObj.cities);

            return responseObj;
        }

        public UsersAudiencesRespObj UsersAudiences(string stringTokenAccess, string token, string start, string end,
            int statusAudiences)
        {
            //Creamos el SoapEnvelope
            Dictionary<string, string> valuesToSet = new Dictionary<string, string>
            {
                { "stringTokenAccess", stringTokenAccess },
                { "token", token },
                { "start", start },
                { "end", end },
                { "statusAudiences", statusAudiences.ToString() }
            };
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(ServiceSIUGJ.usersAudiences, valuesToSet);

            //Llamada al servicio
            string soapResult = CallWebService(soapEnvelopeXml, Helpers.Settings.WSUrlConnection,
                Constants.CallWebServiceActions[1], Constants.Encoding);
            Console.WriteLine(Constants.WriteLine[6] + soapResult);

            //Extraemos respuesta
            var nsmgrResponse = new XmlNamespaceManager(soapEnvelopeXml.NameTable);
            nsmgrResponse.AddNamespace(Constants.NamespacePrefix[0], Constants.NamespaceUri[0]);
            nsmgrResponse.AddNamespace(Constants.NamespacePrefix[1], Constants.NamespaceUri[1]);
            var jsonStringResponse =
                GetJsonResponseFromString(soapResult, Constants.PathResponceUsersAudiences, nsmgrResponse);//trae las audiencias de la fecha seleccionada
            UsersAudiencesRespObj responseObj =
                JsonConvert.DeserializeObject<UsersAudiencesRespObj>(jsonStringResponse);
            Console.WriteLine(Constants.WriteLine[1] + responseObj.result + " / numAudiences=" +
                              responseObj.numAudiences);

            return responseObj;
        }

        public UsersGetCourtsRespObj UsersGetCourts(string stringTokenAccess, string token, string state, int typeCourt)
        {
            //Creamos el SoapEnvelope
            Dictionary<string, string> valuesToSet = new Dictionary<string, string>
            {
                { "stringTokenAccess", stringTokenAccess },
                { "token", token },
                { "state", state },
                { "typeCout", typeCourt.ToString() }
            };
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(ServiceSIUGJ.usersGetCourts, valuesToSet);

            //Llamada al servicio
            string soapResult = CallWebService(soapEnvelopeXml, Helpers.Settings.WSUrlConnection,
                "usersGetCourtsResponse", Constants.Encoding);
            Console.WriteLine(Constants.WriteLine[4] + soapResult);

            //Extraemos respuesta
            var nsmgrResponse = new XmlNamespaceManager(soapEnvelopeXml.NameTable);
            nsmgrResponse.AddNamespace(Constants.NamespacePrefix[0], Constants.NamespaceUri[0]);
            nsmgrResponse.AddNamespace(Constants.NamespacePrefix[1], Constants.NamespaceUri[1]);
            var jsonStringResponse =
                GetJsonResponseFromString(soapResult, Constants.PathResponceGetCourts, nsmgrResponse);
            UsersGetCourtsRespObj responseObj =
                JsonConvert.DeserializeObject<UsersGetCourtsRespObj>(jsonStringResponse);
            Console.WriteLine(Constants.WriteLine[1] + responseObj.result + " / numCourts=" + responseObj.numCourts);

            return responseObj;
        }

        public string CallWebService(XmlDocument soapEnvelopeXml, string url, string action, string encoding)
        {
            //url = "http://192.168.0.100:5000/webServices/wsMovilServices.php";
            var _action = Constants.Actions + "/" + action;

            Console.WriteLine(Constants.WriteLine[5] + soapEnvelopeXml.OuterXml);

            //JL: Para llamado HTTPS (Funciono sin esto)
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            //ServicePointManager.Expect100Continue = false;
            //ServicePointManager.MaxServicePointIdleTime = 200;

            HttpWebRequest webRequest = CreateWebRequest(url, _action);

            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            Console.WriteLine("** XML insertado en llamado del servicio");

            /*
             ***** Llamado asincrono ****
             **/
            Console.WriteLine("** Iniciando llamado asincrono del servicio: ");
            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult = "";
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream(),
                           Encoding.GetEncoding(encoding)))
                {
                    soapResult = rd.ReadToEnd();
                    rd.Close();
                }
            }


            /*
             * **** INI Llamado sincrono ****
             *
            Console.WriteLine("** Iniciando llamado sincrono del servicio: ");
            string soapResult = "";
            WebResponse webResponse = webRequest.GetResponse();
            using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
            {
                soapResult = rd.ReadToEnd();
                rd.Close();
            }
            * **** FIN Llamado sincrono **** */

            /*
             * *** INI Llamado con HttpClient ***
             *
            var client = new HttpClient();

            var response = await client.PostAsync(url, "hola");

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            * *** FIN Llamado con HttpClient *** */

            return soapResult;
        }

        public bool AcceptAllCertifications(object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certification,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private HttpWebRequest CreateWebRequest(string url, string action)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Headers.Add("SOAPAction", action); 
            //webRequest.Headers.Add("Postman-Token", "e30d978c-bfa1-43b1-bb75-80b3c7352983");
            webRequest.ContentType = "text/xml;charset=\"ISO-8859-1\""; 
            webRequest.Accept = "text/xml";
            webRequest.KeepAlive = false;
            webRequest.Method = "POST";
            //webRequest.ContentLength = 500;
            //webRequest.Timeout = Timeout.Infinite;
            webRequest.Timeout = Settings.TimeOutServices;
            webRequest.ServicePoint.Expect100Continue = false;
            return webRequest;
        }

        private XmlDocument CreateSoapEnvelope(ServiceSIUGJ service, Dictionary<string, string> values)
        {
            //Seteamos la plantilla segun el servicio a invocar
            string template;
            switch (service)
            {
                case (ServiceSIUGJ.autenticateUser):
                    template = @"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
							    <soap:Body>
                                    <autenticateUser xmlns = ""http://tempuri.org/"">
                                        <urlAutentication></urlAutentication>
                                        <token></token>
                                    </autenticateUser >
                                </soap:Body >
                                </soap:Envelope > ";
                    break;
                case (ServiceSIUGJ.usersTask):
                    template = @"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
							        <soap:Body>
								        <usersTask xmlns='http://tempuri.org/'>
									        <stringTokenAccess></stringTokenAccess>
									        <token></token>
									        <taskStatus></taskStatus>
								        </usersTask>
							        </soap:Body>
						        </soap:Envelope> ";
                    break;
                case (ServiceSIUGJ.getCities):
                    template = @"<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
							            <soap:Body>
                                          <getCities xmlns='http://tempuri.org/'>
									            <stringTokenAccess></stringTokenAccess>
									            <token></token>
                                                <state></state>
                                                <typeCout></typeCout>
                                          </getCities>
                                       </soap:Body>
                                    </soap:Envelope> ";
                    break;
                case (ServiceSIUGJ.usersGetCourts):
                    template = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <soap:Body>
                                    <usersGetCourts xmlns='http://tempuri.org/'>
                                        <stringTokenAccess></stringTokenAccess>
                                        <token></token>
                                        <state></state>
                                        <typeCout></typeCout>
                                    </usersGetCourts>
                                </soap:Body>
                            </soap:Envelope>";
                    break;
                case (ServiceSIUGJ.usersAudiences):
                    template = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <soap:Body>
                                    <usersAudiences xmlns='http://tempuri.org/'>
                                        <stringTokenAccess></stringTokenAccess>
                                        <token></token>
                                        <start></start>
                                        <end></end>
                                        <statusAudiences></statusAudiences>
                                    </usersAudiences>
                                </soap:Body>
                            </soap:Envelope>";
                    break;
                default:
                    template = @"<soap:Envelope xmlns:s = ""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi = ""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
                                <soap:Body>
                                    <msgError>Plantilla de servicio no configurada</msgError>
                                </s:Body>
                                </s:Envelope> ";
                    break;
            }

            //Seteamos los valores
            foreach (KeyValuePair<string, string> kvp in values)
            {
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    Console.WriteLine("** Incorporando en plantilla: " + "<" + kvp.Key + "> por <" + kvp.Key + ">" +
                                      kvp.Value);
                    template = template.Replace("<" + kvp.Key + ">", "<" + kvp.Key + ">" + kvp.Value);
                }
            }

            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(template.Trim());
            return soapEnvelopeDocument;
        }

        private string GetJsonResponseFromString(string responseString, string pathResponse, XmlNamespaceManager nsmgr)
        {
            XmlDocument soapResponseDocument = new XmlDocument();
            soapResponseDocument.LoadXml(responseString);
            XmlNode nodeXMLResponse = soapResponseDocument.SelectSingleNode(pathResponse, nsmgr);
            //Console.WriteLine("** Node XML JSON: " + nodeXMLResponse.OuterXml);
            string jsonResponse = nodeXMLResponse.InnerText;
            //Console.WriteLine("** Respuesta JSON: " + jsonResponse);
            return jsonResponse;
        }

        private void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, WebRequest webRequest)
        {
            Console.WriteLine("** Incorporando XML en llamado del servicio");
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
                stream.Close();
            }
        }

        private enum ServiceSIUGJ
        {
            autenticateUser,
            usersTask,
            usersAudiences,
            usersGetCourts,
            getCities
        }
    }
}