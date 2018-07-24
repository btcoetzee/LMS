namespace LMS.CampaignManager.UnitTests
{

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq.Expressions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using CampaignManager.Implementation;
    using LMS.Campaign.Interface;
    using LMS.LoggerClient.Interface;
    using LMS.Validator.Interface;
    using LMS.Decorator.Interface;
    using LMS.CampaignManager.Resolver.Interface;
    using LMS.CampaignManager.Subscriber.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LeadEntity.Components;


    [TestClass]
    public class CampaignManagerTests
    {

        private static IServiceProvider _campaignManagerServiceProvider;
        private Mock<ICampaignManagerSubscriber> _campaignManagerSubscriber;
        private Mock<List<ICampaign>> _campaignCollection;
        private Mock<IDecorator> _campaignManagerDecorator;
        private Mock<List<IValidator>> _campaignManagerValidatorCollection;
        private Mock<ICampaignManagerResolver> _campaignManagerResolver;
        private Mock<ILoggerClient> _loggerClient;
        private static readonly string[] EmptyResultArray = new string[] { };
        private ILeadEntity _testLleadEntity;


        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Components that is part of the CampaignManager
            _campaignManagerSubscriber = new Mock<ICampaignManagerSubscriber>();
            _campaignCollection = new Mock<List<ICampaign>> ();
            _campaignManagerValidatorCollection = new Mock<List<IValidator>>();
            _campaignManagerDecorator = new Mock<IDecorator>();
            _campaignManagerResolver = new Mock<ICampaignManagerResolver>();
            _loggerClient = new Mock<ILoggerClient>();
      

            // Create Service Providers 
            _campaignManagerServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ICampaignManagerSubscriber), _campaignManagerSubscriber)
                .AddSingleton(typeof(List<ICampaign>), _campaignCollection)
                .AddSingleton(typeof(List<IValidator>), _campaignManagerValidatorCollection)
                .AddSingleton(typeof(IDecorator), _campaignManagerDecorator.Object)
                .AddSingleton(typeof(ICampaignManagerResolver), _campaignManagerResolver)
                .AddSingleton(typeof(ILoggerClient), _loggerClient.Object)
                .BuildServiceProvider();

            // Create a leadEntity
            CreateLeadEntity();
        }

        /// <summary>
        /// Create an Instance of the LeadEntity
        /// </summary>
        private class TestLeadEntityClass : ILeadEntity
        {
    
            public IContext[] Context { get; set; }
            public IProperty[] Properties { get; set; }
            public ISegment[] Segments { get; set; }
            public IResultCollection ResultCollection { get; set; }
        }

        struct TestLeadEntityResultClass : IResult
        {
            public TestLeadEntityResultClass(string id, object value)
            {
                Id = id;
                Value = value;
            }

            public string Id { get; private set; }

            public object Value { get; private set; }
        }

        void CreateLeadEntity()
        {
            _testLleadEntity = new TestLeadEntityClass()
            {
                Context = new IContext[]
                    {},
                Properties = new IProperty[]
                    {},
                Segments = new ISegment[]
                    {},
                ResultCollection = new DefaultResultCollection()
            };

        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _campaignManagerSubscriber.VerifyAll();
            _campaignManagerSubscriber = null;
            _campaignCollection.VerifyAll();
            _campaignCollection = null;
            _campaignManagerValidatorCollection.VerifyAll();
            _campaignManagerValidatorCollection = null;
            _campaignManagerResolver.VerifyAll();
            _campaignManagerResolver = null;
            _loggerClient.VerifyAll();
            _loggerClient = null;
            _campaignManagerServiceProvider = null;
        }
        #region ConstructorTests

        /// <summary>
        /// Campaing Constructor Test
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorTest()
        {
            var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                _campaignCollection.Object.ToArray(),  _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                _campaignManagerResolver.Object, _loggerClient.Object);
        }

        /// <summary>
        /// Campaing Constructor Test with a Null CampaignManagerSubscriber.
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullCampaignSubscriberTest()
        {

            try
            {
                var campaignManager = new CampaignManager(null,
                    _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                    _campaignManagerResolver.Object, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the CampaignManagerSubscriber is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignManagerSubscriber", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        /// <summary>
        /// Campaing Constructor Test with a Null Campaign List.
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullCampaignListTest()
        {
            try
            {
                var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                    null, _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                    _campaignManagerResolver.Object, _loggerClient.Object);
               Assert.Fail("An Argument Null Exception is expected when the campaignCollection is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignCollection", exception.Message.Replace(Environment.NewLine, " "));
            }
        }
        /// <summary>
        /// Campaing Constructor Test with a Null CampaignValidatorCollection.
        /// This is an optional parameter - so also test other constructor
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullCampaignValidatorCollection()
        {
            new CampaignManager(_campaignManagerSubscriber.Object,
                _campaignCollection.Object.ToArray(), null, _campaignManagerDecorator.Object, 
                _campaignManagerResolver.Object, _loggerClient.Object);

            new CampaignManager(_campaignManagerSubscriber.Object,
                _campaignCollection.Object.ToArray(), _campaignManagerDecorator.Object, 
                _campaignManagerResolver.Object, _loggerClient.Object);
        }

        /// <summary>
        /// Campaing Constructor Test with a Null CampaignManagerResolver.
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullCampaignManagerResolver()
        {
            try
            {
                var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                    _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object, 
                    null, _loggerClient.Object);
              
                Assert.Fail("An Argument Null Exception is expected when the CampaignManagerResolver is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignManagerResolver", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        /// <summary>
        /// Campaing Constructor Test with a Null LoggerClient.
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullLoggerClientTest()
        {
            try
            {
                var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                    _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                    _campaignManagerResolver.Object, null);
                Assert.Fail("An Argument Null Exception is expected when the LoggerClient is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: loggerClient", exception.Message.Replace(Environment.NewLine, " "));
            }
        }
        #endregion

        /// <summary>
        /// CampaignManager Test with a No Campaigns.
        /// </summary>
        [TestMethod]
        public void CampaignManagerProcessEmptyCampaignTest()
        {
            // No Campaigns set up, so result list should be empty
            var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                _campaignManagerResolver.Object, _loggerClient.Object);
            var resultList = campaignManager.ProcessCampaigns(_testLleadEntity);
            Assert.AreEqual(resultList.Length, 0);
        }

        /// <summary>
        /// Execute the Campaign Manager with multiple campaigns.
        /// </summary>
        [TestMethod]
        public void CampaignManagerProcessMultipleCampaignTest()
        {
            //const string testMessage = "Hello from Milo! :-)";
            //const string responseFormat = "Campaign {0} complete.";

            //const int campaignCount = 3;
            //var testCampaignCollection = new ICampaign[campaignCount];

            //for (var campaignIndex = 0; campaignIndex < campaignCount; campaignIndex++)
            //{
            //    var id = campaignIndex;
            //    var mock = new Mock<ICampaign>();


            //    // Changing this for now - but chang it to be a ILeadEntity in the end.
            //    mock.Setup(campaign => campaign.ProcessLead(It.Is<ILeadEntity>(_testLeadEntity)))
            //        .Returns(string.Format(responseFormat, id));

            //    //mock.Setup(campaign => campaign.ProcessLead(It.Is<String>(s => s != String.Empty)))
            //    //    .Returns(string.Format(responseFormat, id));


            //    // TODO - put this back and use ILeadEntity instead of strings
            //    //mock.Setup(campaign => campaign.ProcessLead((It.IsIn(testMessage))))
            //    //    .Returns(string.Format(responseFormat, id));

            //    testCampaignCollection[campaignIndex] = mock.Object;
            //}

            //var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
            //    testCampaignCollection, _campaignManagerValidatorCollection.Object.ToArray(),
            //    _campaignManagerResolver.Object, _loggerClient.Object);
            //var results = campaignManager.ProcessCampaigns(_testLleadEntity);

            //// Check that results were created
            //Assert.IsNotNull(results);

            //// Check for expected result message
            //var campaignIx = 2;
            //Assert.AreEqual(results[campaignIx], string.Format(responseFormat, campaignIx));

            //// Check for expected number of results
            //Assert.AreEqual(campaignCount, results.Length);
        }

        public Expression<Func<ILeadEntity, bool>> _testLeadEntity { get; private set; }

        /// <summary>
        /// Campaign Manager SetupAddOnReceiveActionToChannel Test
        /// </summary>
        [TestMethod]
        public void CampaignManagerSubscriberAddOnReceiveActionToChannel()
        {
            _campaignManagerSubscriber.Setup(svc => svc.SetupAddOnReceiveActionToChannel(It.IsAny<Action<ILeadEntity>>()))
                .Callback((Action<ILeadEntity> action) => action(_testLleadEntity));

            var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                _campaignManagerResolver.Object, _loggerClient.Object);

            // TODO complete!

        }

    }
}