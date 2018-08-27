namespace LMS.LeadCollector.UnitTests
{

    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.Validator.Interface;
    using LMS.Publisher.Interface;
    using LMS.LoggerClient.Interface;
    using LMS.LeadEntity.Components;
    using LMS.LeadCollector.Implementation;

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
        private ILeadEntity _testLeadEntity;

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

            CreateLeadEntity();
        }

        /// <summary>
        /// Create an Instance of the LeadEntity
        /// </summary>
        private class TestLeadEntityClass : ILeadEntity
        {

            public IContext[] Context { get; set; }
            public IProperty[] Properties { get; set; }
            public ISegment[] Segments { get; set; }
            public IResultCollection ResultCollection { get; set; }
            public List<string> ErrorList { get; set; }
        }

        //struct TestLeadEntityResultClass : IResult
        //{
        //    public TestLeadEntityResultClass(string id, object value)
        //    {
        //        Id = id;
        //        Value = value;
        //    }

        //    public string Id { get; private set; }

        //    public object Value { get; private set; }
        //}

        void CreateLeadEntity()
        {
            _testLeadEntity = new TestLeadEntityClass()
            {
                Context = new IContext[]
                    {},
                Properties = new IProperty[]
                    {},
                Segments = new ISegment[]
                    {},
                ResultCollection = new DefaultResultCollection()
            };

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

        #region ConstructorTests

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

        /// <summary>
        /// Mocked the parameter lead collector constructor test.
        /// </summary>
        [TestMethod]
        public void MockedParamLeadCollectorConstructorTest()
        {
            new Implementation.LeadCollector(_serviceProvider.GetService<IValidator>(), _serviceProvider.GetService<IDecorator>(),
                _serviceProvider.GetService<IPublisher>(), _serviceProvider.GetService<ILoggerClient>());
        }

        /// <summary>
        /// Mocked the parameter lead collector constructor test. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void MockedParamLeadCollectorConstructorNullValidatorTest()
        {
            new Implementation.LeadCollector(null, _serviceProvider.GetService<IDecorator>(),
                _serviceProvider.GetService<IPublisher>(), _serviceProvider.GetService<ILoggerClient>());
        }

        /// <summary>
        /// Mocked the parameter lead collector constructor test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void MockedParamLeadCollectorConstructorNullDecoratorTest()
        {
            new Implementation.LeadCollector(_serviceProvider.GetService<IValidator>(), null,
                _serviceProvider.GetService<IPublisher>(), _serviceProvider.GetService<ILoggerClient>());
        }

        /// <summary>
        /// Mocked the parameter lead collector constructor test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void MockedParamLeadCollectorConstructorNullPublisherTest()
        {
            new Implementation.LeadCollector(_serviceProvider.GetService<IValidator>(), _serviceProvider.GetService<IDecorator>(),
                null, _serviceProvider.GetService<ILoggerClient>());
        }

        /// <summary>
        /// Mocked the parameter lead collector constructor test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void MockedParamLeadCollectorConstructorNullLoggerClientTest()
        {
            new Implementation.LeadCollector(_serviceProvider.GetService<IValidator>(), _serviceProvider.GetService<IDecorator>(),
                _serviceProvider.GetService<IPublisher>(), null);
        }

        /// <summary>
        /// Mocked the parameter lead collector constructor test.
        /// </summary>
        [TestMethod]
        public void CollectLeadNullLeadEntityTest()
        {
            try
            {
                var collectLead = new LeadCollector(_serviceProvider.GetService<IValidator>(), _serviceProvider.GetService<IDecorator>(),
             _serviceProvider.GetService<IPublisher>(), _serviceProvider.GetService<ILoggerClient>());

                collectLead.CollectLead(null);

            }
            catch (Exception ex)
            {

                Assert.AreEqual(typeof(ArgumentNullException), ex.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: leadEntity", ex.Message.Replace(Environment.NewLine, " "));
            }
         
        }

        /// <summary>
        /// Mocked the parameter lead collector constructor test.
        /// </summary>
        [TestMethod]
        public void CollectLeadLeadEntityInvalidTest()
        {
            var collectLead = new LeadCollector(_serviceProvider.GetService<IValidator>(), _serviceProvider.GetService<IDecorator>(),
                _serviceProvider.GetService<IPublisher>(), _serviceProvider.GetService<ILoggerClient>());

            var validator = _serviceProvider.GetService<IValidator>();
            bool expectedValue = false;

            // Mock the ValidateLead Function and verify that it was called as expected.
            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(expectedValue);


            collectLead.CollectLead(_testLeadEntity);
        }

        /// <summary>
        /// Mocked the parameter lead collector constructor test.
        /// </summary>
        [TestMethod]
        public void CollectLeadLeadEntityValidTest()
        {
            var collectLead = new LeadCollector(_serviceProvider.GetService<IValidator>(), _serviceProvider.GetService<IDecorator>(),
                _serviceProvider.GetService<IPublisher>(), _serviceProvider.GetService<ILoggerClient>());

            var validator = _serviceProvider.GetService<IValidator>();
            bool expectedValue = true;

            var publisher = _serviceProvider.GetService<IPublisher>();

            // Mock the ValidateLead Function and verify that it was called as expected.
            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(expectedValue);
            _publisher.Setup(p => p.PublishLead(It.IsAny<ILeadEntity>()));

            collectLead.CollectLead(_testLeadEntity);
        }
    }
}
#endregion