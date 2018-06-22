namespace LMS.IoC.Tests
{
    using System;
    using Admiral.Components.Instrumentation.Contract;
    using Compare.Components.Notification.Contract;
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
                //Can't build without the ILogger dependency.
                channel = provider.GetService<INotificationChannel<string>>();
            }
            catch (ArgumentNullException ane)
            {
                //Swallow the exception because we know what caused it.
            }

            provider = _container.AddLogger().BuildServiceProvider();

            channel = provider.GetService<INotificationChannel<string>>();
            Assert.IsNotNull(channel);
        }

        
    }
}
