using System;
using System.Collections.Generic;
using System.Text;

namespace SIUGJ.Models.ServiceSIUGJ
{
    public class City
    {
        public string codigoCiudad { get; set; }
        public string nombreCiudad { get; set; }
        public string nombreDepartamento { get; set; }

        public City(string codigoCiudad, string nombreCiudad, string nombreDepartamento)
        {
            this.codigoCiudad = codigoCiudad;
            this.nombreCiudad = nombreCiudad;
            this.nombreDepartamento = nombreDepartamento;
        }
    }
}
