using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LMS.CampaignManager.Publisher.UnitTests
{
  
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq.Expressions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using LMS.LoggerClient.Interface;
    using LMS.LeadEntity.Interface.Constants;
    using LMS.LeadEntity.Interface;
    using LMS.LeadEntity.Components;

    using LMS.CampaignManager.Publisher.Interface;
    using LMS.CampaignManager.Publisher.Implementation;

    [TestClass]
    public class CampaignManagerPublisherTests
    {

        private static IServiceProvider _campaignManagerServiceProvider;
        private Mock<ICampaignManagerPublisher> _campaignManagerPublisher;
        private Mock<ILoggerClient> _loggerClient;
        private DefaultLeadEntity _testLeadEntity;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Components that is part of the CampaignManager Publisher

            _campaignManagerPublisher = new Mock<ICampaignManagerPublisher>();
            _loggerClient = new Mock<ILoggerClient>();


            // Create Service Providers 
            _campaignManagerServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ICampaignManagerPublisher), _campaignManagerPublisher)
                .AddSingleton(typeof(ILoggerClient), _loggerClient.Object)
                .BuildServiceProvider();

            // Create a leadEntity
            CreateLeadEntity();

        }

        void CreateLeadEntity()
        {
            _testLeadEntity = new DefaultLeadEntity()
            {
                Context = new IContext[] { },
                Properties = new IProperty[] { },
                Segments = new ISegment[] { },
                ResultCollection = new DefaultResultCollection()
            };
        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _campaignManagerPublisher.VerifyAll();
            _campaignManagerPublisher = null;
            _loggerClient.VerifyAll();
            _loggerClient = null;
            _campaignManagerServiceProvider = null;
        }

 #region ConstructorTests

        /// <summary>
        /// Campaign Manager Publisher Constructor Test
        /// </summary>
        [TestMethod]
        public void CampaignManagerPublisherConstructorTest()
        {
            var campaignManager = new CampaignManagerPublisher(_loggerClient.Object);
        }

        /// <summary>
        /// Campaign Manager Publisher Constructor Test With Null Logger
        /// </summary>
        [TestMethod]
        public void CampaignManagerPublisherConstructorTestWithNullLoggerClient()
        {
            try
            {
                var campaignManager = new CampaignManagerPublisher(null);
                Assert.Fail("An Argument Null Exception is expected when the LoggerClient is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: loggerClient", exception.Message.Replace(Environment.NewLine, " "));
            }
        }
 #endregion
        /// <summary>
        /// Campaign Manager Publisher PublishLead Test
        /// </summary>
        [TestMethod]
        public void CampaignManagerPublisheLeadTest()
        {
            var campaignManager = new CampaignManagerPublisher(_loggerClient.Object);
            campaignManager.PublishLead(_testLeadEntity);
        }
 
    }
}
