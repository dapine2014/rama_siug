using System;
using System.Linq;

namespace SIUGJ.Helpers
{
	public static class Settings
	{
        //public static string WSUrlConnection = "https://alambraqa8030.siugj.com/webServices/wsMovilServices.php";
        //public static string SIUGJWebHomeUrl = "https://alambraqa8030.siugj.com/principalPortal/inicio.php";
        //public static string UrlConnection = "alambraqa8030.siugj.com";
        public static string WSUrlConnection = Constants.UriWs;
        public static string SIUGJWebHomeUrl = Constants.SiugJwebHomeUrl;
        public static string UrlConnection = Constants.UrlConnection;
        public static int TimeOutServices = 3000;
        public static int ServiceRetries = 3;
        public static int DaysLowerLimitForHearings = -90;
        public static int DaysUpperLimitForHearings = 360;

        public static string ToCamelCase(this string str)
        {
            var words = str.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);
            var leadWord = words[0].ToLower();
            var tailWords = words.Skip(1)
                .Select(word => char.ToUpper(word[0]) + word.Substring(1))
                .ToArray();
            return $"{leadWord}{string.Join(string.Empty, tailWords)}";
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }
    }
    
}

