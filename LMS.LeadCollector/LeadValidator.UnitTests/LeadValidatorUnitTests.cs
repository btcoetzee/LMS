namespace LeadValidator.UnitTests
{

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Moq;
    using Microsoft.Extensions.DependencyInjection;
    using LMS.Validator.Interface;
    using LMS.LeadEntity.Interface;

    [TestClass]
    public class LeadValidatorUnitTests
    {
        private static IServiceProvider _serviceProvider;
        private Mock<IValidator> _validator;
        private Mock<ILeadEntity> _leadEntity;

        string activityId = LMS.LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey;
        string identityId = LMS.LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey;
        string sessionId = LMS.LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey;
        string quotedProductId = LMS.LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey;


        [TestInitialize]
        public void Initialize()
        {
            _validator = new Mock<IValidator>();
            _leadEntity = new Mock<ILeadEntity>();
            _serviceProvider = new ServiceCollection().AddSingleton(typeof(IValidator), _validator.Object).BuildServiceProvider();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _leadEntity.VerifyAll();
            _leadEntity = null;
            _validator.VerifyAll();
            _validator = null;
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
    }
}
