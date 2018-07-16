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
 
    [TestClass]
    public class CampaignManagerSubscriberTests
    {

        private static IServiceProvider _campaignManagerSubscriberServiceProvider;
        private Mock<ISubscriber<string>> _notificationSubscriber;
        private Mock<ICampaignManager> _campaignManager;
        private Mock<ILoggerClient> _loggerClient;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Notification Subscriber, CampaignManager and Logger Client
            _notificationSubscriber = new Mock<ISubscriber<string>>();
            _campaignManager = new Mock<ICampaignManager>();
            _loggerClient = new Mock<ILoggerClient>();

            // Create Service Providers 
            _campaignManagerSubscriberServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ISubscriber<string>), _notificationSubscriber.Object)
                .AddSingleton(typeof(ICampaignManager), _campaignManager.Object)
                .AddSingleton(typeof(ILoggerClient), _loggerClient.Object)
                .BuildServiceProvider();
        }
        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _notificationSubscriber.VerifyAll();
            _notificationSubscriber = null;
            _campaignManager.VerifyAll();
            _campaignManager = null;
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
            var campaignManagerSubscriber = new CampaignManagerSubscriber(_notificationSubscriber.Object, _campaignManager.Object, _loggerClient.Object);
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
                var campaignManagerSubscriber = new CampaignManagerSubscriber(null, _campaignManager.Object, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the NotificationSubscriber is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: notificationSubscriber", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        /// <summary>
        /// Campaing Constructor Test with a Null Campaign List.
        /// </summary>
        [TestMethod]
        public void CampaignManagerSubscriberConstructorNullCampaignManagerTest()
        {

            try
            {
                var campaignManagerSubscriber = new CampaignManagerSubscriber(_notificationSubscriber.Object, null, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the CampaignManager is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignManager", exception.Message.Replace(Environment.NewLine, " "));
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
                var campaignManagerSubscriber = new CampaignManagerSubscriber(_notificationSubscriber.Object, _campaignManager.Object, null);
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
        /// Campaign Manager Receive Lead Test
        /// </summary>
        [TestMethod]
        public void CampaignManagerSubscriberReceiveLead()
        {
            const string testMessage = "This is a Lead";
            string[] campaignOutputArray = new string[]
                {"Campaign 1 processed Lead", "Campaign 2 processed Lead", "Campaign 3 processed Lead"};
            _campaignManager.Setup(cm => cm.ProcessCampaigns((It.IsIn(testMessage))))
                .Returns(campaignOutputArray);
            var campaignManagerSubscriber = new CampaignManagerSubscriber(_notificationSubscriber.Object,
                _campaignManager.Object, _loggerClient.Object);
            campaignManagerSubscriber.ReceiveLead(testMessage);
        }

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
