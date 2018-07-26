namespace LMS.Filter.UnitTests.Filter
{
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;
    using LMS.Filter.Interface;
    using LMS.LeadEntity.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using LMS.CampaignValidator.Interface;

    /// <summary>
    /// Filter Unit Tests
    /// </summary>
    [TestClass]
    public class FilterTests
    {
        private static IServiceProvider _filterServiceProvider;
        private Mock<IFilter> _filter;
        private Mock<ICampaignValidator> _validator;
        private Mock<ILeadEntityImmutable> _leadEntity;
        private Mock<List<IResult>> _resultList;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Filter and Validator
            _filter = new Mock<IFilter>();
            _validator = new Mock<ICampaignValidator>();
            _leadEntity = new Mock<ILeadEntityImmutable>();
            _resultList = new Mock<List<IResult>>();

            // Create Service Providers for Filter and Validator
            _filterServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(IFilter), _filter.Object)
                .AddSingleton(typeof(ICampaignValidator), _validator.Object)
                .BuildServiceProvider(); 
        }
        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _filter.VerifyAll();
            _filter = null;
            _validator.VerifyAll();
            _validator = null;
            _filterServiceProvider = null;
        }

        /// <summary>
        /// Tests the ProcessLead in Filter Interface.
        /// </summary>
        [TestMethod]
        public void TestIfLeadProcessedInFilterCall()
        {
            var Filter = _filterServiceProvider.GetService<IFilter>();
            const string expectedMessage = "Lead was processed";
            string actualMessage = "";

            // Mock the ProcessLead function to update the message
            _filter.Setup(c => c.ClearedFilter(It.IsAny<ILeadEntityImmutable>())).Callback(() => {
                actualMessage = expectedMessage;
            });

            Filter.ClearedFilter(_leadEntity.Object);
            Assert.AreEqual(expectedMessage, actualMessage);
        }
        /// <summary>
        /// Tests the Filter lead Validation with leads.
        /// </summary>
        [TestMethod]
        public void TestLeadValidationForAFilter()
        {
            // A Filter
            var Filter = _filterServiceProvider.GetService<IFilter>();

            // the validator
            var validator = _filterServiceProvider.GetService<ICampaignValidator>();

            // Set up the messages for Filter to return
            const string expectedValidLeadMessage = "Valid Lead";
            const string expectedInvalidLeadMessage = "Invalid Lead";
            string actualMessage = string.Empty;

            // Set up return values when the validator is invoked
            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntityImmutable>())).Returns<ILeadEntityImmutable>(s => {
                if (s == null)
                    return false;
                else
                    return true;
             });

            // Tie the Filter to call out to the validator
            _filter.Setup(c => c.ClearedFilter(It.IsAny<ILeadEntityImmutable>())).Callback<ILeadEntityImmutable>(s => {
                if(validator.ValidLead(s))
                    actualMessage = expectedValidLeadMessage;
                else
                    actualMessage = expectedInvalidLeadMessage;
            });

            // Send a valid stream parameter
            Filter.ClearedFilter(_leadEntity.Object);
            Assert.AreEqual(expectedValidLeadMessage, actualMessage);

            // Send a null value parameter
            Filter.ClearedFilter(null);
            Assert.AreEqual(expectedInvalidLeadMessage, actualMessage);
        }
 
    }
}
