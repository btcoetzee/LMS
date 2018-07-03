namespace LMS.IoC.Tests
{
    using System;
    using Admiral.Components.Instrumentation.Contract;
    using Compare.Components.Notification.Contract;
    using LeadCollector.Interface;
    using LMS.Validator.Interface;
    using LMS.LeadValidator.Implementation;
    using LoggerClient.Console;
    using LoggerClient.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BootstrapperExtensionsTests
    {
        private IServiceCollection _container;

        [TestInitialize]
        public void Initialize()
        {
            _container = new ServiceCollection();
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

            INotificationChannel<string> channel;

            try
            {
                //Can't build without the dependencies.
                channel = provider.GetService<INotificationChannel<string>>();
            }
            catch (InvalidOperationException ioe)
            {
                //Swallow the exception because we know what caused it.
            }

            //Add dependencies.
            provider = _container.AddLogger().BuildServiceProvider();

            channel = provider.GetService<INotificationChannel<string>>();
            Assert.IsNotNull(channel);
        }

        [TestMethod]
        public void AddNotificationSubscriberTest()
        {
            var provider = _container.AddNotificationSubscriber().BuildServiceProvider();

            ISubscriber<string> subscriber;

            try
            {
                //Can't build without the dependencies.
                subscriber = provider.GetService<ISubscriber<string>>();
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

            subscriber = provider.GetService<ISubscriber<string>>();
            Assert.IsNotNull(subscriber);
        }

        [TestMethod]
        public void AddNotificationPublisherTest()
        {
            var provider = _container.AddNotificationPublisher().BuildServiceProvider();

            IPublisher<string> publisher;

            try
            {
                //Can't build without the dependencies.
                publisher = provider.GetService<IPublisher<string>>();
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

            publisher = provider.GetService<IPublisher<string>>();
            Assert.IsNotNull(publisher);
            Assert.AreEqual(1, publisher.ChannelCount);
        }

        [TestMethod]
        public void AddLeadCollectorTest()
        {
            var provider = _container.AddLeadValidator().AddLeadDecorator().AddLeadPublisher().AddLeadCollector()
                .BuildServiceProvider();

            var collector = provider.GetService<ILeadCollector>();

            //For now, we can just check to make sure that it's made.
            Assert.IsNotNull(collector);
        }

        [TestMethod]
        public void AddLoggerClientTest()
        {
            var provider = _container.AddLoggerClient().BuildServiceProvider();

            var logger = provider.GetRequiredService<ILoggerClient>();

            Assert.IsNotNull(logger);
            //This instance type will need to be updated if it changes in the future.
            Assert.IsInstanceOfType(logger, typeof(ConsoleLoggerClient));
        }

        [TestMethod]
        public void AddLeadValidatorTest()
        {
            var provider = _container.AddLeadValidator().BuildServiceProvider();

            var validator = provider.GetRequiredService<IValidator>();

            Assert.IsNotNull(validator);
            //This instance type will need to be updated if it changes in the future.
            Assert.IsInstanceOfType(validator, typeof(LeadValidator));
        }
    }
}
