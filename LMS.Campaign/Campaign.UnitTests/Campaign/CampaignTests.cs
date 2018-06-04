namespace Campaign.UnitTests.Campaign
{
    using System;
    using System.IO;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    using Publisher.Interface;
    using Validator.Interface;
    using Decorator.Interface;
    using System.Text;
    using global::Campaign.Interface;

    /// <summary>
    /// Campaign Unit Tests
    /// </summary>
    [TestClass]
    public class CampaignTests
    {
        private static IServiceProvider _campaignServiceProvider;
        private Mock<ICampaign> _campaign;
        private Mock<IValidator> _validator;
        private Mock<IPublisher> _publisher;
        private Mock<IDecorator> _decorator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Campaign, Validator, Publisher and Decorator
            _campaign = new Mock<ICampaign>();
            _validator = new Mock<IValidator>();
            _publisher = new Mock<IPublisher>();
            _decorator = new Mock<IDecorator>();

            // Create Service Providers 
            _campaignServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ICampaign), _campaign.Object)
                .AddSingleton(typeof(IValidator), _validator.Object)
                .AddSingleton(typeof(IPublisher), _publisher.Object)
                .AddSingleton(typeof(IDecorator), _decorator.Object)
                .BuildServiceProvider(); 
        }
        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _campaign.VerifyAll();
            _campaign = null;
            _validator.VerifyAll();
            _validator = null;
            _publisher.VerifyAll();
            _publisher = null;
            _decorator.VerifyAll();
            _decorator = null;
            _campaignServiceProvider = null;
        }

        /// <summary>
        /// Tests the ProcessLead in Campaign Interface.
        /// </summary>
        [TestMethod]
        public void TestIfLeadProcessedInCampaignCall()
        {
            var campaign = _campaignServiceProvider.GetService<ICampaign>();
            const string expectedMessage = "Lead was processed";
            string actualMessage = "";

            // Mock the ProcessLead function to update the message
            _campaign.Setup(c => c.ProcessLead(It.IsAny<Stream>())).Callback(() => {
                actualMessage = expectedMessage;
            });

            campaign.ProcessLead(new MemoryStream());
            Assert.AreEqual(expectedMessage, actualMessage);
        }
        /// <summary>
        /// Tests the campaign lead validation with leads.
        /// </summary>
        [TestMethod]
        public void TestLeadValidationForACampaign()
        {
            // A campaign
            var campaign = _campaignServiceProvider.GetService<ICampaign>();

            // the validator
            var validator = _campaignServiceProvider.GetService<IValidator>();

            // Set up the messages for campaign to return
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

            // Tie the campaign to call out to the validator
            _campaign.Setup(c => c.ProcessLead(It.IsAny<Stream>())).Callback<Stream>(s => {
                if(validator.ValidLead(s))
                    actualMessage = expectedValidLeadMessage;
                else
                    actualMessage = expectedInvalidLeadMessage;
            });

            // Send a valid stream parameter
            campaign.ProcessLead(new MemoryStream());
            Assert.AreEqual(expectedValidLeadMessage, actualMessage);

            // Send a null value parameter
            campaign.ProcessLead(null);
            Assert.AreEqual(expectedInvalidLeadMessage, actualMessage);
        }

        /// <summary>
        /// Tests the campaign lead publisher.
        /// </summary>
        [TestMethod]
        public void TestLeadPublisherForACampaign()
        {
            // A campaign
            var campaign = _campaignServiceProvider.GetService<ICampaign>();

            // the validator
            var validator = _campaignServiceProvider.GetService<IValidator>();

            // the publisher
            var publisher = _campaignServiceProvider.GetService<IPublisher>();

            // Set up the messages for campaign to return
            const string expectedPublishedLeadMessage = "Published Lead";
            string actualMessage = string.Empty;

            // Set up validator to return valid
            _validator.Setup(v => v.ValidLead(It.IsAny<Stream>())).Returns<Stream>(s => { return true; });

            // Mock the publish lead function to update the message
            _publisher.Setup(c => c.PublishLead(It.IsAny<Stream>())).Callback(() => {
                actualMessage = expectedPublishedLeadMessage;
            });

            // Tie the campaign to call out to the validator and if valid, publish lead
            _campaign.Setup(c => c.ProcessLead(It.IsAny<Stream>())).Callback<Stream>(s => {
                if (validator.ValidLead(s))
                    publisher.PublishLead(s);
                
            });

            // Send a valid stream parameter
            campaign.ProcessLead(new MemoryStream());
            Assert.AreEqual(expectedPublishedLeadMessage, actualMessage);
        }

        /// <summary>
        /// Tests the campaign lead decorator.
        /// </summary>
        [TestMethod]
        public void TestLeadDecoratorForACampaign()
        {
            // A campaign
            var campaign = _campaignServiceProvider.GetService<ICampaign>();

            // the validator
            var validator = _campaignServiceProvider.GetService<IValidator>();

            // the publisher
            var publisher = _campaignServiceProvider.GetService<IPublisher>();

            // the decorator
            var decorator = _campaignServiceProvider.GetService<IDecorator>();

            // Set up the messages for campaign to return
            const string expectedDecoratedLeadMessage = "Decorated Lead";
            byte[] decoratedLeadMessageByteArray = Encoding.UTF8.GetBytes(expectedDecoratedLeadMessage);
            var lead = new MemoryStream();
 
            // Set up validator to return valid
            _validator.Setup(v => v.ValidLead(It.IsAny<Stream>())).Returns<Stream>(s => { return true; });

            // Mock the publish lead function to publish - do nothing really
            _publisher.Setup(c => c.PublishLead(It.IsAny<Stream>())).Callback(() => { // publish 
            });

            // Mock the decorator lead function to decorate the lead - The text is copied to the input parameter
            _decorator.Setup(c => c.DecorateLead(It.IsAny<Stream>())).Callback(() => {
                lead = new MemoryStream(decoratedLeadMessageByteArray);
            });

            // Tie the campaign to call out to the validator and if valid, publish and then decorate lead
            _campaign.Setup(c => c.ProcessLead(It.IsAny<Stream>()))
                .Callback<Stream>(s => 
                {
                    if (validator.ValidLead(s))
                    {
                        publisher.PublishLead(s);
                        decorator.DecorateLead(s);
                    }
                });

            // Send a valid stream parameter and check that lead is decorated
            campaign.ProcessLead(lead);

            // Read the stream returned
            StreamReader reader = new StreamReader(lead);
            string decoratedLead = reader.ReadToEnd();

            // Verify that the lead carries the decorated string
            Assert.IsTrue(decoratedLead.Contains(expectedDecoratedLeadMessage));
            
        }
    }
}
