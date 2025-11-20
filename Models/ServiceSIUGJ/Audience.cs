using System;
using System.Collections.Generic;
using System.Text;

namespace SIUGJ.Models.ServiceSIUGJ
{
    public class Audience
    {
        public string idEvento { get; set; }
        public string fechaEvento { get; set; }
        public string horaInicial { get; set; }
        public string horaFinal { get; set; }
        public string situacion { get; set; }
        public string comentariosAdicionales { get; set; }
        public string tipoAudiencia { get; set; }
        public string sala { get; set; }
        public string despacho { get; set; }
        public string edificio { get; set; }
        public string juez { get; set; }
        public string codigoUnicoProceso { get; set; }
        public string urlVideoConferencia { get; set; }
        public string esVirtual { get; set; }
        public string totalParticipantes { get; set; }
    }
}
