using System.Collections.Generic;

namespace LMS.LeadCollector.UnitTests
{

    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.Validator.Interface;
    using LMS.Publisher.Interface;
    using LMS.LoggerClient.Interface;

    [TestClass]
    public class LeadCollectorUnitTests
    {
        private static IServiceProvider _serviceProvider;
        private Mock<ILeadEntity> _leadEntity;
        private Mock<List<IResult>> _resultList;
        private Mock<IValidator> _validator;
        private Mock<IPublisher> _publisher;
        private Mock<IDecorator> _decorator;
        private Mock<ILoggerClient> _loggingClient;

        [TestInitialize]
        public void Initialize()
        {
            _leadEntity = new Mock<ILeadEntity>();
            _resultList = new Mock<List<IResult>>();
            _validator = new Mock<IValidator>();
            _publisher = new Mock<IPublisher>();
            _decorator = new Mock<IDecorator>();
            _loggingClient = new Mock<ILoggerClient>();

            _serviceProvider = new ServiceCollection().AddSingleton(typeof(IValidator), _validator.Object)
                .AddSingleton(typeof(IPublisher), _publisher.Object)
                .AddSingleton(typeof(IDecorator), _decorator.Object)
                .AddSingleton(typeof(ILoggerClient), _loggingClient.Object).BuildServiceProvider();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _leadEntity.VerifyAll();
            _leadEntity = null;
            _resultList.VerifyAll();
            _resultList = null;
            _validator.VerifyAll();
            _validator = null;
            _publisher.VerifyAll();
            _publisher = null;
            _decorator.VerifyAll();
            _decorator = null;
            _loggingClient.VerifyAll();
            _loggingClient = null;
        }

        /// <summary>
        /// Test that the ValidateLead function call is invoked as expected.
        /// </summary>
        [TestMethod]
        public void TestLeadToLeadCollectorLeadValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            bool expectedValue = true;

            // Mock the ValidateLead Function and verify that it was called as expected.
            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(expectedValue);

            // Invoke the ValidLead function
            var actualValue = validator.ValidLead(_leadEntity.Object);

            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Test that the DecorateLead function call is invoked as expected.
        /// </summary>
        [TestMethod]
        public void TestLeadToLeadCollectorLeadDecorator()
        {
            var decorator = _serviceProvider.GetService<IDecorator>();

            // Mock the DecorateLead Function and verify that it was called as expected.
            _decorator.Setup(c => c.DecorateLead(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>>())).Verifiable();

            // Invoke the Decorate function
            decorator.DecorateLead(_leadEntity.Object, _resultList.Object);
        }

        /// <summary>
        /// Test that the PublishLead function call is invoked as expected.
        /// </summary>
        [TestMethod]
        public void TestLeadToLeadCollectorLeadPublisher()
        {
            var publisher = _serviceProvider.GetService<IPublisher>();

            // Mock the PublishLead Function and verify that it was called as expected.
            _publisher.Setup(c => c.PublishLead(It.IsAny<ILeadEntity>())).Verifiable();

            // Invoke the Publish function
            publisher.PublishLead(_leadEntity.Object);
        }

        [TestMethod]
        public void TestLeadToLeadCollector()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            bool expectedValue = true;

            // Mock the ValidateLead Function and verify that it was called as expected.
            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(expectedValue);

            // Invoke the ValidLead function
            var actualValue = validator.ValidLead(_leadEntity.Object);

            Assert.AreEqual(expectedValue, actualValue);

            var decorator = _serviceProvider.GetService<IDecorator>();

            // Mock the DecorateLead Function and verify that it was called as expected.
            _decorator.Setup(c => c.DecorateLead(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>>())).Verifiable();

            // Invoke the Decorate function
            decorator.DecorateLead(_leadEntity.Object, _resultList.Object);

            var publisher = _serviceProvider.GetService<IPublisher>();

            // Mock the PublishLead Function and verify that it was called as expected.
            _publisher.Setup(c => c.PublishLead(It.IsAny<ILeadEntity>())).Verifiable();

            // Invoke the Publish function
            publisher.PublishLead(_leadEntity.Object);
        }

        [TestMethod]
        public void TestLeadFalseToLeadCollector()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            bool expectedValue = false;

            // Mock the ValidateLead Function and verify that it was called as expected.
            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(expectedValue);

            _decorator.Verify(v => v.DecorateLead(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>>()), Times.Never());

            _publisher.Verify(v => v.PublishLead(It.IsAny<ILeadEntity>()), Times.Never());

            // Invoke the ValidLead function
            var actualValue = validator.ValidLead(_leadEntity.Object);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AllNullParamLeadCollectorConstructorTest()
        {
            new Implementation.LeadCollector(null, null, null, null);
        }

        [TestMethod]
        public void MockedParamLeadCollectorConstructorTest()
        {
            new Implementation.LeadCollector(_serviceProvider.GetService<IValidator>(), _serviceProvider.GetService<IDecorator>(),
                _serviceProvider.GetService<IPublisher>(), _serviceProvider.GetService<ILoggerClient>());
        }
    }
}
