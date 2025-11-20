using SIUGJ.Helpers;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SIUGJ.Services
{
    public class AuthenticationService
    {
        public async Task<string> Authenticate(string Email, string Password, string MacAddress)
        {
            var token = string.Empty;

            var urlAutentication = ServiceHelper.GetRequiredService<Base64>()
                .EncryptString2(Email + "_@@_" + Password + "_@@_" + MacAddress.ToUpper(), Settings.UrlConnection);


            var client = new WSMobileSIUGJ.ApplicationServicesPortTypeClient();
            var response = await client.autenticateUserAsync(urlAutentication, MacAddress.ToUpper());

            return token;
        }
    }
}
