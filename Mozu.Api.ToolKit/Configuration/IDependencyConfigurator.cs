using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mozu.Api.ToolKit.Configuration
{
    interface IDependencyConfigurator
    { 
        public void Configure(IServiceCollection _containerBuilder);
    }
}
