using System;
using System.Collections.Generic;
using System.Linq;
using Compare.Services.LMS.Campaign.Interface;
using Compare.Services.LMS.Campaign.Implementation;
using Compare.Services.LMS.Campaign.Implementation.Controller;
using Compare.Services.LMS.Campaign.Implementation.Validator;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Validator.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Components;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;
using Compare.Services.LMS.Modules.LoggerClient.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Compare.Services.LMS.Campaign.UnitTests
{
    /// <summary>
    /// Campaign Unit Tests
    /// </summary>
    [TestClass]
    public class CampaignTests
    {
        private static IServiceProvider _campaignServiceProvider;
        private Mock<ICampaign> _campaign;
        private Mock<ILeadEntity> _leadEntity;
        private Mock<ILoggerClient> _loggingClient;
        private Mock<ICampaignConfig> _campaignConfig;
        private Mock<List<IController>> _campaignControllerList;
        private Mock<List<IValidator>> _campaignValidatorList;
        private ILeadEntity _testLeadEntity;
        private List<IResult> _campaignResultList;
        private readonly IController _testConstraintMetController = new TestControllerConstraintMet();
        private readonly IController _testConstraintNotMetController = new TestControllerConstraintNotMet();
        private readonly IController _testControllerWithException = new TestControllerWithException();
        private readonly IValidator _testValidLeadValidator = new TestValidatorConstraintMet();
        private readonly IValidator _testInvalidLeadValidator = new TestValidatorConstraintNotMet();
        private readonly IValidator _testValidatorWithException = new TestValidatorWithException();
        private int _campaignId;
        private string _campaignDescription;
        private int _campaignPriority;

        #region TestClasses
        /// <summary>
        /// Define a Controller where the constraint is met
        /// </summary>
        class TestControllerConstraintMet : IController
        {
            public bool ConstraintMet(ILeadEntity leadEntity)
            {
                return true;
            }
        }

        /// <summary>
        ///  Define a Controller where the constraint is not met
        /// </summary>
        class TestControllerConstraintNotMet : IController
        {
            public bool ConstraintMet(ILeadEntity leadEntity)
            {
                return false;
            }
        }

        /// <summary>
        ///  Define a Controller where the controller throws an exception
        /// </summary>
        class TestControllerWithException : IController
        {
            public bool ConstraintMet(ILeadEntity leadEntity)
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Define a Validator where the constraint is met
        /// </summary>
        class TestValidatorConstraintMet : IValidator
        {
            public bool ValidLead(ILeadEntity leadEntity)
            {
                return true;
            }
        }

        /// <summary>
        ///  Define a Validator where the constraint is not met
        /// </summary>
        class TestValidatorConstraintNotMet : IValidator
        {
            public bool ValidLead(ILeadEntity leadEntity)
            {
                return false;
            }
        }

        /// <summary>
        ///  Define a Validator where the controller throws an exception
        /// </summary>
        class TestValidatorWithException : IValidator
        {
            public bool ValidLead(ILeadEntity leadEntity)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

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
            _loggingClient = new Mock<ILoggerClient>();
            _campaignConfig = new Mock<ICampaignConfig>();
            _campaignControllerList = new Mock<List<IController>> ();
            _campaignValidatorList = new Mock<List<IValidator>>();
            _campaignResultList = new List<IResult>();

            _campaignId = 1;
            _campaignDescription = "Some BuyClick Campaign";
            _campaignPriority = 1;

            // Create Service Providers 
            _campaignServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ICampaign), _campaign.Object)
                .AddSingleton(typeof(ILoggerClient), _loggingClient.Object)
                .AddSingleton(typeof(ICampaignConfig), _campaignConfig.Object)
                .AddSingleton(typeof(List<IController>), _campaignControllerList.Object)
                .AddSingleton(typeof(List<IValidator>), _campaignValidatorList.Object)
                .BuildServiceProvider();
            CreateTestLeadEntity();
        }

        void CreateTestLeadEntity()
        {
            //_testLeadEntity = new DefaultLeadEntity()
            //{
            //    Context = new IContext[] { new DefaultContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()) },
            //    Properties = new IProperty[] { new DefaultProperty(PropertyKeys.VehicleCountKey, "5") },
            //    Segments = new ISegment[] { new DefaultSegment(SegementKeys.HighPOPKey) }
            //};
            _testLeadEntity = new DefaultLeadEntity()
            {
                Context = new IContext[] {  },
                Properties = new IProperty[] { },
                Segments = new ISegment[] {  }
            };
        }
        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _campaignServiceProvider = null;
            _campaign.VerifyAll();
            _campaign = null;
            _campaignConfig.VerifyAll();
            _campaignConfig = null;
            _loggingClient.VerifyAll();
            _loggingClient = null;

        }
        #endregion

        #region ConstructorTests
        // <summary>
        // Tests a successful  Campaign.
        // </summary>
        [TestMethod]
        public void CampaignConstructorSuccess()
        {
            new Implementation.Campaign(_campaignId, _campaignDescription, _campaignPriority,
                _campaignServiceProvider.GetService<ICampaignConfig>(),
                _campaignServiceProvider.GetService<ILoggerClient>());

        }

        // <summary>
        // Constructor Test with Null CampaignConfig
        // </summary>
        [TestMethod]
        public void CampaignConstructorNullCampaignConfig()
        {
            try
            {
                new Implementation.Campaign(_campaignId, _campaignDescription, _campaignPriority,
                    null,
                    _campaignServiceProvider.GetService<ILoggerClient>());

            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentNullException), ex.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignConfig", ex.Message.Replace(Environment.NewLine, " "));
            }
         
        }

        // <summary>
        // Constructor Test with Null LoggerClient
        // </summary>
        [TestMethod]
        public void CampaignConstructorNullLoggerClient()
        {
            try
            {
                new Implementation.Campaign(_campaignId, _campaignDescription, _campaignPriority,
                    _campaignServiceProvider.GetService<ICampaignConfig>(),
                    null);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentNullException), ex.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: loggerClient", ex.Message.Replace(Environment.NewLine, " "));
            }
          
        }

        // <summary>
        // Constructor Test with invalid campaignId
        // </summary>
        [TestMethod]
        public void CampaignConstructorInvalidCampaignId()
        {
            var testCampaingId = 0;
            string testSolutionContext = "Campaign";
            try
            {
                new Implementation.Campaign(testCampaingId, _campaignDescription, _campaignPriority,
                    _campaignServiceProvider.GetService<ICampaignConfig>(),
                    _campaignServiceProvider.GetService<ILoggerClient>());
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentException), ex.GetType());
                Assert.AreEqual($"Error: { testSolutionContext}: campaignId = { testCampaingId}", ex.Message.Replace(Environment.NewLine, " "));
            }
     
        }

        // <summary>
        // Constructor Test with null Campaign Description
        // </summary>
        [TestMethod]
        public void CampaignConstructorNullCampaignDescription()
        {
 
            try
            {
                new Implementation.Campaign(_campaignId, null, _campaignPriority,
                    _campaignServiceProvider.GetService<ICampaignConfig>(),
                    _campaignServiceProvider.GetService<ILoggerClient>());
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentNullException), ex.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignDescription", ex.Message.Replace(Environment.NewLine, " "));
            }
        }

        // <summary>
        // Constructor Test with null Campaign Priority
        // </summary>
        [TestMethod]

        public void CampaignConstructorInvalidCampaignPriority()
        {
            var testCampaignPriority = 0;
            string testSolutionContext = "Campaign";
            try
            {
                new Implementation.Campaign(_campaignId, _campaignDescription, 0,
                    _campaignServiceProvider.GetService<ICampaignConfig>(),
                    _campaignServiceProvider.GetService<ILoggerClient>());
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentException), ex.GetType());
                Assert.AreEqual($"Error: { testSolutionContext}: campaignPriority = { testCampaignPriority}", ex.Message.Replace(Environment.NewLine, " "));

            }

        }
    
        #endregion

        #region CampaignProcessLeadTests

        /// <summary>
        /// Mock the Validator to return a failure and then check that the ResultList has the correct key & value.
        /// </summary>
        [TestMethod]
        public void TestLeadValidationForACampaignReturnFalse()
        {
            // The process lead function adds results to the Campaign Results list
            var expecteResultsIfValidatorReturnsFalse = 6;
          
            // A campaign
            var campaign = new Implementation.Campaign(_campaignId, _campaignDescription, _campaignPriority,
                _campaignServiceProvider.GetService<ICampaignConfig>(),
                _campaignServiceProvider.GetService<ILoggerClient>());

            // Setting up the campaign validator to return false
            _campaignConfig.Setup(v => v.CampaignValidator.ValidLead(It.IsAny<ILeadEntity>())).Returns(false);

            // Let the Campaign Process the Lead
            _campaignResultList = campaign.ProcessLead(_testLeadEntity);

            //// Evaluate if the number of results in results list is as expected
            Assert.AreEqual(expecteResultsIfValidatorReturnsFalse, _campaignResultList.Count);

            // looks in the results collection to see if the validator said the lead failed
            var actualValue = _campaignResultList.SingleOrDefault(item =>
               item.Id == ResultKeys.ValidatorStatusKey)?.Value;
            Assert.AreEqual(actualValue.ToString(), ResultKeys.ResultKeysStatusEnum.Failed.ToString());
        }

        /// <summary>
        /// Mock the Validator to return a success and then check that the ResultList has the correct key & value.
        /// </summary>
        [TestMethod]
        public void TestLeadValidationForACampaignReturnTrue()
        {
            // The process lead function adds results to the Campaign Results list
            var expecteResultsIfValidatorReturnsTrue = 9;
            // A campaign
            var campaign = new Implementation.Campaign(_campaignId, _campaignDescription, _campaignPriority,
                _campaignServiceProvider.GetService<ICampaignConfig>(),
                _campaignServiceProvider.GetService<ILoggerClient>());

            // Setting up the validator to return true
            _campaignConfig.Setup(v => v.CampaignValidator.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);

            // Setting up the Campaign Controller - This will process through the Rules and Filters for the Campaign
            // Mock it to return true
            _campaignConfig.Setup(v => v.CampaignController.ConstraintMet(It.IsAny<ILeadEntity>())).Returns(true);

            // Let the Campaign Process the Lead
            _campaignResultList = campaign.ProcessLead(_testLeadEntity);

            // Evaluate if the number of results in results list is as expected
            Assert.AreEqual(expecteResultsIfValidatorReturnsTrue, _campaignResultList.Count);

            // looks in the results collection to see if the validator said the lead processed
            var expectedValue = _campaignResultList.SingleOrDefault(item =>
               item.Id == ResultKeys.ValidatorStatusKey)?.Value;
            Assert.AreEqual(expectedValue.ToString(), ResultKeys.ResultKeysStatusEnum.Processed.ToString());
        }

        /// <summary>
        /// Mock the Controller to return a failure and then check that the ResultList has the correct key & value.
        /// </summary>
        [TestMethod]
        public void TestCampaignIfControllerReturnsFailed()
        {
            // The process lead function adds results to the Campaign Results list
            var expecteResultsIfControlReturnsFalse = 7;
            // A campaign
            var campaign = new Implementation.Campaign(_campaignId, _campaignDescription, _campaignPriority,
                _campaignServiceProvider.GetService<ICampaignConfig>(),
                _campaignServiceProvider.GetService<ILoggerClient>());

            // Setting up the validator to return true
            _campaignConfig.Setup(v => v.CampaignValidator.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);

            // Setting up the Campaign Controller - This will process through the Rules and Filters for the Campaign
            // Mock it to return a failure
            _campaignConfig.Setup(v => v.CampaignController.ConstraintMet(It.IsAny<ILeadEntity>())).Returns(false);

            // Let the Campaign Process the Lead
            _campaignResultList = campaign.ProcessLead(_testLeadEntity);

            // Evaluate if the number of results in results list is as expected
            Assert.AreEqual(expecteResultsIfControlReturnsFalse, _campaignResultList.Count);

            // looks in the results collection to see if the validator said the lead processed
            var expectedValue = _campaignResultList.SingleOrDefault(item =>
               item.Id == ResultKeys.CampaignKeys.ControlStatusKey)?.Value;
            Assert.AreEqual(expectedValue.ToString(), ResultKeys.ResultKeysStatusEnum.Failed.ToString());
        }

        /// <summary>
        /// Mock the Controller to return a failure and then check that the ResultList has the correct key & value.
        /// </summary>
        [TestMethod]
        public void TestCampaignIfControllerReturnsSuccess()
        {
            // The process lead function adds results to the Campaign Results list
            var expecteResultsIfControlReturnsFalse = 9;
            // A campaign
            var campaign = new Implementation.Campaign(_campaignId, _campaignDescription, _campaignPriority,
                _campaignServiceProvider.GetService<ICampaignConfig>(),
                _campaignServiceProvider.GetService<ILoggerClient>());

            // Setting up the validator to return true
            _campaignConfig.Setup(v => v.CampaignValidator.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);

            // Setting up the Campaign Controller - This will process through the Rules and Filters for the Campaign
            // Mock it to return a success
            _campaignConfig.Setup(v => v.CampaignController.ConstraintMet(It.IsAny<ILeadEntity>())).Returns(true);

            // Let the Campaign Process the Lead
            _campaignResultList = campaign.ProcessLead(_testLeadEntity);

            // Evaluate if the number of results in results list is as expected
            Assert.AreEqual(expecteResultsIfControlReturnsFalse, _campaignResultList.Count);

            // looks in the results collection to see if the validator said the lead processed
            var expectedValue = _campaignResultList.SingleOrDefault(item =>
                item.Id == ResultKeys.CampaignKeys.ControlStatusKey)?.Value;
            Assert.AreEqual(expectedValue.ToString(), ResultKeys.ResultKeysStatusEnum.Processed.ToString());
        }

     
        /// <summary>
        /// Mocked the parameter process lead constructor test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ProcessLeadNullLeadEntityTest()
        {
            var campaign = new Implementation.Campaign(_campaignId, _campaignDescription, _campaignPriority,
                _campaignServiceProvider.GetService<ICampaignConfig>(),
                _campaignServiceProvider.GetService<ILoggerClient>());

            campaign.ProcessLead(null);            
        }

        #endregion

        #region ControllerTests

        /// <summary>
        /// Test successful constructor of the  Controller Implementation
        /// </summary>
        [TestMethod]
        public void ControllerSuccessTest()
        {
            new CampaignController(_campaignControllerList.Object ,_campaignServiceProvider.GetService<ILoggerClient>());
        }

        /// <summary>
        /// Test the constructor of the Campaign Controller Implementation with null controller list
        /// </summary>
        [TestMethod]
        public void CampaignControllerWithNullControllerList()
        {
            try
            {
                new CampaignController(null, _campaignServiceProvider.GetService<ILoggerClient>());
            }
            catch (Exception)
            {
                Assert.Fail("Error: There should be no exceptions if there are no controllers defined.");
            }
        }

        /// <summary>
        /// Test the constructor of the Campaign Controller Implementation with null LoggerClient
        /// </summary>
        [TestMethod]
        public void CampaignControllerWithNullLoggerClientTest()
        {
            try
            {
                new CampaignController(_campaignControllerList.Object, null);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentNullException), ex.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: loggerClient", ex.Message.Replace(Environment.NewLine, " "));
            }
        }
        /// <summary>
        /// Campaign Controller Test for ConstraintMet()
        /// </summary>
        [TestMethod]
        public void CampaignControllerConstraintMetTest()
        {
            // Add one controller for constraint met -  return true
            List<IController> controllerList = new List<IController> {_testConstraintMetController};
            var controller = new CampaignController(controllerList, _campaignServiceProvider.GetService<ILoggerClient>());
            var actualConstraintMet = controller.ConstraintMet(_testLeadEntity);                
            Assert.AreEqual(true, actualConstraintMet);
        }

        /// <summary>
        /// Campaign Controller Test for ConstraintMet() - controller returns false
        ///  </summary>
        [TestMethod]
        public void CampaignControllerConstraintNotMetTest()
        {
            // Add one controller for constraint met -  return false
            List<IController> controllerList = new List<IController> { _testConstraintNotMetController };
            var controller = new CampaignController(controllerList, _campaignServiceProvider.GetService<ILoggerClient>());
            var actualConstraintMet = controller.ConstraintMet(_testLeadEntity);
            Assert.AreEqual(false, actualConstraintMet);
        }

        /// <summary>
        /// Campaign Controller Test for Controller throwing exception
        /// </summary>
        [TestMethod]
        public void CampaignControllerWithExceptionTest()
        {
            // Add one controller that will throw a not implemented exception
            List<IController> controllerList = new List<IController> { _testControllerWithException };
            try
            {
                var controller = new CampaignController(controllerList, _campaignServiceProvider.GetService<ILoggerClient>());
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(NotImplementedException), ex.GetType());
            }
        }
        #endregion

        #region CampaignValidatorTests

        /// <summary>
        /// Test successful constructor of the Campaign Validator Implementation
        /// </summary>
        [TestMethod]
        public void CampaignControlSuccessTest()
        {
            new CampaignValidator(_campaignValidatorList.Object, _campaignServiceProvider.GetService<ILoggerClient>());
        }

        /// <summary>
        /// Test the constructor of the Campaign Validator Implementation with null controller list
        /// </summary>
        [TestMethod]
        public void CampaignValidatorWithNullValidatorList()
        {
            try
            {
                new CampaignValidator(null, _campaignServiceProvider.GetService<ILoggerClient>());
            }
            catch (Exception)
            {
                Assert.Fail("Error: There should be no exceptions if there are no validators defined.");
            }
        }

        /// <summary>
        /// Test the constructor of the Campaign Validator Implementation with null LoggerClient
        /// </summary>
        [TestMethod]
        public void CampaignValidatorWithNullLoggerClientTest()
        {
            try
            {
                new CampaignValidator(_campaignValidatorList.Object, null);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(ArgumentNullException), ex.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: loggerClient", ex.Message.Replace(Environment.NewLine, " "));
            }
        }
        /// <summary>
        /// Campaign Validator Test for ValidLead()
        /// </summary>
        [TestMethod]
        public void CampaignValidatorValidLeadTest()
        {
            // Add one controller for constraint met -  return true
            List<IValidator> controllerList = new List<IValidator> { _testValidLeadValidator };
            var controller = new CampaignValidator(controllerList, _campaignServiceProvider.GetService<ILoggerClient>());
            var actualValidLead = controller.ValidLead(_testLeadEntity);
            Assert.AreEqual(true, actualValidLead);
        }

        /// <summary>
        /// Campaign Validator Test for ValidLead() - controller returns false
        ///  </summary>
        [TestMethod]
        public void CampaignValidatorInValidLeadTest()
        {
            // Add one controller for constraint met -  return false
            List<IValidator> controllerList = new List<IValidator> { _testInvalidLeadValidator };
            var controller = new CampaignValidator(controllerList, _campaignServiceProvider.GetService<ILoggerClient>());
            var actualValidLead = controller.ValidLead(_testLeadEntity);
            Assert.AreEqual(false, actualValidLead);
        }

        /// <summary>
        /// Campaign Validator Test for Validator throwing exception
        /// </summary>
        [TestMethod]
        public void CampaignValidatorWithExceptionTest()
        {
            // Add one controller that will throw a not implemented exception
            List<IValidator> controllerList = new List<IValidator> { _testValidatorWithException };
            try
            {
                var controller = new CampaignValidator(controllerList, _campaignServiceProvider.GetService<ILoggerClient>());
            }
            catch (Exception ex)
            {
                Assert.AreEqual(typeof(NotImplementedException), ex.GetType());
            }
        }
        #endregion
    }
}
