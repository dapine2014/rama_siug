using System;
using System.Collections.Generic;
using System.Text;

namespace SIUGJ.Models.ServiceSIUGJ
{
    public class Task
    {
        //"idRegistro":"51020","fechaAsignacion":"2023-02-12 19:31:51","fechaLimiteAtencion":"",
        //"tipoNotificacion":"NotificaciÃ³n: SIUGJ- NotificaciÃ³n","usuarioRemitente":"Usuario de Sistema (Actor de sÃ³lo lectura)",
        //"usuarioDestinatario":"VIVIANA VANEGAS VARGAS (Destinatario)","contenidoMensaje":"",
        //"fechaVisualizacion":"","codigoUnicoProceso":"660013105005-20230000100","despacho":"USUARIOS EXTERNOS",
        //"folioProceso":"00001/2023","demandante":"VIVIANA VANEGAS VARGAS","demandado":"ALEJANDRA SAENX"}
        public string idRegistro { get; set; }
        public string fechaAsignacion { get; set; }
        public string fechaLimiteAtencion { get; set; }
        public string tipoNotificacion { get; set; }
        public string usuarioRemitente { get; set; }
        public string usuarioDestinatario { get; set; }
        public string contenidoMensaje { get; set; }
        public string fechaVisualizacion { get; set; }
        public string codigoUnicoProceso { get; set; }
        public string despacho { get; set; }
        public string folioProceso { get; set; }
        public string demandante { get; set; }
        public string demandado { get; set; }
        public string taskstatus { get; set; }
        

    }
}
