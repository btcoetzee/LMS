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
    using LMS.CampaignManager.Subscriber.Interface;
    using LMS.LeadEntity.Components;


    [TestClass]
    public class CampaignManagerSubscriberTests
    {
        private static IServiceProvider _campaignManagerSubscriberServiceProvider;
        private Mock<ISubscriber<ILeadEntity>> _notificationSubscriber;
        private Mock<ILoggerClient> _loggerClient;
        private ILeadEntity _testLleadEntity = new DefaultLeadEntity();

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
            //CreateLeadEntity();
        }

        ///// <summary>
        ///// Create an Instance of the LeadEntity
        ///// </summary>
        //private class TestLeadEntityClass : ILeadEntity
        //{
 
        //    public IContext[] Context { get; set; }
        //    public IProperty[] Properties { get; set; }
        //    public ISegment[] Segments { get; set; }
        //    public IResultCollection ResultCollection { get; set; }
        //}
        //void CreateLeadEntity()
        //{
        //    _testLleadEntity = new TestLeadEntityClass()
        //    {
        //        Context = new IContext[]
        //            {},
        //        Properties = new IProperty[]
        //            {},
        //        Segments = new ISegment[]
        //            {},
        //        ResultCollection = new DefaultResultCollection()
        //    };
 

        //}

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
        /// Campaign Manager Subscriber Constructor Test
        /// </summary>
        [TestMethod]
        public void CampaignManagerSubscriberConstructorTest()
        {
            var campaignManagerSubscriber = new CampaignManagerSubscriber(_notificationSubscriber.Object, _loggerClient.Object);
        }

        /// <summary>
        /// Campaign Manager Subscriber Constructor Test with a Null NotificationSubscriber.
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
        /// Campaign Manager Subscriber Constructor Test with a Null LoggerClient.
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

 #region SetupAddOnReceiveActionToChannel

        /// <summary>
        /// Campaign Manager Subscriber AddOnReceiveActionToChannel Test
        /// </summary>
        [TestMethod]
        public void CampaignManagerSubscriberAddOnReceiveActionToChannel()
        {

            var testNotificationChannel = new InProcNotificationChannel<ILeadEntity>(new Mock<ILogger>().Object);
            var testNotificationPublisher =
                new Publisher<ILeadEntity>(new INotificationChannel<ILeadEntity>[] {testNotificationChannel}, true);
            var testNotificationSubscriber = new Subscriber<ILeadEntity>(testNotificationChannel, true);

            // Set up the action to be invoked when a leadEntity is received
            bool actionWasInvoked = false;
            Action<ILeadEntity> leadEntityReceiveAction = campaignManagerDriver => actionWasInvoked = true;

            // Setup the Subscriber to execute the action when invoked
            var campaignManagerSubscriber =
                new CampaignManagerSubscriber(testNotificationSubscriber, _loggerClient.Object);
            campaignManagerSubscriber.SetupAddOnReceiveActionToChannel(leadEntityReceiveAction);

            // Let the Notification Publisher broadcast a leadEntity
            testNotificationPublisher.BroadcastMessage(_testLleadEntity);

            // Sleep a little while for the Notification Channel to pick up the LeadEntity
            Thread.Sleep(5);

            // The subscriber should now have executed the action
            Assert.AreEqual(true, actionWasInvoked);

        }
#endregion


    }
}
