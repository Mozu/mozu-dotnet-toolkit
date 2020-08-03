using System;
using System.Collections.Generic;
using System.IO;
//using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
//using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mozu.Api.Contracts.AppDev;
using Mozu.Api.Events;
using Mozu.Api.Logging;
using Mozu.Api.Security;
using Mozu.Api.ToolKit.Config;
using Mozu.Api.ToolKit.Configuration;
using Mozu.Api.ToolKit.Events;
using Mozu.Api.ToolKit.Handlers;
using Mozu.Api.ToolKit.Logging;

namespace Mozu.Api.ToolKit
{
    //public abstract class AbstractBootstrapper
    //{
    //    protected readonly ContainerBuilder _containerBuilder = new ContainerBuilder();
    //    public IContainer Container;

    //    public AbstractBootstrapper Bootstrap()
    //    {
    //        InitDependencyResolvers();

    //        return this;
    //    }

    //    private void InitDependencyResolvers()
    //    {

    //        var appName = ConfigurationManager.AppSettings["AppName"];
    //        var configPath = ConfigurationManager.AppSettings["ConfigPath"];
    //        var environment = ConfigurationManager.AppSettings["Environment"];

    //        if (!string.IsNullOrEmpty(appName) && !string.IsNullOrEmpty(configPath) &&
    //            !String.IsNullOrEmpty(environment))
    //        {
    //            var appParams = new List<NamedParameter>
    //            {

    //                new NamedParameter("configPath",  configPath),
    //                new NamedParameter("appName", appName),
    //                new NamedParameter("environment", environment)
    //            };

    //            _containerBuilder.RegisterType<AppSetting>().As<IAppSetting>().SingleInstance().WithParameters(appParams);

    //        }
    //        else
    //        {
    //            _containerBuilder.RegisterType<AppSetting>().As<IAppSetting>().SingleInstance();
    //        }
    //        _containerBuilder.RegisterType<Log4NetServiceFactory>().As<ILoggingServiceFactory>().SingleInstance();
    //        if (ConfigurationManager.AppSettings.AllKeys.Contains("UseGenericEventService") &&
    //            bool.Parse(ConfigurationManager.AppSettings["UseGenericEventService"]))
    //            _containerBuilder.RegisterType<Events.GenericEventService>().As<IEventService>();
    //        else
    //            _containerBuilder.RegisterType<Events.EventService>().As<IEventService>();

    //        _containerBuilder.RegisterType<Events.EventServiceFactory>().As<IEventServiceFactory>();

    //        _containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
    //           .Where(t => t.Name.EndsWith("Handler"))
    //           .AsImplementedInterfaces();

    //        _containerBuilder.RegisterType<EntitySchemaHandler>().As<IEntitySchemaHandler>();
    //        _containerBuilder.RegisterType<ReturnEventProcessor>().Keyed<IEventProcessor>(EventCategory.Return);
    //        _containerBuilder.RegisterType<ProductEventProcessor>().Keyed<IEventProcessor>(EventCategory.Product);
    //        _containerBuilder.RegisterType<OrderEventProcessor>().Keyed<IEventProcessor>(EventCategory.Order);
    //        _containerBuilder.RegisterType<ApplicationEventProcessor>().Keyed<IEventProcessor>(EventCategory.Application);
    //        _containerBuilder.RegisterType<CustomerAccountEventProcessor>().Keyed<IEventProcessor>(EventCategory.CustomerAccount);
    //        _containerBuilder.RegisterType<DiscountEventProcessor>().Keyed<IEventProcessor>(EventCategory.Discount);
    //        _containerBuilder.RegisterType<CustomerSegmentEventProcessor>().Keyed<IEventProcessor>(EventCategory.CustomerSegment);
    //        _containerBuilder.RegisterType<TenantEventProcessor>().Keyed<IEventProcessor>(EventCategory.Tenant);
    //        _containerBuilder.RegisterType<EmailEventProcessor>().Keyed<IEventProcessor>(EventCategory.Email);
    //        _containerBuilder.RegisterType<ProductInventoryEventProcessor>().Keyed<IEventProcessor>(EventCategory.ProductInventory);


    //        InitializeContainer(_containerBuilder);

    //        Container = _containerBuilder.Build();

    //        LogManager.LoggingService = Container.Resolve<ILoggingServiceFactory>().GetLoggingService();
    //        var appSetting = Container.Resolve<IAppSetting>();

    //        if (!string.IsNullOrEmpty(appSetting.ApplicationId) && !string.IsNullOrEmpty(appSetting.SharedSecret))
    //        {
    //            if (!string.IsNullOrEmpty(appSetting.BaseUrl))
    //                MozuConfig.BaseAppAuthUrl = appSetting.BaseUrl;

    //            if (!string.IsNullOrEmpty(appSetting.BasePCIUrl))
    //                MozuConfig.BasePciUrl = appSetting.BasePCIUrl;

    //            var appAuthenticator = AppAuthenticator.InitializeAsync(new AppAuthInfo { ApplicationId = appSetting.ApplicationId, SharedSecret = appSetting.SharedSecret }).Result;
    //        }


    //        PostInitialize();


    //    }

    //    public virtual void InitializeContainer(ContainerBuilder containerBuilder)
    //    {
    //    }

    //    public virtual void PostInitialize()
    //    {

    //    }
    //}

    public abstract class AbstractBootstrapper: IDependencyConfigurator
    {
        //protected readonly ContainerBuilder _containerBuilder = new ContainerBuilder();
        //public IContainer Container;
        private readonly IConfigurationRoot _configuration;
        public delegate IEventProcessor ServiceResolver(string key);
        public AbstractBootstrapper()
        {
            _configuration = ConfigHelper.GetConfigBuilder("appsettings.json");
        }

        public AbstractBootstrapper Bootstrap(IServiceCollection _containerBuilder)
        {
            Configure(_containerBuilder);
            return this;
        }
        public virtual void Configure(IServiceCollection _containerBuilder)
        {
            var appName = _configuration["appSettings:AppName"];
            var configPath = _configuration["appSettings:ConfigPath"];
            var environment = _configuration["appSettings:Environment"];
            //var UseGenericEventService = _configuration.GetSection("appSettings").GetChildren()
            //    .Where(x => x.Key.Equals("UseGenericEventService"))
            //    .Select(y => y.Value).First();

            var UseGenericEventService = _configuration.GetValueFromAppSettings("UseGenericEventService");



            if (!string.IsNullOrEmpty(appName) && !string.IsNullOrEmpty(configPath) &&
                !String.IsNullOrEmpty(environment))
            {
                _containerBuilder.AddSingleton<IAppSetting>(x => new AppSetting(configPath, appName, environment));
            }
            else
            {
                _containerBuilder.AddSingleton<IAppSetting,AppSetting>();
            }

            //_containerBuilder.AddSingleton<Log4NetServiceFactory, ILoggingServiceFactory>();
            if( !(string.IsNullOrEmpty(UseGenericEventService)))
            {
                if(bool.Parse(UseGenericEventService))
                    _containerBuilder.AddTransient<IEventService, Events.GenericEventService>();
                else
                    _containerBuilder.AddTransient<IEventService, Events.EventService>();
            }
            else
                _containerBuilder.AddTransient<IEventService, Events.EventService>();

            _containerBuilder.AddTransient<IEventServiceFactory, Events.EventServiceFactory>();

            //var classes = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass).Where(y=>y.Name.EndsWith("Handler"));
            //var interfaces= Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsInterface).Where(y => y.Name.EndsWith("Handler"));

            //foreach(Type t in classes)
            //{
            //    var intface = interfaces.Where(x => x.Name.Equals("I" + t.Name));
            //    _containerBuilder.AddTransient<Activator.CreateInstance(interface), t.Name>();
            //}
            _containerBuilder.AddTransient<IEmailHandler, EmailHandler>();
            _containerBuilder.AddTransient<IEntityHandler, EntityHandler>();
            _containerBuilder.AddTransient<IEntitySchemaHandler, EntitySchemaHandler>();
            _containerBuilder.AddTransient<ISiteHandler, SiteHandler>();
            _containerBuilder.AddTransient<ISubnavLinkHandler, SubnavLinkHandler>();

            

            _containerBuilder.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                switch (int.Parse(key))
                {
                    case 0:
                        return serviceProvider.GetService<OrderEventProcessor>();
                    case 1:
                        return serviceProvider.GetService<ReturnEventProcessor>();
                    case 2:
                        return serviceProvider.GetService<CustomerAccountEventProcessor>();
                    case 3:
                        return serviceProvider.GetService<ProductEventProcessor>();
                    case 4:
                        return serviceProvider.GetService<DiscountEventProcessor>();
                    case 5:
                        return serviceProvider.GetService<ApplicationEventProcessor>();
                    case 6:
                        return serviceProvider.GetService<CustomerSegmentEventProcessor>();
                    case 7:
                        return serviceProvider.GetService<TenantEventProcessor>();
                    case 8:
                        return serviceProvider.GetService<EmailEventProcessor>();
                    case 9:
                        return serviceProvider.GetService<ProductInventoryEventProcessor>();
                    default:
                        throw new KeyNotFoundException(); // or maybe return null, up to you
                }
            });
            //Need to figure out later
            var provider = _containerBuilder.BuildServiceProvider();
            var appSetting = provider.GetService<IAppSetting>();//new AppSetting();//Container.Resolve<IAppSetting>();

            if (!string.IsNullOrEmpty(appSetting.ApplicationId) && !string.IsNullOrEmpty(appSetting.SharedSecret))
            {
                if (!string.IsNullOrEmpty(appSetting.BaseUrl))
                    MozuConfig.BaseAppAuthUrl = appSetting.BaseUrl;

                if (!string.IsNullOrEmpty(appSetting.BasePCIUrl))
                    MozuConfig.BasePciUrl = appSetting.BasePCIUrl;

                var appAuthenticator = AppAuthenticator.InitializeAsync(new AppAuthInfo { ApplicationId = appSetting.ApplicationId, SharedSecret = appSetting.SharedSecret }).Result;
            }
        }
    }
}
