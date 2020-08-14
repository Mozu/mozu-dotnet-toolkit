using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mozu.Api.ToolKit
{
    public static class ConfigHelper
    {
        
        public static IConfigurationRoot GetConfigBuilder(string config,string location=null)
        {
            if (string.IsNullOrEmpty(location))
            {
                location = Directory.GetCurrentDirectory();
            }
            return new ConfigurationBuilder()
                       .SetBasePath(location)
                       .AddJsonFile(config, optional: true, reloadOnChange: false)
                       .Build();
        }

        public static string GetValueFromAppSettings(this IConfigurationRoot configuration,string key)
        {
            var section = configuration.GetSection("appSettings").GetChildren();
            foreach(var item in section)
            {
                if (item.Key.Equals(key))
                {
                    return item.Value;
                }
            }
            return string.Empty;
        }
        
    }
}
