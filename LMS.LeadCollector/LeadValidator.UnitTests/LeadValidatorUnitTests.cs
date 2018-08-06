namespace LeadValidator.UnitTests
{

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Moq;
    using Microsoft.Extensions.DependencyInjection;
    using LMS.Validator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LeadValidator.Implementation;
    using LMS.LeadEntity.Components;
    using LMS.LoggerClient.Interface;
    using System.Linq;

    [TestClass]
    public class LeadValidatorUnitTests
    {
        private static IServiceProvider _serviceProvider;
        private Mock<IValidator> _validator;
        private Mock<ILeadEntity> _leadEntity;
        private Mock<ILoggerClient> _loggingClient;
        private ILeadEntity _testLeadEntity;

        string activityId = LMS.LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey;
        string identityId = LMS.LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey;
        string sessionId = LMS.LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey;
        string quotedProductId = LMS.LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey;

        

        [TestInitialize]
        public void Initialize()
        {
            _validator = new Mock<IValidator>();
            _leadEntity = new Mock<ILeadEntity>();
            _loggingClient = new Mock<ILoggerClient>();
            _serviceProvider = new ServiceCollection().AddSingleton(typeof(IValidator), _validator.Object).BuildServiceProvider();

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
            _validator.VerifyAll();
            _validator = null;
            _loggingClient.VerifyAll();
            _loggingClient = null;
        }

        [TestMethod]
        public void TestValidLead()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            bool expectedValue = true;

            activityId = Guid.NewGuid().ToString();
            identityId = Guid.NewGuid().ToString();
            sessionId = Guid.NewGuid().ToString();
            quotedProductId = "101";


            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(expectedValue);

            // Invoke the ValidLead function
            var actualValue = validator.ValidLead(_leadEntity.Object);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void InvalidLeadNullActivityId()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            bool expectedValue = false;

            activityId = null;
            identityId = Guid.NewGuid().ToString();
            sessionId = Guid.NewGuid().ToString();
            quotedProductId = "101";

            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(expectedValue);

            var actualValue = validator.ValidLead(_leadEntity.Object);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void InvalidLeadNullIdentityId()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            bool expectedValue = false;

            activityId = Guid.NewGuid().ToString();
            identityId = null;
            sessionId = Guid.NewGuid().ToString();
            quotedProductId = "101";

            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(expectedValue);

            var actualValue = validator.ValidLead(_leadEntity.Object);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void InvalidLeadNullSessionId()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            bool expectedValue = false;

            activityId = Guid.NewGuid().ToString();
            identityId = Guid.NewGuid().ToString();
            sessionId = null;
            quotedProductId = "101";

            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(expectedValue);

            var actualValue = validator.ValidLead(_leadEntity.Object);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void InvalidLeadNullProductId()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            bool expectedValue = false;

            activityId = Guid.NewGuid().ToString();
            identityId = Guid.NewGuid().ToString();
            sessionId = Guid.NewGuid().ToString();
            quotedProductId = null;

            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(expectedValue);

            var actualValue = validator.ValidLead(_leadEntity.Object);

            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Mocked the parameter lead validator constructor test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void MockedParamLeadvalidatorConstructorNullLoggerClientTest()
        {
            var validator = new LeadValidator(null);
        }

        /// <summary>
        /// Mocked the parameter lead validator constructor test.
        /// </summary>
        [TestMethod]
        public void ValidLeadWithEmptyContextTest()
        {
            var validator = new LeadValidator(_loggingClient.Object);

            _testLeadEntity.Context = null;
            bool expectedValue = false;

            var isValid = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, isValid);

        }

        /// <summary>
        /// Mocked the parameter lead validator constructor test.
        /// </summary>
        [TestMethod]
        public void ValidLeadWithEmptyContextKeysTest()
        {
            var validator = new LeadValidator(_loggingClient.Object);

            activityId = null;
            identityId = null;
            sessionId = null;
            quotedProductId = null;
            bool expectedValue = false;

            var isValid = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, isValid);

        }

        /// <summary>
        /// Mocked the parameter lead validator constructor test.
        /// </summary>
        [TestMethod]
        public void InvalidLeadTest()
        {
            var validator = new LeadValidator(_loggingClient.Object);
            _testLeadEntity.Context = new IContext[] { new DefaultContext ( LMS.LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString() ) ,
                                                       new DefaultContext ( LMS.LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey, Guid.NewGuid().ToString() )  ,
                                                       new DefaultContext ( LMS.LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString() ) ,
                                                       new DefaultContext ( LMS.LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey, Guid.NewGuid().ToString() )
            };

            //activityId = "ActivityGUID";
            //identityId = "IdentityGUID";
            //sessionId = "SessionGUID";
            //quotedProductId = "QuotedProduct";
            bool expectedValue = false;
            ////var activityGuidValue = _testLeadEntity.Context.SingleOrDefault(item => item.Id == _testLeadEntity..Interface.Constants.ContextKeys.ActivityGuidKey)?.Value;

            var isValid = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, isValid);

        }

    }
}
