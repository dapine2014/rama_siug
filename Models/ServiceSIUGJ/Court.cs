using System;
using System.Collections.Generic;
using System.Text;

namespace SIUGJ.Models.ServiceSIUGJ
{
    public class Court
    {
        public string nombreDespacho { get; set; }
        public string IDDespacho { get; set; }
        public string claveDespacho { get; set; }
        public string domicilio { get; set; }
        public string departamento { get; set; }
        public string municipio { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string telefonos { get; set; }
        public string correos { get; set; }
        public string tipoRegistro { get; set; }
        public string jurisdiccion { get; set; }
        public string especialidad { get; set; }
        public string categoría { get; set; }
        public string distrito { get; set; }
        public string circuito { get; set; }
    }
}
