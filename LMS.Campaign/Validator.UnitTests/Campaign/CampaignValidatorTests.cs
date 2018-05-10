namespace Validator.UnitTests.Campaign
{
    using System;
    using System.IO;
    using global::Validator.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class CampaignValidatorTests
    {
        /// <summary> 
        /// The container 
        /// </summary> 
        private static IServiceProvider _validatorServiceProvider;
        //private static IServiceCollection _validatorService;

        private Mock<IValidator> _campaignValidator;

        [TestInitialize]
        public void Initialize()
        {
            _campaignValidator = new Mock<IValidator>();
            _validatorServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(IValidator), _campaignValidator.Object)
                .BuildServiceProvider();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _campaignValidator.VerifyAll();
            _campaignValidator = null;
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
            _campaignValidator.Setup(c => c.ValidLead(It.IsAny<Stream>())).Returns(expectedValue);

            // Invoke the ValidLead function
            var actualValue = validator.ValidLead(new MemoryStream());

            Assert.AreEqual(expectedValue, actualValue);

        }
    }
}

