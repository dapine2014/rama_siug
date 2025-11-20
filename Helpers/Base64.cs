using System;
using System.Text;


namespace SIUGJ.Helpers
{
	public class Base64
	{
        public string EncryptString2(string cadenaValidacion, string urlConexion) {
            
            var plainTextBytes = Encoding.UTF8.GetBytes(cadenaValidacion);
            var psSource = Convert.ToBase64String(plainTextBytes);

            var sDestination = "";
            var iSourceLen = psSource.Length;
            var iKeyLen = urlConexion.Length;


            for (var iSource = 0; iSource < iSourceLen; iSource++) {
                var cSource = psSource[iSource];
                var cKey = urlConexion[iSource % iKeyLen];
                var cDestination = cSource ^ cKey;
                sDestination += Convert.ToChar(cDestination);
            }

            plainTextBytes = Encoding.UTF8.GetBytes(sDestination);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}

