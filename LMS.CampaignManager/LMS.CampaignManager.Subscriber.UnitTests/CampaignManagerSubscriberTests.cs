using LMS.CampaignManager.Subscriber.Interface;

namespace LMS.CampaignManager.Subscriber.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Admiral.Components.Instrumentation.Contract;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Compare.Components.Notification.Channels.InProc;
    using Compare.Components.Notification.Contract;
    using Compare.Components.Notification.Publishers;
    using Compare.Components.Notification.Subscribers;
    using LMS.LoggerClient.Interface;
    using LMS.CampaignManager.Subscriber.Implementation;
    using LMS.CampaignManager.Interface;
    using LMS.LeadEntity.Interface;

    [TestClass]
    public class CampaignManagerSubscriberTests
    {
        private static IServiceProvider _campaignManagerSubscriberServiceProvider;
        private Mock<ISubscriber<ILeadEntity>> _notificationSubscriber;
        private Mock<ILoggerClient> _loggerClient;
        private ILeadEntity _testLleadEntity;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Notification Subscriber, CampaignManager and Logger Client
            _notificationSubscriber = new Mock<ISubscriber<ILeadEntity>>();
            _loggerClient = new Mock<ILoggerClient>();

            // Create Service Providers 
            _campaignManagerSubscriberServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ISubscriber<ILeadEntity>), _notificationSubscriber.Object)
                .AddSingleton(typeof(ILoggerClient), _loggerClient.Object)
                .BuildServiceProvider();
            // Create a leadEntity
            CreateLeadEntity();
        }

        /// <summary>
        /// Create an Instance of the LeadEntity
        /// </summary>
        private class TestLeadEntityClass : ILeadEntity
        {
            public bool isValid()
            {
                throw new NotImplementedException();
            }

            public IContext[] Context { get; set; }
            public IProperty[] Properties { get; set; }
            public ISegment[] Segments { get; set; }
            public IResults[] Results { get; set; }
        }
        void CreateLeadEntity()
        {
            _testLleadEntity = new TestLeadEntityClass()
            {
                Context = new IContext[]
                    {},
                Properties = new IProperty[]
                    {},
                Segments = new ISegment[]
                    {},
                Results = new IResults[]
                    {}
            };

        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _notificationSubscriber.VerifyAll();
            _notificationSubscriber = null;
            _loggerClient.VerifyAll();
            _loggerClient = null;
            _campaignManagerSubscriberServiceProvider = null;
        }

     #region ConstructorTests
        /// <summary>
        /// Campaing Constructor Test
        /// </summary>
        [TestMethod]
        public void CampaignManagerSubscriberConstructorTest()
        {
            var campaignManagerSubscriber = new CampaignManagerSubscriber(_notificationSubscriber.Object, _loggerClient.Object);
        }

        /// <summary>
        /// Campaing Constructor Test with a Null NotificationSubscriber.
        /// </summary>
        [TestMethod]
        // [ExpectedException(typeof(ArgumentNullException))]
        public void CampaignManagerSubscriberConstructorNullNotificationTest()
        {
            try
            {
                var campaignManagerSubscriber = new CampaignManagerSubscriber(null, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the NotificationSubscriber is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: notificationSubscriber", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        /// <summary>
        /// Campaing Constructor Test with a Null LoggerClient.
        /// </summary>
        [TestMethod]
        public void CampaignManagerSubscriberConstructorNullLoggerClientTest()
        {
            try
            {
                var campaignManagerSubscriber = new CampaignManagerSubscriber(_notificationSubscriber.Object, null);
                Assert.Fail("An Argument Null Exception is expected when the LoggerClient is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: loggerClient", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        #endregion

        #region ReceiveLeadTests

        /// <summary>
        /// Campaign Manager AddOnReceiveActionToChannel Test
        /// </summary>
        //[TestMethod]
        //public void CampaignManagerSubscriberAddOnReceiveActionToChannel()
        //{
        //    EventArgs 
        //    var campaignManagerSubscriber = new Mock<ICampaignManagerSubscriber>();

        //    campaignManagerSubscriber.Setup(svc => svc.SetupAddOnReceiveActionToChannel(It.IsAny<Action<ILeadEntity>>()))
        //        .Callback((Action<ILeadEntity> action) => action(_testLleadEntity));
        //    _notificationSubscriber.Raise(x => x.S);
            
          
     

        //}

        //const string testMessage = "This is a Lead";
        //    string[] campaignOutputArray = new string[]
        //        {"Campaign 1 processed Lead", "Campaign 2 processed Lead", "Campaign 3 processed Lead"};
        //    _campaignManager.Setup(cm => cm.ProcessCampaigns((It.IsIn(testMessage))))
        //        .Returns(campaignOutputArray);
        //    var campaignManagerSubscriber = new CampaignManagerSubscriber(_notificationSubscriber.Object,
        //        _campaignManager.Object, _loggerClient.Object);
        //    campaignManagerSubscriber.AddOnReceiveActionToChannel();.ReceiveLead(testMessage);
        //}

        //[TestMethod]
        //public void CampaignManagerProcessMultipleCampaignTest()
        //{
        //    #region Notification Setup

        //    INotificationChannel<string> channel = new InProcNotificationChannel<string>(new Mock<ILogger>().Object);
        //    var publisher = new Publisher<string>(new[] { channel }, true);
        //    var subscriber = new Subscriber<string>(channel, true);

        //    #endregion

        //    var random = new Random();
        //    const int minSleepInMs = 1000;
        //    const int maxSleepInMs = 5000;

        //    const string testMessage = "Hello from Milo! :-)";
        //    const string responseFormat = "Campaign {0} complete.";

        //    const int campaignCount = 3;
        //    var campaignSubscribers = new ICampaignSubscriber[campaignCount];

        //    for (var i = 0; i < campaignCount; i++)
        //    {
        //        var id = i;
        //        var mock = new Mock<ICampaignSubscriber>();
        //        mock.Setup(campaign => campaign.ReceiveLead(It.IsIn(testMessage)))
        //            .Callback(() => Thread.Sleep(random.Next(minSleepInMs, maxSleepInMs)))
        //            .Returns(string.Format(responseFormat, id));
        //        campaignSubscribers[i] = mock.Object;
        //    }

        //    var campaignManager = new CampaignManager(subscriber, campaignSubscribers, new Mock<ILoggerClient>().Object);

        //    var results = campaignManager.ProcessCampaigns("TestMessage");
        //}

     
     #endregion


    }
}
