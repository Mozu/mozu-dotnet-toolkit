using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mozu.Api.ToolKit.Handlers;
using Mozu.Api.ToolKit.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Mozu.Api.ToolKit.Test
{
    [TestClass]
    public class EmailHandlerTest : BaseTest
    {
        private IEmailHandler _emailHandler;
        [TestInitialize]
        public void Setup()
        {
            _emailHandler = provider.GetService<IEmailHandler>();
        }

        [TestMethod]
        public void TestErrorEmail()
        {
            _emailHandler.SendErrorEmail(new ErrorInfo{Message = "Event order.opened failed", Context = new ApiContext(18037), Exception = new Exception("test")});
        }
    }
}
