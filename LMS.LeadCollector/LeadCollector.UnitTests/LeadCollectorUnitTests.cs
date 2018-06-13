namespace LeadCollector.UnitTests
{
    using Decorator.Interface;
    using LeadEntity.Interface;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Publisher.Interface;
    using System;
    using Validator.Interface;
    using System.IO;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;


    [TestClass]
    public class LeadCollectorUnitTests
    {
        private static IServiceProvider _serviceProvider;
        private Mock<ILeadEntity> _leadEntity;
        private Mock<IValidator> _validator;
        private Mock<IPublisher> _publisher;
        private Mock<IDecorator> _decorator;

        [TestInitialize]
        public void Initialize()
        {
            _leadEntity = new Mock<ILeadEntity>();
            _validator = new Mock<IValidator>();
            _publisher = new Mock<IPublisher>();
            _decorator = new Mock<IDecorator>();

            _serviceProvider = new ServiceCollection().AddSingleton(typeof(IValidator), _validator.Object)
                .AddSingleton(typeof(IPublisher), _publisher.Object)
                .AddSingleton(typeof(IDecorator), _decorator.Object).BuildServiceProvider();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _leadEntity.VerifyAll();
            _leadEntity = null;
            _validator.VerifyAll();
            _validator = null;
            _publisher.VerifyAll();
            _publisher = null;
            _decorator.VerifyAll();
            _decorator = null;
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
            _decorator.Setup(c => c.DecorateLead(It.IsAny<ILeadEntity>())).Verifiable();

            // Invoke the Decorate function
            decorator.DecorateLead(_leadEntity.Object);
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
            _decorator.Setup(c => c.DecorateLead(It.IsAny<ILeadEntity>())).Verifiable();

            // Invoke the Decorate function
            decorator.DecorateLead(_leadEntity.Object);

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

            // Invoke the ValidLead function
            var actualValue = validator.ValidLead(_leadEntity.Object);

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
