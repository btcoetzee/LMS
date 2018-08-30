namespace LMS.DataProviderUnitTest
{
    using LMS.DataProvider.ValidatorCollection;
    using LMS.LeadEntity.Components;
    using LMS.LeadEntity.Interface;
    using LMS.Validator.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    [TestClass]
    public class ActivityGuidValidatorTest
    {
        private static IServiceProvider _serviceProvider;
        private Mock<IValidator> _validator;
        private Mock<ILeadEntity> _leadEntity;
        private ILeadEntity _testLeadEntity;

        string activityIdKey = LMS.LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey;

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
        /// Valids the activity unique identifier validator.
        /// </summary>
        [TestMethod]
        public void ValidActivityGuidValidator()
        {
  
            _testLeadEntity.Context = new IContext[] { new DefaultContext(LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()) };
            var activityGuidValidator = new ActivityGuidValidator();

            var actualValue = activityGuidValidator.ValidLead(_testLeadEntity);

            bool expectedValue = true;

            Assert.AreEqual(expectedValue, actualValue);
        }
        /// <summary>
        /// Invalids the activity unique identifier validator key.
        /// </summary>
        [TestMethod]
        public void InvalidActivityGuidValidatorKey()
        {

            _testLeadEntity.Context = new IContext[] { new DefaultContext(null, Guid.NewGuid().ToString()) };
            var activityGuidValidator = new ActivityGuidValidator();

            var actualValue = activityGuidValidator.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            Assert.AreEqual(expectedValue, actualValue);
        }
        /// <summary>
        /// Invalids the activity unique identifier value validator.
        /// </summary>
        [TestMethod]
        public void InvalidActivityGuidValueValidator()
        {

            _testLeadEntity.Context = new IContext[] { new DefaultContext(LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey, 5) };
            var activityGuidValidator = new ActivityGuidValidator();

            var actualValue = activityGuidValidator.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            Assert.AreEqual(expectedValue, actualValue);
        }
        /// <summary>
        /// Nulls the activity unique identifier value validator.
        /// </summary>
        [TestMethod]
        public void NullActivityGuidValueValidator()
        {

            _testLeadEntity.Context = new IContext[] { new DefaultContext(LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey, null) };
            var activityGuidValidator = new ActivityGuidValidator();

            var actualValue = activityGuidValidator.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
