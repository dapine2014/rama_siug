using System;
using System.Collections.Generic;
using System.Text;

namespace SIUGJ.Models.ServiceSIUGJ
{
    public class GetCitiesRespObj
    {
        public string result { get; set; }
        public string msgErr { get; set; }
        public string numCities { get; set; }
        public List<City> cities { get; set; }

        public GetCitiesRespObj(string result, string msgErr)
        {
            this.result = result;
            this.msgErr = msgErr;
        }
    }
}
