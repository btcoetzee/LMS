namespace LMS.Validator.UnitTests
{
    using LMS.Modules.LeadEntity.Components;
    using LMS.Modules.LeadEntity.Interface;
    using LMS.Validator.Implementation.Validators;
    using LMS.Validator.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [TestClass]
    public class IdentityGuidValidatorTest
    {
        private static IServiceProvider _serviceProvider;
        private Mock<IValidator> _validator;
        private Mock<ILeadEntity> _leadEntity;
        private ILeadEntity _testLeadEntity;

        #region Initializer        
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _validator = new Mock<IValidator>();
            _leadEntity = new Mock<ILeadEntity>();
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
            public List<string> ErrorList { get; set; }
        }

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

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _validator.VerifyAll();
            _validator = null;
        }
        #endregion        
        /// <summary>
        /// Valids the quoted product validator.
        /// </summary>
        [TestMethod]
        public void ValidIdentityGuidValidator()
        {

            _testLeadEntity.Context = new IContext[] { new DefaultContext(Modules.LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()) };
            var identityGuidValidator = new IdentityGuidValidator();

            var actualValue = identityGuidValidator.ValidLead(_testLeadEntity);

            bool expectedValue = true;

            Assert.AreEqual(expectedValue, actualValue);
        }
        /// <summary>
        /// Invalids the quoted product validator key.
        /// </summary>
        [TestMethod]
        public void InvalidIdentityGuidValidatorKey()
        {

            _testLeadEntity.Context = new IContext[] { new DefaultContext(null, Guid.NewGuid().ToString()) };
            var identityGuidValidator = new IdentityGuidValidator();

            var actualValue = identityGuidValidator.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            Assert.AreEqual(expectedValue, actualValue);
        }
        /// <summary>
        /// Invalids the quoted product value validator.
        /// </summary>
        [TestMethod]
        public void InvalidIdentityGuidtValueValidator()
        {

            _testLeadEntity.Context = new IContext[] { new DefaultContext(Modules.LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey, 5) };
            var identityGuidValidator = new IdentityGuidValidator();

            var actualValue = identityGuidValidator.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void NullIdentityGuidValueValidator()
        {

            _testLeadEntity.Context = new IContext[] { new DefaultContext(Modules.LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey, null) };
            var identityGuidValidator = new IdentityGuidValidator();

            var actualValue = identityGuidValidator.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
