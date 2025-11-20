using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using SIUGJ.Data;
using SIUGJ.Helpers;
using SIUGJ.Models;
using SIUGJ.Services;

namespace SIUGJ
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsRegular");
                    fonts.AddFont("MaterialIconsOutlined-Regular.otf", "Material");
                });

            builder.Services.AddSingleton<Base64>();
            builder.Services.AddSingleton<LocationService>();
            builder.Services.AddSingleton<SIUGJ_Service>();
            builder.Services.AddSingleton<MockDataStore>();
            builder.Services.AddSingleton<AuthenticationService>();
            builder.Services.AddSingleton<NotificationService>();
            builder.Services.AddSingleton<HearingService>();
            builder.Services.AddSingleton<IAppVersionAndBuild, AppVersionAndBuildService>();
            builder.Services.AddSingleton(DataBaseFactory);
            builder.Services.AddSingleton<IDataStore<Item>>(sp => sp.GetRequiredService<MockDataStore>());
            builder.Services.AddSingleton<IDataStore<Notification>>(sp => sp.GetRequiredService<NotificationService>());
            builder.Services.AddSingleton<IDataStore<Hearing>>(sp => sp.GetRequiredService<HearingService>());

            var app = builder.Build();
            ServiceHelper.Initialize(app.Services);
            return app;
        }

        private static DataBase DataBaseFactory(IServiceProvider serviceProvider)
        {
            var dbPath = FileHelper.ObtenerRutaLocal("SIUGJ.db3");
            var database = new DataBase(dbPath);
            database.Conectar();
            return database;
        }
    }
}
