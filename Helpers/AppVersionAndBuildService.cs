using Microsoft.Maui.ApplicationModel;

namespace SIUGJ.Helpers
{
    public class AppVersionAndBuildService : IAppVersionAndBuild
    {
        public string GetVersionNumber() => AppInfo.Current.VersionString;
        public string GetBuildNumber() => AppInfo.Current.BuildString;
    }
}
