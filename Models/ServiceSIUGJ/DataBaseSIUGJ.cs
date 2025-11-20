using System;
using System.Collections.Generic;
using System.Text;

namespace SIUGJ.Models.ServiceSIUGJ
{
    public sealed class DataBaseSIUGJ
    {
        private static readonly DataBaseSIUGJ _instance = new DataBaseSIUGJ();        

        private DataBaseSIUGJ()
        {            
        }

        public static DataBaseSIUGJ GetInstance
        {
            get
            {
                if (_instance.responseAuth == null)
                {
                    _instance.responseAuth = App.dataBase.GetAuth(); ;
                }

                return _instance;
            }
        }

        public EnServiceResults serviceStatus { get; set; }
        public AutenticateUserRespObj responseAuth { get; set; }

        public UsersTaskResponseRespObj responseUserTask { get; set; }
        public UsersAudiencesRespObj responseUsersAudiences { get; set; }
        public UsersGetCourtsRespObj responseUsersGetCourts { get; set; }
        public GetCitiesRespObj responseGetCities { get; set; }


        public enum EnServiceResults
        {
            Success,
            InvalidCredentials,
            InvocationError
        }

    }
}
