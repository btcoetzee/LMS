// ReSharper disable ObjectCreationAsStatement
namespace LMS.LeadPublisher.UnitTests
{
    using System;
    using System.Threading;
    using Admiral.Components.Instrumentation.Contract;
    using Compare.Components.Notification.Channels.InProc;
    using Compare.Components.Notification.Contract;
    using Compare.Components.Notification.Publishers;
    using Compare.Components.Notification.Subscribers;
    using Implementation;
    using LeadEntity.Interface;
    using LoggerClient.Interface;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class LeadPublisherUnitTests
    {
        [TestMethod]
        public void LeadPublisherConstructorNullNotificationPublisherTest()
        {
            try
            {
                new LeadPublisher(null, new Mock<ILoggerClient>().Object);
            }
            catch (ArgumentNullException ane)
            {
                Assert.AreEqual(
                    typeof(LeadPublisher)
                        .GetConstructor(new[] { typeof(IPublisher<string>), typeof(ILoggerClient)})
                        .GetParameters()[0]
                        .Name, ane.ParamName);
            }
        }

        [TestMethod]
        public void LeadPublisherConstructorNullLoggingClientTest()
        {
            try
            {
                new LeadPublisher(new Mock<IPublisher<string>>().Object, null);
            }
            catch (ArgumentNullException ane)
            {
                Assert.AreEqual(
                    typeof(LeadPublisher)
                        .GetConstructor(new[] { typeof(IPublisher<string>), typeof(ILoggerClient) })
                        .GetParameters()[1]
                        .Name, ane.ParamName);
            }
        }

        [TestMethod]
        public void LeadPublisherPublishLeadTest()
        {
            var testChannel = new InProcNotificationChannel<string>(new Mock<ILogger>().Object);
            var testPublisher = new Publisher<string>(new INotificationChannel<string>[] {testChannel}, true);
            var testSubscriber = new Subscriber<string>(testChannel, true);
            const string expectedMessage = "{\"Context\":null,\"Properties\":[{\"Id\":\"testKey\",\"Value\":\"testValue\"}],\"Segments\":null}";
            var actualMessage = string.Empty;

            var leadPublisher = new LeadPublisher(testPublisher, new Mock<ILoggerClient>().Object);

            var testProperty = new TestProperty
            {
                Id = "testKey",
                Value = "testValue"
            };

            var testLead = new TestLeadEntity
            {
                Properties = new IProperty[] {testProperty}
            };

            testSubscriber.AddOnReceiveActionToChannel(message => actualMessage = message);

            leadPublisher.PublishLead(testLead);

            Thread.Sleep(5); //Speed bump to let the threads process.

            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }

    #region Private Testing Classes

    internal struct TestLeadEntity : ILeadEntity
    {
        public bool isValid()
        {
            return true;
        }

        public IContext[] Context { get; set; }
        public IProperty[] Properties { get; set; }
        public ISegment[] Segments { get; set; }
    }

    internal struct TestProperty : IProperty
    {
        public string Id { get; set; }
        public object Value { get; set;  }
    }

    #endregion
}
