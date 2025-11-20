using System;
using SIUGJ.Enums;

namespace SIUGJ.Models
{
	public class Notification
	{
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

        public string BackgroundColor { get {
                return this.taskstatus.Equals(((int)TaskStatusEnum.Active).ToString()) ? "#F3F7FE" : "White";
            } }
    }
}

