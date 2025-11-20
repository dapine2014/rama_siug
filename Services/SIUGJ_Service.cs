using Fizzler;
using SIUGJ.Helpers;
using SIUGJ.Models.ServiceSIUGJ;
using Svg;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using static SIUGJ.Models.ServiceSIUGJ.DataBaseSIUGJ;

namespace SIUGJ.Services
{
    public class SIUGJ_Service
    {
        public DataBaseSIUGJ AuthenticateUser(string email, string password) 
        {
            DataBaseSIUGJ dataBase = DataBaseSIUGJ.GetInstance;

            //Seteamos el estatus de respuesta del servicio por defecto
            dataBase.serviceStatus = EnServiceResults.InvocationError;
            var i = 0;
            while ((i < Settings.ServiceRetries) && (dataBase.serviceStatus == EnServiceResults.InvocationError))
            {
                i++;

                var macAddress = "";
                var urlAutentication = "";

                try
                {
                    macAddress = GetDeviceInfo().ToUpper();
                }
                catch (Exception e)
                {
                    Console.WriteLine("GENERAMOS UN MAC ADDRESS PARA CELULARES IOS");
                    Console.WriteLine(e);
                    macAddress = "000000000000";
                }
                urlAutentication = Helpers.ServiceHelper.GetRequiredService<Helpers.Base64>().EncryptString2(email + "_@@_" + password + "_@@_" + macAddress.ToUpper(), Constants.UrlConnection);
                Console.WriteLine("********** Invocando servicio******* urlAutentication: " + urlAutentication);
                AutenticateUserRespObj responseAuth = null;
                try
                {
                    responseAuth = (new wsSOAP_SIUGJ()).AutenticateUser(urlAutentication, macAddress);
                    Console.WriteLine("Respuesta servicio: " + responseAuth.result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("** Ocurrio excepcion en servicio: " + ex.ToString());
                    responseAuth = new AutenticateUserRespObj("100", "Servicio no disponible, por favor reintente");
                }

                //Seteamos la repuesta en la base de datos central
                dataBase.responseAuth = responseAuth;
                if (responseAuth != null)
                {
                    switch (responseAuth.result)
                    {
                        case "1":
                            dataBase.serviceStatus = DataBaseSIUGJ.EnServiceResults.Success;                            

                            //JL: Temporalmente seteamos el nombre mientras el servicio lo retorna
                            dataBase.responseAuth.userName = email;

                            App.dataBase.SaveAuth(dataBase.responseAuth);
                            break;
                        case "0":
                            dataBase.serviceStatus = DataBaseSIUGJ.EnServiceResults.InvalidCredentials;
                            break;
                        case "2":
                            dataBase.serviceStatus = DataBaseSIUGJ.EnServiceResults.InvalidCredentials;
                            break;
                    }
                }
            }

            return dataBase;
        }

        public DataBaseSIUGJ UsersTask(int taskStatus, bool forceUpdate)
        {
            //Cargamos la base de datos central
            DataBaseSIUGJ dataBase = DataBaseSIUGJ.GetInstance;

            //JL: TMP Linea para probar servicio sin autenticar, desde boton login
            //dataBase.responseAuth = new AutenticateUserRespObj("1", "", "DC8C8B986AECB7336C13BC774434CEDE9541D43D668A6624F25991C0DFC63AD65B4747A06F15D56D43795F9A2DBE0E9C", "Prueba");

            if (dataBase.responseAuth != null)
            {
                //Validamos si hay que invocar el servicio para actualizar los datos
                bool responseIsNotNull = dataBase.responseUserTask != null && dataBase.responseUserTask.tasks != null;
                if (!forceUpdate && responseIsNotNull)
                {
                    //Retornamos base actual sin actualizar
                    Console.WriteLine("** Retornando base datos sin actualizar");
                    dataBase.serviceStatus = EnServiceResults.Success;
                    return dataBase;
                }

                //Se debe invocar el servicio según reintentos fallidos
                //Seteamos el estatus de respuesta del servicio por defecto
                dataBase.serviceStatus = EnServiceResults.InvocationError;  
                var i = 0;
                while ((i < Settings.ServiceRetries) && (dataBase.serviceStatus == EnServiceResults.InvocationError))
                {
                    i++;

                    var tokenAccess = dataBase.responseAuth.tokenAccess;
                    var macAddress = "";
                    try
                    {
                        macAddress = GetDeviceInfo().ToUpper();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error obteniendo información del dispositivo: {e}");
                        macAddress = "000000000000";
                    }
                    Console.WriteLine("********** Invocando servicio******* macAddress: " + macAddress);
                    UsersTaskResponseRespObj response = new UsersTaskResponseRespObj("100", "Servicio no disponible, por favor reintente");
                    var wasException = false;
                    try
                    {
                        response = (new wsSOAP_SIUGJ()).UsersTask(tokenAccess, macAddress, taskStatus);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("** Ocurrio excepcion en servicio: " + ex.ToString());
                        wasException = true;
                    }

                    if (response != null && !wasException)
                    {
                        switch (response.result)
                        {
                            case "1":
                                dataBase.serviceStatus = EnServiceResults.Success;
                                //Ordenamos las tareas
                                response.tasks.Sort(delegate (Models.ServiceSIUGJ.Task x, Models.ServiceSIUGJ.Task y) {
                                    DateTime date_x = DateTime.ParseExact(x.fechaAsignacion, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                                    DateTime date_y = DateTime.ParseExact(x.fechaAsignacion, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                                    return date_x.CompareTo(date_y);
                                });
                                
                                //Se actualizan las tareas en base datos por respuesta exitosa
                                dataBase.responseUserTask = response;
                                

                                break;
                            case "0":
                                dataBase.serviceStatus = EnServiceResults.InvalidCredentials;
                                break;
                        }
                    }
                }                
            } 
            else
            {
                Console.WriteLine("** No existe TOKEN para invocar el servicio");
                dataBase.serviceStatus = EnServiceResults.InvocationError;
            }
            
            return dataBase;
        }

        public DataBaseSIUGJ UsersAudiences(string start, string end, int statusAudiences, bool forceUpdate)
        {
            //Cargamos la base de datos central
            DataBaseSIUGJ dataBase = DataBaseSIUGJ.GetInstance;

            //JL: TMP Linea para probar servicio sin autenticar, desde boton login
            //dataBase.responseAuth = new AutenticateUserRespObj("1", "", "DC8C8B986AECB7336C13BC774434CEDE9541D43D668A6624F25991C0DFC63AD65B4747A06F15D56D43795F9A2DBE0E9C", "Prueba");

            if (dataBase.responseAuth != null)
            {
                //Validamos si hay que invocar el servicio para actualizar los datos
                bool responseIsNotNull = dataBase.responseUsersAudiences != null && dataBase.responseUsersAudiences.audiences != null;
                if (!forceUpdate && responseIsNotNull)
                {
                    //Retornamos base actual sin actualizar
                    Console.WriteLine("** Retornando base datos sin actualizar");
                    dataBase.serviceStatus = EnServiceResults.Success;
                    return dataBase;
                }

                //Se debe invocar el servicio según reintentos fallidos
                //Seteamos el estatus de respuesta del servicio por defecto
                dataBase.serviceStatus = EnServiceResults.InvocationError;
                var i = 0;
                while ((i < Settings.ServiceRetries) && (dataBase.serviceStatus == EnServiceResults.InvocationError))
                {
                    i++;

                    var tokenAccess = dataBase.responseAuth.tokenAccess;
                    var macAddress = "";
                    try
                    {
                        macAddress = GetDeviceInfo().ToUpper();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error obteniendo información del dispositivo: {e}");
                        macAddress = "000000000000";
                    }
                    
                    Console.WriteLine("********** Invocando servicio******* macAddress: " + macAddress);
                    UsersAudiencesRespObj response = new UsersAudiencesRespObj("100", "Servicio no disponible, por favor reintente");
                    var wasException = false;
                    try
                    {
                        response = (new wsSOAP_SIUGJ()).UsersAudiences(tokenAccess, macAddress, start, end, statusAudiences);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("** Ocurrio excepcion en servicio: " + ex.ToString());
                        wasException = true;
                    }

                    if (response != null && !wasException)
                    {
                        switch (response.result)
                        {
                            case "1":
                                dataBase.serviceStatus = EnServiceResults.Success;
                                //Ordenamos las audiencias
                                response.audiences.Sort(delegate (Audience x, Audience y) {
                                    DateTime date_x = DateTime.ParseExact(x.horaInicial, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                                    DateTime date_y = DateTime.ParseExact(x.horaInicial, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                                    return date_x.CompareTo(date_y);
                                });

                                //Se actualizan las audiencias en base datos por respuesta exitosa                                
                                dataBase.responseUsersAudiences = response;

                                //JL: Temporalmente seteamos datos faltantes
                                foreach (var aud in dataBase.responseUsersAudiences.audiences)
                                {
                                    aud.urlVideoConferencia = "https://teams.microsoft.com/l/meetup-join/19:meeting_NzJkMTA2ODctZmVkNC00Y2UxLWE3MTUtM2MwNDhiNDI2MWI1@thread.v2/0?context=%7B%22Tid%22:%221bff854d-e0cf-410f-bcb0-d94934431e43%22,%22Oid%22:%22efc1e378-747a-4d63-862d-a6ff5ac2a95c%22%7D";
                                }

                                break;
                            case "0":
                                dataBase.serviceStatus = EnServiceResults.InvalidCredentials;
                                break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("** No existe TOKEN para invocar el servicio");
                dataBase.serviceStatus = EnServiceResults.InvocationError;
            }

            return dataBase;
        }

        public DataBaseSIUGJ UsersGetCourts(string state, int typeCourt, bool forceUpdate)
        {
            //Cargamos la base de datos central
            DataBaseSIUGJ dataBase = DataBaseSIUGJ.GetInstance;

            //JL: TMP Linea para probar servicio sin autenticar, desde boton login
            //dataBase.responseAuth = new AutenticateUserRespObj("1", "", "DC8C8B986AECB7336C13BC774434CEDE9541D43D668A6624F25991C0DFC63AD65B4747A06F15D56D43795F9A2DBE0E9C", "Prueba");

            if (dataBase.responseAuth != null)
            {
                // Validamos si hay que invocar el servicio para actualizar los datos
                bool responseIsNotNull = dataBase.responseUsersGetCourts != null && dataBase.responseUsersGetCourts.courts != null;
                if (!forceUpdate && responseIsNotNull)
                {
                    //Retornamos base actual sin actualizar
                    Console.WriteLine("** Retornando base datos sin actualizar");
                    dataBase.serviceStatus = EnServiceResults.Success;
                    return dataBase;
                }

                //Se debe invocar el servicio según reintentos fallidos
                //Seteamos el estatus de respuesta del servicio por defecto
                dataBase.serviceStatus = EnServiceResults.InvocationError;
                var i = 0;
                while ((i < Settings.ServiceRetries) && (dataBase.serviceStatus == EnServiceResults.InvocationError))
                {
                    i++;

                    var tokenAccess = dataBase.responseAuth.tokenAccess;
                    var macAddress = "";
                    try
                    {
                        macAddress = GetDeviceInfo().ToUpper();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error obteniendo información del dispositivo: {e}");
                        macAddress = "000000000000";
                    }
                    Console.WriteLine("********** Invocando servicio******* macAddress: " + macAddress);
                    UsersGetCourtsRespObj response = new UsersGetCourtsRespObj("100", "Servicio no disponible, por favor reintente");
                    var wasException = false;
                    try
                    {
                        response = (new wsSOAP_SIUGJ()).UsersGetCourts(tokenAccess, macAddress, state, typeCourt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("** Ocurrio excepcion en servicio: " + ex.ToString());
                        wasException = true;
                    }

                    if (response != null && !wasException)
                    {
                        switch (response.result)
                        {
                            case "1":
                                dataBase.serviceStatus = EnServiceResults.Success;
                                //Se actualiza el mapa judicial en base datos por respuesta exitosa
                                dataBase.responseUsersGetCourts = response;

                                /*JL: Temporalmente seteamos algunos datos faltantes
                                Dictionary<string, string> geoCourts = new Dictionary<string, string>
                                        {
                                            //Pereira Codigo despacho , ubicacion con slash lat/long
                                            { "660012205001", "-75.712646/4.816588" },
                                            { "660013105001", "-75.69370329/4.805076429" },
                                            { "660014105001", "-75.69425/4.81213" },
                                            { "660014105002", "-75.69425/4.81213" },
                                            { "660013105005", "-75.7127768/4.8165487" },
                                            { "660013105004", "-75.7127768/4.8165487" },
                                            //Armenia
                                            { "630013105004", "-75.672405/4.531733" },
                                            { "630013105003", "-75.712646/4.816588" },
                                            { "630014105001", "-75.6734113/4.5327321" },
                                            //Sincelejo
                                            { "700013105001", "-75.7127768/4.8165487" },
                                            { "700013105002", "-75.39694/9.30127" },
                                            //Bogota
                                            { "110012205001", "-74.099204/4.640274" },
                                            { "110012205015", "-74.17960358/4.31533432" },
                                            { "110013105001", "-74.073888/4.600971" },
                                            { "110013105009", "-74.16747/4.67899" },
                                            { "110013105020", "-74.09861344/4.61703922" },
                                            { "110013105012", "-74.073888/4.600971" }
                                        };
                                foreach (var ct in dataBase.responseUsersGetCourts.courts)
                                {
                                    ct.jurisdiccion = "Ordinaria";
                                    if (ct.nombreDespacho.Contains("LABORAL")) 
                                    {
                                        ct.especialidad = "Laboral";
                                    } 
                                    else if (ct.nombreDespacho.Contains("CIVIL FAMILIA"))
                                    {
                                        ct.especialidad = "Familia";
                                    } 
                                    else if (ct.nombreDespacho.Contains("PENAL"))
                                    {
                                        ct.especialidad = "Penal";
                                    } 
                                    else
                                    {
                                        ct.especialidad = "Civil";
                                    }
                                    string geo;
                                    if (geoCourts.TryGetValue(ct.claveDespacho, out geo))
                                    {
                                        string[] geoValues = geo.Split('/');
                                        ct.longitud = geoValues[0];
                                        ct.latitud = geoValues[1];
                                    }                                
                                }
                                */

                                break;
                            case "0":
                                dataBase.serviceStatus = EnServiceResults.InvalidCredentials;
                                break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("** No existe TOKEN para invocar el servicio");
                dataBase.serviceStatus = EnServiceResults.InvocationError;
            }

            return dataBase;
        }

        public DataBaseSIUGJ GetCities(bool forceUpdate)
        {
            //Cargamos la base de datos central
            DataBaseSIUGJ dataBase = DataBaseSIUGJ.GetInstance;

            //JL: TMP Linea para probar servicio sin autenticar, desde boton login
            //dataBase.responseAuth = new AutenticateUserRespObj("1", "", "DC8C8B986AECB7336C13BC774434CEDE9541D43D668A6624F25991C0DFC63AD65B4747A06F15D56D43795F9A2DBE0E9C", "Prueba");

            if (dataBase.responseAuth != null)
            {
                //Validamos si hay que invocar el servicio para actualizar los datos
                bool responseIsNotNull = dataBase.responseGetCities != null && dataBase.responseGetCities.cities != null;
                if (!forceUpdate && responseIsNotNull)
                {
                    //Retornamos base actual sin actualizar
                    Console.WriteLine("** Retornando base datos sin actualizar");
                    dataBase.serviceStatus = EnServiceResults.Success;
                    return dataBase;
                }

                //Se debe invocar el servicio según reintentos fallidos
                //Seteamos el estatus de respuesta del servicio por defecto
                dataBase.serviceStatus = EnServiceResults.InvocationError;
                var i = 0;
                while ((i < Settings.ServiceRetries) && (dataBase.serviceStatus == EnServiceResults.InvocationError))
                {
                    i++;

                    var tokenAccess = dataBase.responseAuth.tokenAccess;
                    var macAddress = "";
                    try
                    {
                        macAddress = GetDeviceInfo().ToUpper();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error obteniendo información del dispositivo: {e}");
                        macAddress = "000000000000";
                    }
                    Console.WriteLine("********** Invocando servicio GetCiudades ******* macAddress: " + macAddress);
                    GetCitiesRespObj response = new GetCitiesRespObj("100", "Servicio no disponible, por favor reintente");
                    var wasException = false;
                    try
                    {
                        //response = new GetCitiesRespObj("1", "");
                        response = (new wsSOAP_SIUGJ()).GetCities(tokenAccess, macAddress);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("** Ocurrio excepcion en servicio: " + ex.ToString());
                        wasException = true;
                    }

                    if (response != null && !wasException)
                    {
                        switch (response.result)
                        {
                            case "1":
                                dataBase.serviceStatus = EnServiceResults.Success;
                                //Se actualizan las tareas si hay una respuesta exitosa
                                dataBase.responseGetCities = response;

                                //JL: Temporalmente seteamos datos 
                                //dataBase.responseGetCities.numCities = "5";
                                //dataBase.responseGetCities.cities = new List<City>() {
                                //    new City("63001", "Armenia", "Quindio"),
                                //    new City("11001", "Bogotá", "Bogotá"),
                                //    new City("17001", "Manizales", "Caldas"),
                                //    new City("66001", "Pereira", "Risaralda"),
                                //    new City("70001", "Sincelejo", "Sucre")
                                //};

                                break;
                            case "0":
                                dataBase.serviceStatus = EnServiceResults.InvalidCredentials;
                                break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("** No existe TOKEN para invocar el servicio");
                dataBase.serviceStatus = EnServiceResults.InvocationError;
            }

            return dataBase;
        }

        private string GetDeviceInfo()
        {
            string mac = string.Empty;
            string ip = string.Empty;

            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    var address = netInterface.GetPhysicalAddress();
                    mac = BitConverter.ToString(address.GetAddressBytes());

                    IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
                    if (addresses != null && addresses[0] != null)
                    {
                        ip = addresses[0].ToString();
                        break;
                    }
                }
            }

            return !string.IsNullOrEmpty(mac) ? mac : "001ec29e286b";
        }

        /**
         * JL: Este motodo es usado para invocación con codigo autogenerado, usa la clase CustomTextMessageBindingElement
         * 
         * 
             * **** JL: INI Invocación servicio con codigo autogenerado  *********
            //var endpoint = new EndpointAddress(new Uri("http://192.168.0.101:5000/webServices/wsMovilServices.php"));
            var endpoint = new EndpointAddress(new Uri("https://qa-siugj.linktic.com/webServices/wsMovilServices.php"));
            WsSIUGJ.ApplicationServicesPortTypeClient clientWs = new WsSIUGJ.ApplicationServicesPortTypeClient(GetBindConfiguration(endpoint), endpoint);
            //WsSIUGJ.ApplicationServicesPortTypeClient clientWs = new WsSIUGJ.ApplicationServicesPortTypeClient(endpoint);
            //WSMobileSIUGJ.ApplicationServicesPortTypeClient clientWs = new WSMobileSIUGJ.ApplicationServicesPortTypeClient(
            //    WSMobileSIUGJ.ApplicationServicesPortTypeClient.EndpointConfiguration.ApplicationServicesPort,
            //    "http://localhost:5000/webServices/wsMovilServices.php");
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            
            using (new OperationContextScope(clientWs.InnerChannel))
            {
                // // Add a SOAP Header (Header property in the envelope) to an outgoing request. 
                // MessageHeader aMessageHeader = MessageHeader
                //    .CreateHeader("MySOAPHeader", "http://tempuri.org", "MySOAPHeaderValue");
                // OperationContext.Current.OutgoingMessageHeaders.Add(aMessageHeader);

                // Add a HTTP Header to an outgoing request
                HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                requestMessage.Headers["Host"] = "qa-siugj.linktic.com";
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name]
                   = requestMessage;

                string result = "";
                try
                {
                    result = clientWs.autenticateUser(urlAutentication, MacAddress);
                    Console.WriteLine("Respuesta servicio: " + result);
                } catch (Exception ex)
                {
                    Console.WriteLine("Respuesta servicio: " + result);
                }
                result = clientWs.autenticateUser(urlAutentication, MacAddress);

                Console.WriteLine("Respuesta servicio: " + result);
            }

            var response = clientWs.autenticateUser(urlAutentication, MacAddress);
            var response = await clientWs.autenticateUser(urlAutentication, MacAddress);

            * **** FIN Invocación servicio con codigo autogenerado  *********
            
         */
        private System.ServiceModel.Channels.Binding GetBindConfiguration(EndpointAddress endpoint)
        {
            //This configs could be different in Soap server that you are implementing
            //set the soap server config: encoding, mediaType and Soap Version 1.1
            var custom = new CustomTextMessageBindingElement("ISO-8859-1", "text/xml", MessageVersion.CreateVersion(EnvelopeVersion.Soap11, AddressingVersion.None));

            TransportBindingElement httpsBindingElement;
            if (endpoint.Uri.Scheme == "https")
                httpsBindingElement = new HttpsTransportBindingElement()
                {
                    MaxReceivedMessageSize = int.MaxValue,
                };
            else
                httpsBindingElement = new HttpTransportBindingElement()
                {
                    MaxReceivedMessageSize = int.MaxValue,
                };

            return new CustomBinding(custom, httpsBindingElement);
        }


    }
}
