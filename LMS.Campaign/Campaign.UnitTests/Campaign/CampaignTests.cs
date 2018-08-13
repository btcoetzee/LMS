using System.Collections.Generic;

namespace LMS.Campaign.UnitTests.Campaign
{
    using System;
    using System.IO;
    using System.Text;
    using LMS.Campaign.Interface;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.Publisher.Interface;
    using LMS.Validator.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using LMS.Campaign.BuyClick;
    using LMS.CampaignValidator.Interface;
    using LMS.LoggerClient.Interface;
    using LMS.Filter.Interface;
    using LMS.Rule.Interface;
    using LMS.LeadEntity.Components;
    using LMS.Campaign.BuyClick.Filter;
    using LMS.Campaign.BuyClick.Validator;
    using LMS.Campaign.BuyClick.Rule;
    using LMS.LeadEntity.Interface.Constants;
    using System.Linq;



    /// <summary>
    /// Campaign Unit Tests
    /// </summary>
    [TestClass]
    public class CampaignTests
    {
        private static IServiceProvider _campaignServiceProvider;
        private Mock<ICampaign> _campaign;
        private Mock<ILeadEntity> _leadEntity;
        private Mock<ICampaignValidator> _campaignValidator;
        private Mock<IFilter> _filter;
        private Mock<IRule> _rule;
        private Mock<ILoggerClient> _loggingClient;
        private ILeadEntity _testLeadEntity;
        private List<IResult> _campaignResultList;

        #region Initializer
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Campaign, Validator, Publisher and Decorator
            _campaign = new Mock<ICampaign>();
            _leadEntity = new Mock<ILeadEntity>();
            _campaignValidator = new Mock<ICampaignValidator>();
            _filter = new Mock<IFilter>();
            _rule = new Mock<IRule>();
            _loggingClient = new Mock<ILoggerClient>();
            _campaignResultList = new List<IResult>();

            // Create Service Providers 
            _campaignServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ICampaign), _campaign.Object)
                 .AddSingleton(typeof(ICampaignValidator), _campaignValidator.Object)
                .AddSingleton(typeof(IFilter), _filter.Object)
                .AddSingleton(typeof(IRule), _rule.Object)
                .AddSingleton(typeof(ILoggerClient), _loggingClient.Object)
                .BuildServiceProvider();
            CreateTestLeadEntity();
        }

        void CreateTestLeadEntity()
        {
            _testLeadEntity = new DefaultLeadEntity()
            {
                Context = new IContext[] { },
                Properties = new IProperty[] { },
                Segments = new ISegment[] { }
            };

        }
        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _campaign.VerifyAll();
            _campaign = null;
            _campaignServiceProvider = null;
            _campaignValidator.VerifyAll();
            _campaignValidator = null;
            _filter.VerifyAll();
            _filter = null;
            _rule.VerifyAll();
            _rule = null;
            _loggingClient.VerifyAll();
            _loggingClient = null;
        }
        #endregion

        #region BuyClickCampaignTest

        /// <summary>
        /// Tests the campaign lead validation with leads.
        /// </summary>
        [TestMethod]
        public void TestLeadValidationForACampaignReturnFalse()
        {
            // The process lead function adds results to the Campaign Results list
            var expecteResultsIfValidatorReturnsFalse = 5;
          
            // A campaign
            var campaign = new BuyClickCampaign(_campaignServiceProvider.GetService<ICampaignValidator>(), _campaignServiceProvider.GetService<IFilter>(),
         _campaignServiceProvider.GetService<IRule>(), _campaignServiceProvider.GetService<ILoggerClient>());

            // Setting up the validator to return false
            _campaignValidator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(false);
   
            // Send a valid stream parameter
            _campaignResultList = campaign.ProcessLead(_testLeadEntity);

            //// Evaluate if the number of results in results list is as expected
            Assert.AreEqual(expecteResultsIfValidatorReturnsFalse, _campaignResultList.Count);

            // looks in the results collection to see if the validator said the lead failed
            var expectedValue = _campaignResultList.SingleOrDefault(item =>
               item.Id == ResultKeys.ValidatorStatusKey)?.Value;
            Assert.AreEqual(expectedValue.ToString(), ResultKeys.ResultKeysStatusEnum.Failed.ToString());
        }

        /// <summary>
        /// Tests the campaign lead validation with leads.
        /// </summary>
        [TestMethod]
        public void TestLeadValidationForACampaignReturnTrue()
        {
            // The process lead function adds results to the Campaign Results list
            var expecteResultsIfValidatorReturnsTrue = 6;
            // A campaign
            var campaign = new BuyClickCampaign(_campaignServiceProvider.GetService<ICampaignValidator>(), _campaignServiceProvider.GetService<IFilter>(),
         _campaignServiceProvider.GetService<IRule>(), _campaignServiceProvider.GetService<ILoggerClient>());



            // Setting up the validator to return true
            _campaignValidator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);

            // Send a valid stream parameter
            _campaignResultList = campaign.ProcessLead(_testLeadEntity);

            // Evaluate if the number of results in results list is as expected
            Assert.AreEqual(expecteResultsIfValidatorReturnsTrue, _campaignResultList.Count);

            // looks in the results collection to see if the validator said the lead processed
            var expectedValue = _campaignResultList.SingleOrDefault(item =>
               item.Id == ResultKeys.ValidatorStatusKey)?.Value;
            Assert.AreEqual(expectedValue.ToString(), ResultKeys.ResultKeysStatusEnum.Processed.ToString());
        }

        /// <summary>
        /// Tests the campaign lead filter with leads.
        /// </summary>
        [TestMethod]
        public void TestLeadFilterForACampaignReturnTrue()
        {
            // The process lead function adds results to the Campaign Results list
            var expecteResultsIfFilterReturnsTrue = 7;
            // A campaign
            var campaign = new BuyClickCampaign(_campaignServiceProvider.GetService<ICampaignValidator>(), _campaignServiceProvider.GetService<IFilter>(),
         _campaignServiceProvider.GetService<IRule>(), _campaignServiceProvider.GetService<ILoggerClient>());



            // Setting up the validator to return true
            _campaignValidator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);

            // Setting up the filter to return true
            _filter.Setup(v => v.ClearedFilter(It.IsAny<ILeadEntity>())).Returns(true);
            

            // Send a valid stream parameter
            _campaignResultList = campaign.ProcessLead(_testLeadEntity);

            // Evaluate if the number of results in results list is as expected
            Assert.AreEqual(expecteResultsIfFilterReturnsTrue, _campaignResultList.Count);

            // looks in the results collection to see if the validator said the lead processed
            var expectedValue = _campaignResultList.SingleOrDefault(item =>
               item.Id == ResultKeys.ValidatorStatusKey)?.Value;
            Assert.AreEqual(expectedValue.ToString(), ResultKeys.ResultKeysStatusEnum.Processed.ToString());
        }

        /// <summary>
        /// Tests the campaign lead rule with leads.
        /// </summary>
        [TestMethod]
        public void TestLeadRuleForACampaignReturnTrue()
        {
            // The process lead function adds results to the Campaign Results list
            var expecteResultsIfRuleReturnsTrue = 9;
            // A campaign
            var campaign = new BuyClickCampaign(_campaignServiceProvider.GetService<ICampaignValidator>(), _campaignServiceProvider.GetService<IFilter>(),
         _campaignServiceProvider.GetService<IRule>(), _campaignServiceProvider.GetService<ILoggerClient>());



            // Setting up the validator to return true
            _campaignValidator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);

            // Setting up the filter to return true
            _filter.Setup(v => v.ClearedFilter(It.IsAny<ILeadEntity>())).Returns(true);

            // Setting up the rule to return true
            _rule.Setup(v => v.ValidateForRule(It.IsAny<ILeadEntity>())).Returns(true);


            // Send a valid stream parameter
            _campaignResultList = campaign.ProcessLead(_testLeadEntity);

            // Evaluate if the number of results in results list is as expected
            Assert.AreEqual(expecteResultsIfRuleReturnsTrue, _campaignResultList.Count);

            // looks in the results collection to see if the validator said the lead processed
            var expectedValue = _campaignResultList.SingleOrDefault(item =>
               item.Id == ResultKeys.ValidatorStatusKey)?.Value;
            Assert.AreEqual(expectedValue.ToString(), ResultKeys.ResultKeysStatusEnum.Processed.ToString());
        }

        // <summary>
        // Tests a null campaign validator.
        // </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void TestNullBuyClickValidatorCampaign()
        {
            new BuyClickCampaign(null, _campaignServiceProvider.GetService<IFilter>(), _campaignServiceProvider.GetService<IRule>(), _campaignServiceProvider.GetService<ILoggerClient>());
        }

        // <summary>
        // Tests a null campaign filter.
        // </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void TestNullFilterCampaign()
        {
            new BuyClickCampaign(_campaignServiceProvider.GetService<ICampaignValidator>(), null, _campaignServiceProvider.GetService<IRule>(), _campaignServiceProvider.GetService<ILoggerClient>());
        }

        // <summary>
        // Tests a null campaign rule.
        // </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void TestNullRuleCampaign()
        {
            new BuyClickCampaign(_campaignServiceProvider.GetService<ICampaignValidator>(), _campaignServiceProvider.GetService<IFilter>(), null, _campaignServiceProvider.GetService<ILoggerClient>());
        }

        // <summary>
        // Tests a null campaign logger client.
        // </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void TestNullLoggerClientCampaign()
        {
            new BuyClickCampaign(_campaignServiceProvider.GetService<ICampaignValidator>(), _campaignServiceProvider.GetService<IFilter>(), _campaignServiceProvider.GetService<IRule>(), null);
        }

        /// <summary>
        /// Mocked the parameter process lead constructor test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ProcessLeadNullLeadEntityTest()
        {
                var campaign = new BuyClickCampaign(_campaignServiceProvider.GetService<ICampaignValidator>(), _campaignServiceProvider.GetService<IFilter>(),
                    _campaignServiceProvider.GetService<IRule>(), _campaignServiceProvider.GetService<ILoggerClient>());

            campaign.ProcessLead(null);            
        }

        #endregion

        #region BuyclickFilterTest

        /// <summary>
        /// Mocked the buy click filter test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void BuyClickFilterNullTest()
        {
            new BuyClickFilter(null);
        }

        /// <summary>
        /// Mocked the buy click filter test.
        /// </summary>
        [TestMethod]
        public void BuyClickClearedFilterTest()
        {
            var filter = new BuyClickFilter(_campaignServiceProvider.GetService<ILoggerClient>());

            filter.ClearedFilter(_testLeadEntity);                
        }

        /// <summary>
        /// Mocked the buy click filter test.
        /// </summary>
        [TestMethod]
        public void BuyClickClearedFilterNullTest()
        {
            var filter = new BuyClickFilter(_campaignServiceProvider.GetService<ILoggerClient>());

            filter.ClearedFilter(null);
        }

        #endregion

        #region BuyClickValidatorTest

        /// <summary>
        /// Mocked the buy click validator test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void BuyClickValidatorNullTest()
        {
            new BuyClickValidator(null);
        }

        /// <summary>
        /// Mocked the buy click validator test.
        /// </summary>
        [TestMethod]
        public void BuyClickClearedValidatorNullTest()
        {
            var validator = new BuyClickValidator(_campaignServiceProvider.GetService<ILoggerClient>());

            validator.ValidLead(null);
        }

        /// <summary>
        /// Mocked the buy click validator test.
        /// </summary>
        [TestMethod]
        public void BuyClickClearedValidatorTest()
        {
            var validator = new BuyClickValidator(_campaignServiceProvider.GetService<ILoggerClient>());

            validator.ValidLead(_testLeadEntity);
        }

        #endregion

        #region BuyClickRuleTest

        /// <summary>
        /// Mocked the buy click rule test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void BuyClickRuleNullTest()
        {
            new BuyClickRule(null);
        }

        /// <summary>
        /// Mocked the buy click rule test.
        /// </summary>
        [TestMethod]
        public void BuyClickRuleClearedRuleNullTest()
        {
            var rule = new BuyClickRule(_campaignServiceProvider.GetService<ILoggerClient>());

            rule.ValidateForRule(null);
        }

        /// <summary>
        /// Mocked the buy click rule test.
        /// </summary>
        [TestMethod]
        public void BuyClickRuleClearedRuleTest()
        {
            var rule = new BuyClickRule(_campaignServiceProvider.GetService<ILoggerClient>());

            rule.ValidateForRule(_testLeadEntity).Equals(true);
        }

        #endregion
    }
}
