using System;
using SQLite;
using SQLitePCL;
using SQLiteNetExtensions.Extensions;
using SIUGJ.Models.ServiceSIUGJ;
using System.Linq;

namespace SIUGJ.Data
{
	public class DataBase
	{
        static object locker = new object();
        string _rutaBD;

        public SQLiteConnection Conexion { get; set; }

        public DataBase(string rutaBD)
        {
            _rutaBD = rutaBD;
        }
        public void Conectar()
        {
            try
            {
                Conexion = new SQLiteConnection(_rutaBD,
                    SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create
                                              | SQLiteOpenFlags.FullMutex, true);

                Conexion.CreateTable<AutenticateUserRespObj>();
            }
            catch (Exception e)
            {
              
            }
            

        }

        public void SaveAuth(AutenticateUserRespObj auth)
        {
            lock (locker)
            {
                Conexion.Insert(auth);
            }
        }
        public void UpdateAuth(AutenticateUserRespObj auth)
        {
            lock (locker)
            {
                Conexion.Update(auth);
            }
        }
        public AutenticateUserRespObj GetAuth()
        {
            lock (locker)
            {
                return Conexion.GetAllWithChildren<AutenticateUserRespObj>().FirstOrDefault();
            }
        }
        public void RemoveAuth()
        {
            lock (locker)
            {
                Conexion.DeleteAll<AutenticateUserRespObj>();
            }
        }
    }
}

