namespace LMS.CampaignManager.UnitTests
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using Compare.Components.Notification.Contract;
    using Compare.Components.Notification.Subscribers;
    using LMS.Campaign.Interface;
    using LMS.LoggerClient.Interface;

    [TestClass]
    public class CampaignManagerTests
    {

        private static IServiceProvider _campaignManagerServiceProvider;
        private Mock<ISubscriber<string>> _notificationSubscriber;
        private Mock<List<ICampaignSubscriber>> _campaingSubscriberList;
        private Mock<ILoggerClient> _loggerClient;
        private static readonly string[] EmptyResultArray = new string[] { };

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Campaign, Validator, Publisher and Decorator
            _notificationSubscriber = new Mock<ISubscriber<string>>();
            _campaingSubscriberList = new Mock<List<ICampaignSubscriber>>();
            _loggerClient = new Mock<ILoggerClient>();
          
            var campaingSubscriber = new Mock<ICampaignSubscriber>();

            // Create Service Providers 
            _campaignManagerServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ISubscriber<string>), _notificationSubscriber.Object)
                .AddSingleton(typeof(IList<ICampaignSubscriber>), _campaingSubscriberList.Object)
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
            _campaingSubscriberList.VerifyAll();
            _campaingSubscriberList = null;
            _loggerClient.VerifyAll();
            _loggerClient = null;
            _campaignManagerServiceProvider = null;
        }

        /// <summary>
        /// Campaing Constructor Test
        /// </summary>
        [TestMethod]
        public void CampaignConstructorTest()
        {
            var campaignManager = new Implementation.CampaignManager(_notificationSubscriber.Object, _campaingSubscriberList.Object,  _loggerClient.Object);
        }

        /// <summary>
        /// Campaing Constructor Test with a Null NotificationSubscriber.
        /// </summary>
        [TestMethod]
       // [ExpectedException(typeof(ArgumentNullException))]
        public void CampaignConstructorNullNotificationTest()
        {
            try
            {
                var campaignManager = new Implementation.CampaignManager(null, _campaingSubscriberList.Object, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the NotificationSubscriber is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: notificationSubscriber", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        /// <summary>
        /// Campaing Constructor Test with a Null CampaignSubscriberList.
        /// </summary>
        [TestMethod]
         public void CampaignConstructorNullCampaignSubscriberListTest()
         {

            try
            {
                var campaignManager = new Implementation.CampaignManager(_notificationSubscriber.Object, null, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the CampaignSubscriberList is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignSubscriberList", exception.Message.Replace(Environment.NewLine, " "));
            }
         }
        /// <summary>
        /// Campaing Constructor Test with a Null LoggerClient.
        /// </summary>
        [TestMethod]
        public void CampaignConstructorNullLoggerClientTest()
        {

            try
            {
                var campaignManager = new Implementation.CampaignManager(_notificationSubscriber.Object, _campaingSubscriberList.Object, null);
                Assert.Fail("An Argument Null Exception is expected when the LoggerClient is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: loggerClient", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        /// <summary>
        /// Campaing Constructor Test with a No Campaigns.
        /// </summary>
        [TestMethod]
        public void CampaignProcessEmptyCampaignTest()
        {
            // No Campaigns set up, so result list should be empty
            var campaignManager = new Implementation.CampaignManager(_notificationSubscriber.Object, _campaingSubscriberList.Object, _loggerClient.Object);
            var resultList = campaignManager.ProcessCampaigns("This is the lead");
            Assert.AreEqual(resultList.Length, 0);
        }

        /// <summary>
        /// Campaing Constructor Test with a Null CampaignSubscriberList.
        /// </summary>
        //[TestMethod]
        //  TBD public void CampaignProcessMultipleCampaignTest()
        //{
        //    // Setup some Campaigns 
        //    var campaignSubscriber = new ICampaignSubscriber()
        //    var campaignManager = new Implementation.CampaignManager(_notificationSubscriber.Object, _campaingSubscriberList.Object, _loggerClient.Object);
        //    var resultList = campaignManager.ProcessCampaigns("This is the lead");
        //    Assert.AreEqual(resultList.Length, 0);
        //}
    }
}