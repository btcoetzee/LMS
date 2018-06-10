namespace Validator.UnitTests
{
    using System;
    using System.IO;
    using LeadEntity.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Validator.Interface;

    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ValidatorTests
    {
        /// <summary> 
        /// The container 
        /// </summary> 
        private static IServiceProvider _validatorServiceProvider;
        //private static IServiceCollection _validatorService;

        private Mock<IValidator> _validator;
        private Mock<ILeadEntity> _leadEntity;

        [TestInitialize]
        public void Initialize()
        {
            _validator = new Mock<IValidator>();
            _leadEntity = new Mock<ILeadEntity>();
            _validatorServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(IValidator), _validator.Object)
                .BuildServiceProvider();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _validator.VerifyAll();
            _validator = null;
            _validatorServiceProvider = null;
        }
        /// <summary>
        /// Tests the validLead function call.
        /// </summary>
        [TestMethod]
        public void TestLeadValidCall()
        {
            var validator = _validatorServiceProvider.GetService<IValidator>();
            bool expectedValue = true;

            // Mock the ValidLead Function to return true
            _validator.Setup(c => c.ValidLead(It.IsAny<ILeadEntity>())).Returns(expectedValue);

            
            // Invoke the ValidLead function
            var actualValue = validator.ValidLead(_leadEntity.Object);

            Assert.AreEqual(expectedValue, actualValue);

        }
    }
}

