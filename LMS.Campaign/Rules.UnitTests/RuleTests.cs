namespace Rules.UnitTests
{
    using System;
    using System.IO;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Decorator.Interface;
    using Validator.Interface;
    using Rule.Interface;
    using System.Text;

    [TestClass]
    public class RuleTests
    {
        private static System.IServiceProvider _ruleServiceProvider;
        private Mock<IRule> _rule;
        private Mock<IValidator> _validator;
        private Mock<IDecorator> _decorator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Rule and Validator
            _rule = new Mock<IRule>();
            _validator = new Mock<IValidator>();
             _decorator = new Mock<IDecorator>();

            // Create Service Providers for Rule and Validator
            _ruleServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(IRule), _rule.Object)
                .AddSingleton(typeof(IValidator), _validator.Object)
                .AddSingleton(typeof(IDecorator), _decorator.Object)
                .BuildServiceProvider();
        }
        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _rule.VerifyAll();
            _rule = null;
            _validator.VerifyAll();
            _validator = null;
            _decorator.VerifyAll();
            _decorator = null;
            _ruleServiceProvider = null;
        }

        /// <summary>
        /// Tests the ProcessLead in Rule Interface.
        /// </summary>
        [TestMethod]
        public void TestIfLeadProcessedInRuleCall()
        {
            var Rule = _ruleServiceProvider.GetService<IRule>();
            const string expectedMessage = "Lead was processed";
            string actualMessage = "";

            // Mock the ProcessLead function to update the message
            _rule.Setup(c => c.ProcessLead(It.IsAny<Stream>())).Callback(() => {
                actualMessage = expectedMessage;
            });

            Rule.ProcessLead(new MemoryStream());
            Assert.AreEqual(expectedMessage, actualMessage);
        }
        /// <summary>
        /// Tests the Rule lead validation with leads.
        /// </summary>
        [TestMethod]
        public void TestLeadValidationForARule()
        {
            // A Rule
            var Rule = _ruleServiceProvider.GetService<IRule>();

            // the validator
            var validator = _ruleServiceProvider.GetService<IValidator>();

            // Set up the messages for Rule to return
            const string expectedValidLeadMessage = "Valid Lead";
            const string expectedInvalidLeadMessage = "Invalid Lead";
            string actualMessage = string.Empty;

            // Set up return values when the validator is invoked
            _validator.Setup(v => v.ValidLead(It.IsAny<Stream>())).Returns<Stream>(s => {
                if (s == null)
                    return false;
                else
                    return true;
            });

            // Tie the Rule to call out to the validator
            _rule.Setup(c => c.ProcessLead(It.IsAny<Stream>())).Callback<Stream>(s => {
                if (validator.ValidLead(s))
                    actualMessage = expectedValidLeadMessage;
                else
                    actualMessage = expectedInvalidLeadMessage;
            });

            // Send a valid stream parameter
            Rule.ProcessLead(new MemoryStream());
            Assert.AreEqual(expectedValidLeadMessage, actualMessage);

            // Send a null value parameter
            Rule.ProcessLead(null);
            Assert.AreEqual(expectedInvalidLeadMessage, actualMessage);
        }

 
        /// <summary>
        /// Tests the Rule lead decorator.
        /// </summary>
        [TestMethod]
        public void TestLeadDecoratorForARule()
        {
            // A Rule
            var Rule = _ruleServiceProvider.GetService<IRule>();

            // the validator
            var validator = _ruleServiceProvider.GetService<IValidator>();

            // the decorator
            var decorator = _ruleServiceProvider.GetService<IDecorator>();

            // Set up the messages for Rule to return
            const string expectedDecoratedLeadMessage = "Decorated Lead";
            byte[] decoratedLeadMessageByteArray = Encoding.UTF8.GetBytes(expectedDecoratedLeadMessage);
            var lead = new MemoryStream();

            // Set up validator to return valid
            _validator.Setup(v => v.ValidLead(It.IsAny<Stream>())).Returns<Stream>(s => { return true; });


            // Mock the decorator lead function to decorate the lead - The text is copied to the input parameter
            _decorator.Setup(c => c.DecorateLead(It.IsAny<Stream>())).Callback(() => {
                lead = new MemoryStream(decoratedLeadMessageByteArray);
            });

            // Tie the Rule to call out to the validator and if valid, publish and then decorate lead
            _rule.Setup(c => c.ProcessLead(It.IsAny<Stream>()))
                .Callback<Stream>(s =>
                {
                    if (validator.ValidLead(s))
                    {
                        decorator.DecorateLead(s);
                    }
                });

            // Send a valid stream parameter and check that lead is decorated
            Rule.ProcessLead(lead);

            // Read the stream returned
            StreamReader reader = new StreamReader(lead);
            string decoratedLead = reader.ReadToEnd();

            // Verify that the lead carries the decorated string
            Assert.IsTrue(decoratedLead.Contains(expectedDecoratedLeadMessage));

        }
    }
}