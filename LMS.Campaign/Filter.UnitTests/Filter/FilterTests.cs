namespace Filter.UnitTests.Filter
{
    using System;
    using System.IO;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using global::Filter.Interface;
    using Validator.Interface;
    using Decorator.Interface;
    using System.Text;

    /// <summary>
    /// Filter Unit Tests
    /// </summary>
    [TestClass]
    public class FilterTests
    {
        private static IServiceProvider _FilterServiceProvider;
        private Mock<IFilter> _Filter;
        private Mock<IValidator> _validator;
        private Mock<IDecorator> _decorator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Filter and Validator
            _Filter = new Mock<IFilter>();
            _validator = new Mock<IValidator>();
            _decorator = new Mock<IDecorator>();

            // Create Service Providers for Filter and Validator
            _FilterServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(IFilter), _Filter.Object)
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
            _Filter.VerifyAll();
            _Filter = null;
            _validator.VerifyAll();
            _validator = null;
            _decorator.VerifyAll();
            _decorator = null;
            _FilterServiceProvider = null;
        }

        /// <summary>
        /// Tests the ProcessLead in Filter Interface.
        /// </summary>
        [TestMethod]
        public void TestIfLeadProcessedInFilterCall()
        {
            var Filter = _FilterServiceProvider.GetService<IFilter>();
            const string expectedMessage = "Lead was processed";
            string actualMessage = "";

            // Mock the ProcessLead function to update the message
            _Filter.Setup(c => c.ProcessLead(It.IsAny<Stream>())).Callback(() => {
                actualMessage = expectedMessage;
            });

            Filter.ProcessLead(new MemoryStream());
            Assert.AreEqual(expectedMessage, actualMessage);
        }
        /// <summary>
        /// Tests the Filter lead Validation with leads.
        /// </summary>
        [TestMethod]
        public void TestLeadValidationForAFilter()
        {
            // A Filter
            var Filter = _FilterServiceProvider.GetService<IFilter>();

            // the validator
            var validator = _FilterServiceProvider.GetService<IValidator>();

            // Set up the messages for Filter to return
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

            // Tie the Filter to call out to the validator
            _Filter.Setup(c => c.ProcessLead(It.IsAny<Stream>())).Callback<Stream>(s => {
                if(validator.ValidLead(s))
                    actualMessage = expectedValidLeadMessage;
                else
                    actualMessage = expectedInvalidLeadMessage;
            });

            // Send a valid stream parameter
            Filter.ProcessLead(new MemoryStream());
            Assert.AreEqual(expectedValidLeadMessage, actualMessage);

            // Send a null value parameter
            Filter.ProcessLead(null);
            Assert.AreEqual(expectedInvalidLeadMessage, actualMessage);
        }

        /// <summary>
        /// Tests the Filter lead decorator.
        /// </summary>
        [TestMethod]
        public void TestLeadDecoratorForAFilter()
        {
            // A Filter
            var Filter = _FilterServiceProvider.GetService<IFilter>();

            // the validator
            var validator = _FilterServiceProvider.GetService<IValidator>();

            // the decorator
            var decorator = _FilterServiceProvider.GetService<IDecorator>();

            // Set up the messages for Filter to return
            const string expectedDecoratedLeadMessage = "Decorated Lead";
            byte[] decoratedLeadMessageByteArray = Encoding.UTF8.GetBytes(expectedDecoratedLeadMessage);
            var lead = new MemoryStream();
 
            // Set up validator to return valid
            _validator.Setup(v => v.ValidLead(It.IsAny<Stream>())).Returns<Stream>(s => { return true; });

            // Mock the decorator lead function to decorate the lead - The text is copied to the input parameter
            _decorator.Setup(c => c.DecorateLead(It.IsAny<Stream>())).Callback(() => {
                lead = new MemoryStream(decoratedLeadMessageByteArray);
            });

            // Tie the Filter to call out to the validator and if valid, publish and then decorate lead
            _Filter.Setup(c => c.ProcessLead(It.IsAny<Stream>()))
                .Callback<Stream>(s => 
                {
                    if (validator.ValidLead(s))
                    {
                        decorator.DecorateLead(s);
                    }
                });

            // Send a valid stream parameter and check that lead is decorated
            Filter.ProcessLead(lead);

            // Read the stream returned
            StreamReader reader = new StreamReader(lead);
            string decoratedLead = reader.ReadToEnd();

            // Verify that the lead carries the decorated string
            Assert.IsTrue(decoratedLead.Contains(expectedDecoratedLeadMessage));
            
        }
    }
}
