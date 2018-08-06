using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LMS.CampaignManager.Decorator.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq.Expressions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using LMS.LoggerClient.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LeadEntity.Components;
    using LMS.CampaignManager.Decorator.Interface;
    [TestClass]
    public class CampaignManagerDecoratorTests
    {
        private static IServiceProvider _campaignManagerServiceProvider;
        private Mock<ICampaignManagerDecorator> _campaignManagerDecorator;
        private Mock<ILoggerClient> _loggerClient;
        private ILeadEntity _testLleadEntity;
        private List<IResult> _testCampaignResultList;
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
