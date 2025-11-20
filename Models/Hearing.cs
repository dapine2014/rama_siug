using System;
using System.Globalization;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using SIUGJ.Models.ServiceSIUGJ;
using SIUGJ.Views;

namespace SIUGJ.Models
{
	public class Hearing
    {
        public Command ManageHearingCommand { get; }

        public string Name { get; set; }
        public string IdEvento { get; set; }
        public string UniqueCode { get; set; }
        public string Description { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsVirtual { get; set; }
        public DateTime Starting { get; set; }
        public string StartingString
        {
            get {
                var culture = CultureInfo.CreateSpecificCulture("es");
                return Helpers.Settings.FirstCharToUpper(Starting.ToString("dddd, dd MMMM yyyy", culture));
            }
        }

        public Audience Audience { get; set; }

        public Hearing()
        {
            ManageHearingCommand = new Command(OnManageHearingClicked);
        }

        private void OnManageHearingClicked(object obj)
        {
            Launcher.OpenAsync(new Uri(Audience.urlVideoConferencia));
        }
    }
}
