using System.IO;
using Microsoft.Maui.Storage;

namespace SIUGJ.Helpers
{
    public static class FileHelper
    {
        public static string ObtenerRutaLocal(string nombreBD)
        {
            string folder = FileSystem.AppDataDirectory;
            return Path.Combine(folder, nombreBD);
        }
    }
}
