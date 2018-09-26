using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Validator.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Components;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace LMS.IoC.Tests
{
    using System;
    using Admiral.Components.Instrumentation.Contract;
    using Compare.Components.Notification.Contract;
   
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
   
    using Moq;
  
    using StackExchange.Redis;

    [TestClass]
    public class BootstrapperExtensionsTests
    {
        private IServiceCollection _container;
        private ILeadEntity _testLeadEntity;
        private Mock<ILoggerClient> _loggingClient;
        private Mock<IValidator> _validator;
        private Mock<IPublisher> _publisher;
        private Mock<IDecorator> _decorator;
        private Mock<ISubscriber> _subscriber;

        [TestInitialize]
        public void Initialize()
        {
            _container = new ServiceCollection().AddLoggerClient();
            _loggingClient = new Mock<ILoggerClient>();
            _validator = new Mock<IValidator>();
            _publisher = new Mock<IPublisher>();
            _decorator = new Mock<IDecorator>();
            _subscriber = new Mock<ISubscriber>();

            CreateTestLeadEntity();
        }

        void CreateTestLeadEntity()
        {
            _testLeadEntity = new DefaultLeadEntity()
            {
                Context = new IContext[] { },
                Properties = new IProperty[] { },
                Segments = new ISegment[] { }
            };

        }

        [TestCleanup]
        public void Cleanup()
        {
            _validator.VerifyAll();
            _validator = null;
            _publisher.VerifyAll();
            _publisher = null;
            _decorator.VerifyAll();
            _decorator = null;
            _loggingClient.VerifyAll();
            _loggingClient = null;
        }

        [TestMethod]
        public void AddLoggerTest()
        {
            var provider = _container.AddLogger().BuildServiceProvider();

            var logger = provider.GetService<ILogger>();
            Assert.IsNotNull(logger);
        }

        [TestMethod]
        public void AddNotificationChannelTest()
        {
            var provider = _container.AddNotificationChannel().BuildServiceProvider();

            INotificationChannel<ILeadEntity> channel;

            try
            {
                //Can't build without the dependencies.
                channel = provider.GetService<INotificationChannel<ILeadEntity>>();
            }
            catch (InvalidOperationException ioe)
            {
                //Swallow the exception because we know what caused it.
            }

            //Add dependencies.
            provider = _container.AddLogger().BuildServiceProvider();

            channel = provider.GetService<INotificationChannel<ILeadEntity>>();
            Assert.IsNotNull(channel);
        }

        [TestMethod]
        public void AddNotificationSubscriberTest()
        {
            var provider = _container.AddNotificationSubscriber().BuildServiceProvider();

            ISubscriber<ILeadEntity> subscriber;

            try
            {
                //Can't build without the dependencies.
                subscriber = provider.GetService<ISubscriber<ILeadEntity>>();
            }
            catch (InvalidOperationException ioe)
            {
                //Swallow the exception because we know what caused it.
            }

            //Add dependencies.
            provider = _container
                .AddLogger()
                .AddNotificationChannel()
                .BuildServiceProvider();

            subscriber = provider.GetService<ISubscriber<ILeadEntity>>();
            Assert.IsNotNull(subscriber);
        }

        [TestMethod]
        public void AddNotificationPublisherTest()
        {
            var provider = _container.AddNotificationPublisher().BuildServiceProvider();

            IPublisher<ILeadEntity> publisher;

            try
            {
                //Can't build without the dependencies.
                publisher = provider.GetService<IPublisher<ILeadEntity>>();
            }
            catch (ArgumentException ae)
            {
                Assert.AreEqual("Cannot create a publisher with 0 channels. There must be at least one channel.",
                    ae.Message);
                //Swallow the exception because we know what caused it.
            }

            //Add dependencies.
            provider = _container
                .AddLogger()
                .AddNotificationChannel()
                .BuildServiceProvider();

            publisher = provider.GetService<IPublisher<ILeadEntity>>();
            Assert.IsNotNull(publisher);
            Assert.AreEqual(1, publisher.ChannelCount);
        }

        //[TestMethod]
        //public void AddLeadCollectorTest()
        //{
        //    var provider = _container.AddLeadValidator().AddLeadDecorator().AddLeadPublisher().AddLeadCollector()
        //        .BuildServiceProvider();

        //    var collector = provider.GetService<ILeadCollector>();

        //    //For now, we can just check to make sure that it's made.
        //    Assert.IsNotNull(collector);
        //}

        //[TestMethod]
        //public void AddLoggerClientTest()
        //{
        //    var provider = _container.AddLoggerClient().BuildServiceProvider();

        //    var logger = provider.GetRequiredService<ILoggerClient>();

        //    Assert.IsNotNull(logger);
        //    //This instance type will need to be updated if it changes in the future.
        //    Assert.IsInstanceOfType(logger, typeof(ConsoleLoggerClient));
        //}

        //[TestMethod]
        //public void AddLeadValidatorTest()
        //{
        //    var provider = _container.AddLeadValidator().BuildServiceProvider();

        //    var validator = provider.GetRequiredService<IValidator>();

        //    Assert.IsNotNull(validator);
        //    //This instance type will need to be updated if it changes in the future.
        //    Assert.IsInstanceOfType(validator, typeof(LeadValidator));
        //}

        //[TestMethod]
        //public void AddLeadDecoratorTest()
        //{
        //    var provider = _container.AddLeadDecorator().BuildServiceProvider();

        //    var decorator = provider.GetRequiredService<IDecorator>();

        //    Assert.IsNotNull(decorator);
        //    //This instance type will need to be updated if it changes in the future.
        //    Assert.IsInstanceOfType(decorator, typeof(LeadDecorator));
        //}

        //[TestMethod]
        //public void AddAddCampaignManagerSubscriberTest()
        //{
        //    var provider = _container.AddCampaignManagerSubscriber().BuildServiceProvider();

        //    var campaignManagerSubscriber = provider.GetRequiredService<ISubscriber<ILeadEntity>>();

        //    Assert.IsNotNull(campaignManagerSubscriber);
        //    //This instance type will need to be updated if it changes in the future.
        //    Assert.IsInstanceOfType(campaignManagerSubscriber, typeof(CampaignManagerSubscriber));
        //}
    }
}
