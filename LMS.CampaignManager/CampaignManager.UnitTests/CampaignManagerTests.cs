namespace CampaignManager.UnitTests
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using Compare.Components.Notification.Contract;
    using Compare.Components.Notification.Subscribers;
    using LMS.LoggerClient.Interface;
    using Campaign.Interface;

    [TestClass]
    public class CampaignManagerTests
    {

        private static IServiceProvider _campaignManagerServiceProvider;
        private Mock<ISubscriber<string>> _notificationSubscriber;
        private Mock<IList<ICampaignSubscriber>> _campaingSubscriberList;
        private Mock<ILoggerClient> _loggerClient;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Campaign, Validator, Publisher and Decorator
            _notificationSubscriber = new Mock<ISubscriber<string>>();
            _campaingSubscriberList = new Mock<IList<ICampaignSubscriber>>();
            _loggerClient = new Mock<ILoggerClient>();
          

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
            var campaignManager = new LMS.CampaignManager.CampaignManager(_notificationSubscriber.Object, _campaingSubscriberList.Object,  _loggerClient.Object);
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
                var campaignManager = new LMS.CampaignManager.CampaignManager(null, _campaingSubscriberList.Object, _loggerClient.Object);
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
                var campaignManager = new LMS.CampaignManager.CampaignManager(_notificationSubscriber.Object, null, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the CampaignSubscriberList is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignSubscriberList", exception.Message.Replace(Environment.NewLine, " "));
            }
         }

        /// <summary>
        /// Campaing Constructor Test with a Null CampaignSubscriberList.
        /// </summary>
        [TestMethod]
        public void CampainConstructorNullCampaignSubscriberListTest()
        {

            try
            {
                var campaignManager = new LMS.CampaignManager.CampaignManager(_notificationSubscriber.Object, null, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the CampaignSubscriberList is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignSubscriberList", exception.Message.Replace(Environment.NewLine, " "));
            }
        }
    }
}