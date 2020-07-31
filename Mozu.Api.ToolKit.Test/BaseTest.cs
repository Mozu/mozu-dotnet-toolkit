using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
//using Autofac;
using Mozu.Api.ToolKit.Config;

namespace Mozu.Api.ToolKit.Test
{
    public class BaseTest
    {

        //public IContainer Container;
        public IServiceProvider provider;
        protected int TenantId;
        public BaseTest()
        {

            var services = new ServiceCollection();
            var bootstrapper = new Bootstrapper();
            bootstrapper.Configure(services);
            provider = services.BuildServiceProvider();
            var appSetting = provider.GetService<IAppSetting>();
            TenantId = int.Parse(appSetting.Settings["TenantId"].ToString());
        }
    }
}
