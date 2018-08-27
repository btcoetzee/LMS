// ReSharper disable ObjectCreationAsStatement
namespace LMS.LeadPublisher.UnitTests
{
    using System;
    using System.Collections.Generic;
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
                        .GetConstructor(new[] { typeof(IPublisher<ILeadEntity>), typeof(ILoggerClient)})
                        .GetParameters()[0]
                        .Name, ane.ParamName);
            }
        }

        [TestMethod]
        public void LeadPublisherConstructorNullLoggingClientTest()
        {
            try
            {
                new LeadPublisher(new Mock<IPublisher<ILeadEntity>>().Object, null);
            }
            catch (ArgumentNullException ane)
            {
                Assert.AreEqual(
                    typeof(LeadPublisher)
                        .GetConstructor(new[] { typeof(IPublisher<ILeadEntity>), typeof(ILoggerClient) })
                        .GetParameters()[1]
                        .Name, ane.ParamName);
            }
        }

        [TestMethod]
        public void LeadPublisherPublishLeadTest()
        {
            var testChannel = new InProcNotificationChannel<ILeadEntity>(new Mock<ILogger>().Object);
            var testPublisher = new Publisher<ILeadEntity>(new INotificationChannel<ILeadEntity>[] {testChannel}, true);
            var testSubscriber = new Subscriber<ILeadEntity>(testChannel, true);
            const string expectedMessage =
                "{\"Context\":null,\"Properties\":[{\"Id\":\"testKey\",\"Value\":\"testValue\"}],\"Segments\":null,\"Results\":null}";
           

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
            var actualLead = new TestLeadEntity(); 

            testSubscriber.AddOnReceiveActionToChannel(testlead => actualLead = testLead);

            leadPublisher.PublishLead(testLead);

            Thread.Sleep(5); //Speed bump to let the threads process.

            Assert.AreEqual(testLead, actualLead);
        }
    }

    #region Private Testing Classes

    internal struct TestLeadEntity : ILeadEntity
    {
  

        public IContext[] Context { get; set; }
        public IProperty[] Properties { get; set; }
        public ISegment[] Segments { get; set; }
        public IResultCollection ResultCollection { get; set; }
        public List<string> ErrorList { get; set; }
    }

    internal struct TestProperty : IProperty
    {
        public string Id { get; set; }
        public object Value { get; set;  }
    }

    #endregion
}
