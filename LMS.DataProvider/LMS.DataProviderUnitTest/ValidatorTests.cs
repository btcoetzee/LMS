namespace LMS.DataProviderUnitTest
{
    using LMS.DataProvider;
    using LMS.DataProvider.ValidatorCollection;
    using LMS.DataProvider.Validators;
    using LMS.LeadEntity.Components;
    using LMS.LeadEntity.Interface;
    using LMS.Validator.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public class ValidatorTests
    {
        private static IServiceProvider _serviceProvider;
        private Mock<IValidator> _validator;
        private Mock<ILeadEntity> _leadEntity;
        private ILeadEntity _testLeadEntity;

        string addressKey = "990 Main St";

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

        #region ValidatorTest
        [TestMethod]
        public void ActivityGuidValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Context = new IContext[] { new DefaultContext(LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()) };
            var activityGuid = new ActivityGuidValidator();

            activityGuid.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void AdditionalProductsValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Context = new IContext[] { new DefaultContext(LeadEntity.Interface.Constants.ContextKeys.AdditionalProductKey, 101) };
            var additonalProducts = new AdditionalProductsValidator();

            additonalProducts.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void AddressValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Properties = new IProperty[] { new DefaultProperty(LeadEntity.Interface.Constants.PropertyKeys.AddressKey, addressKey) };
            var address = new AddressValidator();           

            address.ValidLead(_testLeadEntity);
            
            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void EmailAddressValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Properties = new IProperty[] { new DefaultProperty(LeadEntity.Interface.Constants.PropertyKeys.EmailAddressKey, "google@gmail.com") };
            var address = new EmailAddressValidator();

            address.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void EnvironmentValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Context = new IContext[] { new DefaultContext(LeadEntity.Interface.Constants.ContextKeys.EnvironmentKey, "PT") };
            var address = new EnvironmentValidator();

            address.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void FullNameValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Properties = new IProperty[] { new DefaultProperty(LeadEntity.Interface.Constants.PropertyKeys.FullNameKey, "PT") };
            var address = new FullNameValidator();

            address.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void HighPOPValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Segments = new ISegment[] { new DefaultSegment(LeadEntity.Interface.Constants.SegementKeys.HighPOPKey) };
            var address = new HighPOPValidator();

            address.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void HomewOwnerValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Segments = new ISegment[] { new DefaultSegment(LeadEntity.Interface.Constants.SegementKeys.HomeownerKey) };
            var address = new HomeOwnerValidator();

            address.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void IdentityGuidValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Context = new IContext[] { new DefaultContext(LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()) };
            var identityGuid = new IdentityGuidValidator();

            identityGuid.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void LowPOPValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Segments = new ISegment[] { new DefaultSegment(LeadEntity.Interface.Constants.SegementKeys.LowPOPKey) };
            var lowPOP = new LowPOPValidator();

            lowPOP.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void PNIBirthDateValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Properties = new IProperty[] { new DefaultProperty(LeadEntity.Interface.Constants.PropertyKeys.PNI_Age, 32) };
            var pniAge = new PNIBirthDateValidator();

            pniAge.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void PhoneNumberValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Properties = new IProperty[] { new DefaultProperty(LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber, "888-888-8888") };
            var phoneNumber = new PhoneNumberValidator();

            phoneNumber.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void PriorBIValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Properties = new IProperty[] { new DefaultProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorBIKey, "25/50") };
            var priorBI = new PriorBIValidator();

            priorBI.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void PriorInsuranceValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Properties = new IProperty[] { new DefaultProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorInsuranceKey, true) };
            var priorInsurance = new PriorInsuranceValidator();

            priorInsurance.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void QuotedBIValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Properties = new IProperty[] { new DefaultProperty(LeadEntity.Interface.Constants.PropertyKeys.QuotedBIKey, "100/300") };
            var quotedBI = new QuotedBIValidator();

            quotedBI.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void QuotedProductValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Context = new IContext[] { new DefaultContext(LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey, 101) };
            var quotedProduct = new QuotedProductValidator();

            quotedProduct.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void RenterValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Segments = new ISegment[] { new DefaultSegment(LeadEntity.Interface.Constants.SegementKeys.RenterKey) };
            var renter = new RenterValidator();

            renter.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void SessionGuidValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Context = new IContext[] { new DefaultContext(LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey, Guid.NewGuid().ToString()) };
            var sessionGuid = new SessionGuidValidator();

            sessionGuid.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void SessionRequestSequenceValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Context = new IContext[] { new DefaultContext(LeadEntity.Interface.Constants.ContextKeys.SessionRequestSeqKey, 5) };
            var sessionRequestSequence = new SessionRequestSequenceValidator();

            sessionRequestSequence.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void SiteIdValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Context = new IContext[] { new DefaultContext(LeadEntity.Interface.Constants.ContextKeys.SiteIDKey, 26238) };
            var siteId = new SiteIdValidator();

            siteId.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void StateIdValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Properties = new IProperty[] { new DefaultProperty(LeadEntity.Interface.Constants.PropertyKeys.StateKey, "VA") };
            var stateId = new StateValidator();

            stateId.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void VehicleCountValidator()
        {
            var validator = _serviceProvider.GetService<IValidator>();
            _testLeadEntity.Properties = new IProperty[] { new DefaultProperty(LeadEntity.Interface.Constants.PropertyKeys.VehicleCountKey, 1) };
            var vehicleCount = new VehicleCountValidator();

            vehicleCount.ValidLead(_testLeadEntity);

            bool expectedValue = false;

            var actualValue = validator.ValidLead(_testLeadEntity);

            Assert.AreEqual(expectedValue, actualValue);
        }

        #endregion
    }
}
