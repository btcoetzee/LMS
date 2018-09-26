//using System.Collections.Generic;
//using Compare.Services.LMS.Controls.Validator.Interface;
//using Compare.Services.LMS.Modules.LeadEntity.Interface;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;

//namespace Compare.Services.LMS.Campaign.UnitTests
//{
//    [TestClass]
//    public class RuleTests
//    {
//        private static System.IServiceProvider _ruleServiceProvider;
//        private Mock<IRule> _rule;
//        private Mock<IValidator> _validator;
//        private Mock<ILeadEntityImmutable> _leadEntity;
//        private Mock<List<IResult>> _resultList;
//        /// <summary>
//        /// Initializes this instance.
//        /// </summary>
//        [TestInitialize]
//        public void Initialize()
//        {
//            // Mock the Rule and Validator
//            _rule = new Mock<IRule>();
//            _leadEntity = new Mock<ILeadEntityImmutable>();
//            _resultList = new Mock<List<IResult>>();
//            _validator = new Mock<IValidator>();

//            // Create Service Providers for Rule and Validator
//            _ruleServiceProvider = new ServiceCollection()
//                .AddSingleton(typeof(IRule), _rule.Object)
//                .AddSingleton(typeof(IValidator), _validator.Object)
//                .BuildServiceProvider();
//        }
//        /// <summary>
//        /// Cleanups this instance.
//        /// </summary>
//        [TestCleanup]
//        public void Cleanup()
//        {
//            _rule.VerifyAll();
//            _rule = null;
//            _validator.VerifyAll();
//            _validator = null;
//            _ruleServiceProvider = null;
//        }

//        /// <summary>
//        /// Tests the ProcessLead in Rule Interface.
//        /// </summary>
//        [TestMethod]
//        public void TestIfLeadProcessedInRuleCall()
//        {
//            var Rule = _ruleServiceProvider.GetService<IRule>();
//            const string expectedMessage = "Lead was processed";
//            string actualMessage = "";

//            // Mock the ProcessLead function to update the message
//            _rule.Setup(c => c.ConstraintMet(It.IsAny<ILeadEntityImmutable>())).Callback(() => {
//                actualMessage = expectedMessage;
//            });

//            Rule.ConstraintMet(_leadEntity.Object);
//            Assert.AreEqual(expectedMessage, actualMessage);
//        }
//        /// <summary>
//        /// Tests the Rule lead validation with leads.
//        /// </summary>
//        [TestMethod]
//        public void TestLeadValidationForARule()
//        {
//            // A Rule
//            var Rule = _ruleServiceProvider.GetService<IRule>();

//            // the validator
//            var validator = _ruleServiceProvider.GetService<ICampaignValidator>();

//            // Set up the messages for Rule to return
//            const string expectedValidLeadMessage = "Valid Lead";
//            const string expectedInvalidLeadMessage = "Invalid Lead";
//            string actualMessage = string.Empty;

//            // Set up return values when the validator is invoked
//            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns<ILeadEntityImmutable>(s => {
//                if (s == null)
//                    return false;
//                else
//                    return true;
//            });

//            // Tie the Rule to call out to the validator
//            _rule.Setup(c => c.ConstraintMet(It.IsAny<ILeadEntityImmutable>())).Callback<ILeadEntityImmutable>(s => {
//                if (validator.ValidLead(s))
//                    actualMessage = expectedValidLeadMessage;
//                else
//                    actualMessage = expectedInvalidLeadMessage;
//            });

//            // Send a valid stream parameter
//            Rule.ConstraintMet(_leadEntity.Object);
//            Assert.AreEqual(expectedValidLeadMessage, actualMessage);

//            // Send a null value parameter
//            Rule.ConstraintMet(null);
//            Assert.AreEqual(expectedInvalidLeadMessage, actualMessage);
//        }

 
 
//    }
//}
