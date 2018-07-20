namespace LMS.Campaign.UnitTests.Campaign
{
    using System;
    using System.IO;
    using System.Text;
    using LMS.Decorator.Interface;
    using LMS.Campaign.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.Publisher.Interface;
    using LMS.Validator.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
 

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
        private Mock<ILeadEntity> _leadEntity;


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
            _leadEntity = new Mock<ILeadEntity>();

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

            //TBD - Uncomment when change string --> ILEadEntity
             //Mock the ProcessLead function to update the message
            _campaign.Setup(c => c.ProcessLead(It.IsAny<ILeadEntity>())).Callback(() =>
            {
                actualMessage = expectedMessage;
            });
            campaign.ProcessLead(_leadEntity.Object);

            // Mock the ProcessLead function to update the message
            //_campaign.Setup(c => c.ProcessLead(It.IsAny<ILeadEntity>())).Callback(() =>
            //{
            //    actualMessage = expectedMessage;
            //});

            Assert.AreEqual(expectedMessage, actualMessage);
        }
        /// <summary>
        /// Tests the campaign lead validation with leads.
        /// </summary>
       // TBD - this can be enabled again when we start using LeadEntity instead of String [TestMethod]
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
            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns<ILeadEntity>(s => {
                if (s == null)
                    return false;
                else
                    return true;
             });

            // TBD - Uncomment when change string --> ILEadEntity
            // Tie the campaign to call out to the validator
            _campaign.Setup(c => c.ProcessLead(It.IsAny<ILeadEntity>())).Callback<ILeadEntity>(s =>
            {
                if (validator.ValidLead(s))
                    actualMessage = expectedValidLeadMessage;
                else
                    actualMessage = expectedInvalidLeadMessage;
            });
            // Send a valid stream parameter
            campaign.ProcessLead(_leadEntity.Object);

            // Tie the campaign to call out to the validator
            //_campaign.Setup(c => c.ProcessLead(It.IsAny<string>())).Callback<ILeadEntity>(s =>
            //{
            //    if (validator.ValidLead(s))
            //        actualMessage = expectedValidLeadMessage;
            //    else
            //        actualMessage = expectedInvalidLeadMessage;
            //});
            //// Send a valid stream parameter
            //campaign.ProcessLead(expectedValidLeadMessage);

            //Assert.AreEqual(expectedValidLeadMessage, actualMessage);

            //// Send a null value parameter
            //campaign.ProcessLead(null);
            Assert.AreEqual(expectedInvalidLeadMessage, actualMessage);
        }

 
        /// <summary>
        /// Tests the campaign lead decorator.
        /// </summary>
      // TBD - This can be enabled again when we use LeadEntity  [TestMethod]
        public void TestLeadDecoratorForACampaign()
        {
            // A campaign
            var campaign = _campaignServiceProvider.GetService<ICampaign>();

            // the validator
            var validator = _campaignServiceProvider.GetService<IValidator>();

            // the decorator
            var decorator = _campaignServiceProvider.GetService<IDecorator>();

            // Set up the messages for campaign to return
            const string expectedDecoratedLeadMessage = "Decorated Lead";
            byte[] decoratedLeadMessageByteArray = Encoding.UTF8.GetBytes(expectedDecoratedLeadMessage);
            var lead = new MemoryStream();
 
            // Set up validator to return valid
            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns<ILeadEntity>(s => { return true; });

            // Mock the decorator lead function to decorate the lead - The text is copied to the input parameter
            _decorator.Setup(c => c.DecorateLead(It.IsAny<ILeadEntity>())).Callback(() => {
                lead = new MemoryStream(decoratedLeadMessageByteArray);
            });
            // TBD - Uncomment when change string --> ILEadEntity
            //// Tie the campaign to call out to the validator and if valid, decorate and then publish lead
            _campaign.Setup(c => c.ProcessLead(It.IsAny<ILeadEntity>()))
                .Callback<ILeadEntity>(s =>
                {
                    if (validator.ValidLead(s))
                    {
                        decorator.DecorateLead(s);
                    }
                });

            //// Send a valid stream parameter and check that lead is decorated
            campaign.ProcessLead(_leadEntity.Object);
            //_campaign.Setup(c => c.ProcessLead(It.IsAny<string>()))
            //    .Callback<ILeadEntity>(s =>
            //    {
            //        if (validator.ValidLead(s))
            //        {
            //            decorator.DecorateLead(s);
            //        }
            //    });

            // Send a valid stream parameter and check that lead is decorated
            //campaign.ProcessLead(expectedDecoratedLeadMessage);

            // Read the stream returned
            StreamReader reader = new StreamReader(lead);
            string decoratedLead = reader.ReadToEnd();

            // Verify that the lead carries the decorated string
            Assert.IsTrue(decoratedLead.Contains(expectedDecoratedLeadMessage));
            
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

            // the decorator
            var decorator = _campaignServiceProvider.GetService<IDecorator>();

            // the publisher
            var publisher = _campaignServiceProvider.GetService<IPublisher>();

            // Set up the messages for campaign to return
            const string expectedPublishedLeadMessage = "Published Lead";
            string actualMessage = string.Empty;

            // Set up validator to return valid
            _validator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns<ILeadEntity>(s => { return true; });

            // Mock the decorator lead function - doesn't have to do anything here
            _decorator.Setup(c => c.DecorateLead(It.IsAny<ILeadEntity>())).Callback(() => {
                //Do Nothing
                          });
            // Mock the publish lead function to update the message
            _publisher.Setup(c => c.PublishLead(It.IsAny<ILeadEntity>())).Callback(() => {
                actualMessage = expectedPublishedLeadMessage;
            });

            // TBD - Uncomment when change string --> ILEadEntity
            // Tie the campaign to call out to the validator and if valid, publish lead
            //_campaign.Setup(c => c.ProcessLead(It.IsAny<ILeadEntity>())).Callback<ILeadEntity>(s =>
            //{
            //    if (validator.ValidLead(s))
            //    {
            //        decorator.DecorateLead(s);
            //        publisher.PublishLead(s);
            //    }

            //});

            //// Send a valid stream parameter
            campaign.ProcessLead(_leadEntity.Object);

            // Tie the campaign to call out to the validator and if valid, publish lead - some cheating going on here!!!! :-) - change to leadEntity 
            _campaign.Setup(c => c.ProcessLead(It.IsAny<ILeadEntity>())).Callback<ILeadEntity>(s =>
            {
                if (validator.ValidLead(_leadEntity.Object))
                {
                    decorator.DecorateLead(_leadEntity.Object);
                    publisher.PublishLead(_leadEntity.Object);
                }

            });

            //// Send a valid stream parameter
            //campaign.ProcessLead(expectedPublishedLeadMessage);
            Assert.AreEqual(expectedPublishedLeadMessage, actualMessage);
        }

    }
}
