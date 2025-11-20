using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace SIUGJ.Helpers
{
    public static class ServiceHelper
    {
        private static IServiceProvider? serviceProvider;

        public static void Initialize(IServiceProvider provider)
        {
            serviceProvider = provider;
        }

        public static T GetRequiredService<T>() where T : notnull
        {
            return Services.GetRequiredService<T>();
        }

        public static T? GetService<T>()
        {
            return Services.GetService<T>();
        }

        private static IServiceProvider Services
            => serviceProvider
               ?? throw new InvalidOperationException("No se pudo resolver el ServiceProvider actual");
    }
}
