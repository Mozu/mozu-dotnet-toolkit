using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Mozu.Api.Contracts.Event;
using Mozu.Api.Events;
//using Mozu.Api.Logging;
using Mozu.Api.ToolKit.Handlers;
using Mozu.Api.ToolKit.Models;
using Newtonsoft.Json;
using Mozu.Api.ToolKit.Logging;
using Microsoft.Extensions.Logging;

namespace Mozu.Api.ToolKit.Events
{
    public class GenericEventService : IEventService
    {
        private readonly ILogger _logger = LoggingService.LoggerFor(typeof(EventService));
        private readonly IServiceProvider _container;
        private readonly IEmailHandler _emailHandler;

        public GenericEventService(IServiceProvider container, IEmailHandler emailHandler)
        {
            _container = container;
            _emailHandler = emailHandler;
           
        }

        public async Task ProcessEventAsync(IApiContext apiContext, Event eventPayLoad)
        {
            try
            {
                var eventProcessor = _container.GetService<IGenericEventProcessor>();
                await eventProcessor.ProcessEvent(apiContext, eventPayLoad);
            }
            catch (Exception exc)
            {
                _emailHandler.SendErrorEmail(new ErrorInfo { Message = "Error Processing Event : " + JsonConvert.SerializeObject(eventPayLoad), Context = apiContext, Exception = exc });
                throw;
            }
        }
    }
}
