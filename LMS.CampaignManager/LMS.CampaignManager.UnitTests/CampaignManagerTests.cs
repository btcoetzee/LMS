using System;
using System.Collections.Generic;
using System.Threading;
using Compare.Components.Notification.Contract;
using Compare.Services.LMS.CampaignManager.Interface;
using Compare.Services.LMS.CampaignManager.Implementation;
using Compare.Services.LMS.Campaign.Interface;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Validator.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Components;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;
using Compare.Services.LMS.Modules.LoggerClient.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Compare.Services.LMS.CampaignManager.UnitTests
{
    [TestClass]
    public class CampaignManagerTests
    {

        private static IServiceProvider _campaignManagerServiceProvider;
        private Mock<ICampaignManagerConfig> _campaignManagerConfig;
        private Mock<ILoggerClient> _loggerClient;
        private static readonly string[] EmptyResultArray = new string[] { };
        private DefaultLeadEntity _testLeadEntity;
        private List<IResult> _testCampaignManagerResultList;
        private readonly int _testMultiThreadedWaitTime = 200;
        private readonly int _testCampaignManagerId = 1;
 
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Components that is part of the CampaignManager
            _campaignManagerConfig = new Mock<ICampaignManagerConfig>();
            _loggerClient = new Mock<ILoggerClient>();
      

            // Create Service Providers 
            _campaignManagerServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ICampaignManagerConfig), _campaignManagerConfig)
                .AddSingleton(typeof(ILoggerClient), _loggerClient.Object)
                .BuildServiceProvider();

            // Create a leadEntity
            CreateLeadEntity();
            CreateACampaignResult();
        }
        void CreateLeadEntity()
        {
            _testLeadEntity = new DefaultLeadEntity()
            {
                Context = new IContext[] { },
                Properties = new IProperty[] { },
                Segments = new ISegment[] { },
                ResultCollection = new DefaultResultCollection()
            };
        }

        void CreateACampaignResult() => _testCampaignManagerResultList = new List<IResult>()
            {
                new DefaultResult(ResultKeys.CampaignKeys.LeadSuccessStatusKey.ToString(), ResultKeys.ResultKeysStatusEnum.Processed.ToString())
                
            };

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _campaignManagerConfig.VerifyAll();
            _campaignManagerConfig = null;
            _loggerClient.VerifyAll();
            _loggerClient = null;
            _campaignManagerServiceProvider = null;
        }
        #region ConstructorTests

        /// <summary>
        /// Campaign Manager Constructor Test
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorTest()
        {
            // The Subscriber is set up in the constructor
            var testCampaignManagagerSubscriber = new Mock<ISubscriber>();
            // Mock the call to the subscriber
            testCampaignManagagerSubscriber.Setup(d => d.SetupAddOnReceiveActionToChannel(It.IsAny<Action<ILeadEntity>>())).Verifiable();
            // Mock CampaignConfig to return the above Subscriber
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerSubscriber).Returns(testCampaignManagagerSubscriber.Object);
            var campaignManager = new Implementation.CampaignManager(_testCampaignManagerId, _campaignManagerConfig.Object,  _loggerClient.Object);
        }

        /// <summary>
        /// Campaign Manager Constructor Test With Null CampaignManagerConfig
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullConfigTest()
        {
            try
            {
                var campaignManager = new Implementation.CampaignManager(_testCampaignManagerId,  null, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the campaignManagerConfig is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignManagerConfig", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

   
        /// <summary>
        /// Campaign Manager Constructor Test with a Null LoggerClient.
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullLoggerClientTest()
        {
            try
            {
                var campaignManager = new Implementation.CampaignManager(_testCampaignManagerId, _campaignManagerConfig.Object, null);
                Assert.Fail("An Argument Null Exception is expected when the LoggerClient is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: loggerClient", exception.Message.Replace(Environment.NewLine, " "));
            }
        }
        #endregion

        #region CampaignManagerDriverTests

        /// <summary>
        /// Campaign Manager Driver Test with a No Campaigns and Null LeadEntity.
        /// </summary>
        [TestMethod]
        public void CampaignManagerDriverTestWithNullLeadEntityObject()
        {

            // The Subscriber is set up in the constructor
            var testCampaignManagagerSubscriber = new Mock<ISubscriber>();
            // Mock the call to the subscriber
            testCampaignManagagerSubscriber.Setup(d => d.SetupAddOnReceiveActionToChannel(It.IsAny<Action<ILeadEntity>>())).Verifiable();
            // Mock CampaignConfig to return the above Subscriber
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerSubscriber).Returns(testCampaignManagagerSubscriber.Object);

            // No Campaigns set up, not necessary to mock other components
            var campaignManager = new Implementation.CampaignManager(_testCampaignManagerId, _campaignManagerConfig.Object, _loggerClient.Object);

            try
            {
                campaignManager.CampaignManagerDriver(null);
            }
            catch (Exception exception)
            {

                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: leadEntity",
                    exception.Message.Replace(Environment.NewLine, " "));
  
            }
        }
        /// <summary>
        /// Campaign Manager Driver Test with a No Campaigns.
        /// </summary>
        [TestMethod]
        public void CampaignManagerDriverEmptyCampaignTest()
        {
            // The Subscriber is set up in the constructor
            var testCampaignManagagerSubscriber = new Mock<ISubscriber>();
            // Mock the call to the subscriber
            testCampaignManagagerSubscriber.Setup(d => d.SetupAddOnReceiveActionToChannel(It.IsAny<Action<ILeadEntity>>())).Verifiable();
            // Mock CampaignConfig to return the above Subscriber
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerSubscriber).Returns(testCampaignManagagerSubscriber.Object);

            // No Campaigns set up, mock the Decorator that is called once
            var decorator = new Mock<IDecorator>();
            decorator.Setup(d => d.DecorateLead(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>>())).Verifiable();
            // Mock CampaignConfig to return the above Decorator
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerDecorator).Returns(decorator.Object);

            var campaignManager = new Implementation.CampaignManager(_testCampaignManagerId, _campaignManagerConfig.Object, _loggerClient.Object);
            campaignManager.CampaignManagerDriver(_testLeadEntity);
        }

        /// <summary>
        /// Set up Multiple Campaigns to be managed.  The Validator Collection 
        /// contains a validator that return false;
        /// </summary>
        [TestMethod]
        public void CampaignManagerDriverMultipleCampaignsWithValidatorReturningFalseTest()
        {
            // The Subscriber is set up in the constructor
            var testCampaignManagagerSubscriber = new Mock<ISubscriber>();
            // Mock the call to the subscriber
            testCampaignManagagerSubscriber.Setup(d => d.SetupAddOnReceiveActionToChannel(It.IsAny<Action<ILeadEntity>>())).Verifiable();
            // Mock CampaignConfig to return the above Subscriber
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerSubscriber).Returns(testCampaignManagagerSubscriber.Object);

            // No Campaigns set up, mock the Decorator that is called once
            var decorator = new Mock<IDecorator>();
            decorator.Setup(d => d.DecorateLead(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>>())).Verifiable();
            // Mock CampaignConfig to return the above Decorator
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerDecorator).Returns(decorator.Object);

            // Set up 3 Campaigns
            const int testCampaignCnt = 3;
            var testCampaignCollection = new ICampaign[testCampaignCnt];
            for (var campaignIndex = 0; campaignIndex < testCampaignCnt; campaignIndex++)
            {
                var id = campaignIndex;
                var mock = new Mock<ICampaign>();
                mock.Setup(campaign => campaign.ProcessLead(It.IsAny<ILeadEntity>()))
                    .Returns(_testCampaignManagerResultList);
                testCampaignCollection[id] = mock.Object;
            }
            // Mock the Campaign Config to return the above Campaign Collection
            _campaignManagerConfig.SetupGet(cc =>
                cc.CampaignCollection).Returns(testCampaignCollection);

            // Mock the Campaign Manager Validator to return false
            var testValidator = new Mock<IValidator>();
            testValidator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(false);

            // Mock the Campaign Config to return the testValidator as the CampaignMangerValidator
            _campaignManagerConfig.SetupGet(cc =>
                cc.CampaignManagerValidator).Returns(testValidator.Object);

            // Intitialize the CampaignManager with the mocked testCampaignCollection
            var campaignManager = new Implementation.CampaignManager(_testCampaignManagerId, _campaignManagerConfig.Object, _loggerClient.Object);
            campaignManager.CampaignManagerDriver(_testLeadEntity);
           
        }

        /// <summary>
        /// Set up Multiple Campaigns to be managed.
        /// </summary>
        [TestMethod]
        public void CampaignManagerDriverMultipleCampaignTest()
        {

            // The Subscriber is set up in the constructor
            var testCampaignManagagerSubscriber = new Mock<ISubscriber>();
            // Mock the call to the subscriber
            testCampaignManagagerSubscriber.Setup(d => d.SetupAddOnReceiveActionToChannel(It.IsAny<Action<ILeadEntity>>())).Verifiable();
            // Mock CampaignConfig to return the above Subscriber
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerSubscriber).Returns(testCampaignManagagerSubscriber.Object);

            // No Campaigns set up, mock the Decorator that is called once
            var testCampaignManagerDecorator = new Mock<IDecorator>();
            testCampaignManagerDecorator.Setup(d => d.DecorateLead(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>>())).Verifiable();
            // Mock CampaignConfig to return the above Decorator
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerDecorator).Returns(testCampaignManagerDecorator.Object);

            // Set up 3 Campaigns
            const int testCampaignCnt = 3;
            var testCampaignCollection = new ICampaign[testCampaignCnt];
            for (var campaignIndex = 0; campaignIndex < testCampaignCnt; campaignIndex++)
            {
                var id = campaignIndex;
                var mock = new Mock<ICampaign>();
                mock.Setup(campaign => campaign.ProcessLead(It.IsAny<ILeadEntity>()))
                    .Returns(_testCampaignManagerResultList);
                testCampaignCollection[id] = mock.Object;
            }
            // Mock the Campaign Config to return the above Campaign Collection
            _campaignManagerConfig.SetupGet(cc =>
                cc.CampaignCollection).Returns(testCampaignCollection);

            // Mock the Campaign Manager Validator to return true
            var testValidator = new Mock<IValidator>();
            testValidator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);

            // Mock the Campaign Config to return the testValidator as the CampaignMangerValidator
            _campaignManagerConfig.SetupGet(cc =>
                cc.CampaignManagerValidator).Returns(testValidator.Object);

            // The Resolver 
            var testCampaignManagagerResolver = new Mock<IResolver>();
            // Mock the call to the resolver
            testCampaignManagagerResolver.Setup(d => d.ResolveLead(It.IsAny<ILeadEntity>())).Verifiable();
            // Mock CampaignConfig to return the above Resolver
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerResolver).Returns(testCampaignManagagerResolver.Object);

            // Intitialize the CampaignManager with the mocked testCampaignCollection
            var campaignManager = new Implementation.CampaignManager(_testCampaignManagerId, _campaignManagerConfig.Object, _loggerClient.Object);
            campaignManager.CampaignManagerDriver(_testLeadEntity);

  
            // A sleep here - not a great solution - to wait for the campaign tasks to finish and ensure that
            // the resolver, publisher and decorator are called in the CampaignManagerProcessResults() function
            // which is set up via ContinueWith() of the Campaign Task.
            Thread.Sleep(_testMultiThreadedWaitTime);
        }


        /// <summary>
        /// Set up Multiple Campaigns to be managed and the Resolver that throws an exception
        ///           
        /// Mock the Campaign Manager Resolver, Decorator and Publisher - If these are executed
        /// it means that campaign manager executed the Campaign tasks successfully.
        /// The Resolver throws exception, so the Publisher should not execute
        /// </summary>
        [TestMethod]
        public void CampaignManagerDriverMultipleCampaignTestWithResolverException()
        {

            // The Subscriber is set up in the constructor
            var testCampaignManagagerSubscriber = new Mock<ISubscriber>();
            // Mock the call to the subscriber
            testCampaignManagagerSubscriber.Setup(d => d.SetupAddOnReceiveActionToChannel(It.IsAny<Action<ILeadEntity>>())).Verifiable();
            // Mock CampaignConfig to return the above Subscriber
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerSubscriber).Returns(testCampaignManagagerSubscriber.Object);

            // No Campaigns set up, mock the Decorator that is called once
            var testCampaignManagerDecorator = new Mock<IDecorator>();
            testCampaignManagerDecorator.Setup(d => d.DecorateLead(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>>())).Verifiable();
            // Mock CampaignConfig to return the above Decorator
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerDecorator).Returns(testCampaignManagerDecorator.Object);

            // Set up 3 Campaigns
            const int testCampaignCnt = 3;
            var testCampaignCollection = new ICampaign[testCampaignCnt];
            for (var campaignIndex = 0; campaignIndex < testCampaignCnt; campaignIndex++)
            {
                var id = campaignIndex;
                var mock = new Mock<ICampaign>();
                mock.Setup(campaign => campaign.ProcessLead(It.IsAny<ILeadEntity>()))
                    .Returns(_testCampaignManagerResultList);
                testCampaignCollection[id] = mock.Object;
            }
            // Mock the Campaign Config to return the above Campaign Collection
            _campaignManagerConfig.SetupGet(cc =>
                cc.CampaignCollection).Returns(testCampaignCollection);

            // Mock the Campaign Manager Validator to return true
            var testValidator = new Mock<IValidator>();
            testValidator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);

            // Mock the Campaign Config to return the testValidator as the CampaignMangerValidator
            _campaignManagerConfig.SetupGet(cc =>
                cc.CampaignManagerValidator).Returns(testValidator.Object);

            // The Resolver 
            var testCampaignManagagerResolver = new Mock<IResolver>();
            // Mock the call to the resolver to return an exception
            testCampaignManagagerResolver.Setup(d => d.ResolveLead(It.IsAny<ILeadEntity>())).Throws(new ArgumentNullException($"ResolveLeads"));
            // Mock CampaignConfig to return the above Resolver
            _campaignManagerConfig.SetupGet(cc => cc.CampaignManagerResolver).Returns(testCampaignManagagerResolver.Object);




            //_campaignManagerResolver.Setup(c => c.ResolveLeads(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>[]>())).Throws(new ArgumentNullException(
            //    $"ResolveLeads"));
            //_campaignManagerDecorator.Setup(c => c.DecorateLead(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>>())).Verifiable();
            //_campaignManagerPublisher.Verify(c => c.PublishLead(It.IsAny<ILeadEntity>()), Times.Never());



            try
            {

                // Intitialize the CampaignManager with the mocked testCampaignCollection
                var campaignManager = new Implementation.CampaignManager(_testCampaignManagerId, _campaignManagerConfig.Object, _loggerClient.Object);
                campaignManager.CampaignManagerDriver(_testLeadEntity);
             
                // A sleep here - not a great solution - to wait for the campaign tasks to finish and ensure that
                // the resolver thows exception and the decorator are called in the CampaignManagerProcessResults() function
                // which is set up via ContinueWith() of the Campaign Task.
                Thread.Sleep(_testMultiThreadedWaitTime);
            }
            catch (Exception exception)
            {

                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: ResolveLeads",
                    exception.Message.Replace(Environment.NewLine, " "));
            }


        }
        #endregion

        //        public Expression<Func<ILeadEntity, bool>> _testLeadEntity { get; private set; }


    }
}