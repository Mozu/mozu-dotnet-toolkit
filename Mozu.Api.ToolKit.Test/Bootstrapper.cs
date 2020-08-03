using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Autofac;

namespace Mozu.Api.ToolKit.Test
{
    public class Bootstrapper : AbstractBootstrapper
    {
        public override void Configure(IServiceCollection services)
        {
            base.Configure(services);
        }
    }
}
