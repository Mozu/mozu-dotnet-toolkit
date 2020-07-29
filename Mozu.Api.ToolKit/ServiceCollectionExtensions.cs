using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mozu.Api.ToolKit
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMozuLogging(this IServiceCollection services)
        {
            foreach (var registration in services.Where(x =>
              x.ServiceType == typeof(ConsoleLoggerProvider) ||
              x.ImplementationType == typeof(ConsoleLoggerProvider) ||
                x.ServiceType == typeof(JsonConsoleLoggerProvider) ||
              x.ImplementationType == typeof(JsonConsoleLoggerProvider)).Distinct().ToList())
            {
                services.Remove(registration);
            }
            services.AddSingleton<ILoggerProvider, JsonConsoleLoggerProvider>();
            services.AddSingleton<JsonConsoleLoggerProvider.JsonConsoleLoggerSettings>();
            return services;
        }
    }
}
