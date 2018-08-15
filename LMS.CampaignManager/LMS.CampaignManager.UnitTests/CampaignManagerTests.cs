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
    using LMS.LoggerClient.Interface;
    using LMS.LeadEntity.Interface.Constants;
    using LMS.CampaignManager.Resolver.Interface;
    using LMS.CampaignManager.Subscriber.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LeadEntity.Components;
    using LMS.CampaignManager.Decorator.Interface;
    using LMS.CampaignManager.Validator.Interface;
    using LMS.CampaignManager.Publisher.Interface;
    using LMS.Campaign.Interface;

    [TestClass]
    public class CampaignManagerTests
    {

        private static IServiceProvider _campaignManagerServiceProvider;
        private Mock<ICampaignManagerSubscriber> _campaignManagerSubscriber;
        private Mock<List<ICampaign>> _campaignCollection;
        private Mock<ICampaignManagerDecorator> _campaignManagerDecorator;
        private Mock<List<ICampaignManagerValidator>> _campaignManagerValidatorCollection;
        private Mock<ICampaignManagerResolver> _campaignManagerResolver;
        private Mock<ICampaignManagerPublisher> _campaignManagerPublisher;
        private Mock<ILoggerClient> _loggerClient;
        private static readonly string[] EmptyResultArray = new string[] { };
        private DefaultLeadEntity _testLeadEntity;
        private List<IResult> _testCampaignManagerResultList;
        private readonly int _testMultiThreadedWaitTime = 200;
 
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Components that is part of the CampaignManager
            _campaignManagerSubscriber = new Mock<ICampaignManagerSubscriber>();
            _campaignCollection = new Mock<List<ICampaign>> ();
            _campaignManagerValidatorCollection = new Mock<List<ICampaignManagerValidator>>();
            _campaignManagerDecorator = new Mock<ICampaignManagerDecorator>();
            _campaignManagerResolver = new Mock<ICampaignManagerResolver>();
            _campaignManagerPublisher = new Mock<ICampaignManagerPublisher>();
            _loggerClient = new Mock<ILoggerClient>();
      

            // Create Service Providers 
            _campaignManagerServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ICampaignManagerSubscriber), _campaignManagerSubscriber)
                .AddSingleton(typeof(List<ICampaign>), _campaignCollection)
                .AddSingleton(typeof(List<ICampaignManagerValidator>), _campaignManagerValidatorCollection)
                .AddSingleton(typeof(ICampaignManagerDecorator), _campaignManagerDecorator.Object)
                .AddSingleton(typeof(ICampaignManagerResolver), _campaignManagerResolver)
                .AddSingleton(typeof(ICampaignManagerPublisher), _campaignManagerPublisher)
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

            _campaignManagerSubscriber.VerifyAll();
            _campaignManagerSubscriber = null;
            _campaignCollection.VerifyAll();
            _campaignCollection = null;
            _campaignManagerValidatorCollection.VerifyAll();
            _campaignManagerValidatorCollection = null;
            _campaignManagerDecorator.VerifyAll();
            _campaignManagerDecorator = null;
            _campaignManagerResolver.VerifyAll();
            _campaignManagerResolver = null;
            _campaignManagerPublisher.VerifyAll();
            _campaignManagerPublisher = null;
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
            var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                _campaignCollection.Object.ToArray(),  _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                _campaignManagerResolver.Object, _campaignManagerPublisher.Object, _loggerClient.Object);
        }

        /// <summary>
        /// Campaign Manager Constructor Test With Null Publisher
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullPublisherTest()
        {
            try
            {
                var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                    _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                    _campaignManagerResolver.Object, null, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the campaignManagerPublisher is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignManagerPublisher", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        /// <summary>
        /// Campaign Manager Constructor Test With Null Decorator
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullDecoratorTest()
        {
           
            try
            {
                var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                    _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), null,
                    _campaignManagerResolver.Object, _campaignManagerPublisher.Object, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the campaignManagerDecorator is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignManagerDecorator", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        /// <summary>
        /// Campaign Manager Constructor Test with a Null CampaignManagerSubscriber.
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullCampaignSubscriberTest()
        {

            try
            {
                var campaignManager = new CampaignManager(null,
                    _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                    _campaignManagerResolver.Object, _campaignManagerPublisher.Object, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the CampaignManagerSubscriber is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignManagerSubscriber", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        /// <summary>
        /// Campaign Manager Constructor Test with a Null Campaign List.
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullCampaignListTest()
        {
            try
            {
                var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                    null, _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                    _campaignManagerResolver.Object, _campaignManagerPublisher.Object, _loggerClient.Object);
               Assert.Fail("An Argument Null Exception is expected when the campaignCollection is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignCollection", exception.Message.Replace(Environment.NewLine, " "));
            }
        }
        /// <summary>
        /// Campaign Manager Constructor Test with a Null CampaignValidatorCollection.
        /// This is an optional parameter - so also test other constructor
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullCampaignValidatorCollection()
        {
            new CampaignManager(_campaignManagerSubscriber.Object,
                _campaignCollection.Object.ToArray(), null, _campaignManagerDecorator.Object, 
                _campaignManagerResolver.Object, _campaignManagerPublisher.Object, _loggerClient.Object);

            new CampaignManager(_campaignManagerSubscriber.Object,
                _campaignCollection.Object.ToArray(), _campaignManagerDecorator.Object, 
                _campaignManagerResolver.Object, _campaignManagerPublisher.Object, _loggerClient.Object);
        }

        /// <summary>
        /// Campaign Manager Constructor Test with a Null CampaignManagerResolver.
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullCampaignManagerResolver()
        {
            try
            {
                var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                    _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object, 
                    null, _campaignManagerPublisher.Object, _loggerClient.Object);
              
                Assert.Fail("An Argument Null Exception is expected when the CampaignManagerResolver is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignManagerResolver", exception.Message.Replace(Environment.NewLine, " "));
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
                var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                    _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                    _campaignManagerResolver.Object, _campaignManagerPublisher.Object, null);
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
            // No Campaigns set up, not necessary to mock other components
            var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                _campaignManagerResolver.Object, _campaignManagerPublisher.Object, _loggerClient.Object);

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
            // No Campaigns set up, so do not have to mock other components
            var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
                _campaignCollection.Object.ToArray(), _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                _campaignManagerResolver.Object, _campaignManagerPublisher.Object, _loggerClient.Object);
           campaignManager.CampaignManagerDriver(_testLeadEntity);

        }

        /// <summary>
        /// Set up Multiple Campaigns to be managed.  The Validator Collection 
        /// contains a validator that return false;
        /// </summary>
        [TestMethod]
        public void CampaignManagerDriverMultipleCampaignsWithValidatorReturningFalseTest()
        {

            // Mock the Campaign Manager Validator Collection where the first validator returns false
            var testValidator = new Mock<ICampaignManagerValidator>();
            testValidator.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(false);
            _campaignManagerValidatorCollection.Object.Add(testValidator.Object);


            // The resolver and publisher should not be called
            // The decorator should be called when the lead is classified as not valid.
            _campaignManagerResolver.Verify(c => c.ResolveLeads(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>[]>()),Times.Never());
            _campaignManagerDecorator.Setup(c => c.DecorateLead(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>>())).Verifiable();
            _campaignManagerPublisher.Verify(c => c.PublishLead(It.IsAny<ILeadEntity>()), Times.Never());


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

            // Intitialize the CampaignManager with the mocked testCampaignCollection
            var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
               testCampaignCollection, _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                _campaignManagerResolver.Object, _campaignManagerPublisher.Object, _loggerClient.Object);
            campaignManager.CampaignManagerDriver(_testLeadEntity);

           
        }

        /// <summary>
        /// Set up Multiple Campaigns to be managed.
        /// </summary>
        [TestMethod]
        public void CampaignManagerDriverMultipleCampaignTest()
        {

            // Mock the Campaign Manager Resolver, Decorator and Publisher - If these are executed
            // it means that campaign manager executed the Campaign tasks successfully.
            _campaignManagerResolver.Setup(c => c.ResolveLeads(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>[]>())).Verifiable();
            _campaignManagerDecorator.Setup(c => c.DecorateLead(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>>())).Verifiable();
            _campaignManagerPublisher.Setup(c => c.PublishLead(It.IsAny<ILeadEntity>())).Verifiable();

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

            // Intitialize the CampaignManager with the mocked testCampaignCollection
            var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
               testCampaignCollection, _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                _campaignManagerResolver.Object, _campaignManagerPublisher.Object, _loggerClient.Object);
            campaignManager.CampaignManagerDriver(_testLeadEntity);

            // A sleep here - not a great solution - to wait for the campaign tasks to finish and ensure that
            // the resolver, publisher and decorator are called in the CampaignManagerProcessResults() function
            // which is set up via ContinueWith() of the Campaign Task.
            Thread.Sleep(_testMultiThreadedWaitTime);
        }


        /// <summary>
        /// Set up Multiple Campaigns to be managed and the Resolver that throws an exception
        /// </summary>
        [TestMethod]
        public void CampaignManagerDriverMultipleCampaignTestWithResolverException()
        {

            // Mock the Campaign Manager Resolver, Decorator and Publisher - If these are executed
            // it means that campaign manager executed the Campaign tasks successfully.
            // The Resolver throws exception, so the Publisher should not execute
            _campaignManagerResolver.Setup(c => c.ResolveLeads(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>[]>())).Throws(new ArgumentNullException(
                $"ResolveLeads"));
            _campaignManagerDecorator.Setup(c => c.DecorateLead(It.IsAny<ILeadEntity>(), It.IsAny<List<IResult>>())).Verifiable();
            _campaignManagerPublisher.Verify(c => c.PublishLead(It.IsAny<ILeadEntity>()), Times.Never()); 

            var random = new Random();
            const int minSleepInMs = 1000;
            const int maxSleepInMs = 5000;

            // Set up 3 Campaigns
            const int testCampaignCnt = 3;
            var testCampaignCollection = new ICampaign[testCampaignCnt];

            for (var campaignIndex = 0; campaignIndex < testCampaignCnt; campaignIndex++)
            {
                var id = campaignIndex;
                var mock = new Mock<ICampaign>();
                mock.Setup(campaign => campaign.ProcessLead(It.IsAny<ILeadEntity>()))
                    //.Callback(() => Thread.Sleep(random.Next(minSleepInMs, maxSleepInMs)))
                    .Returns(_testCampaignManagerResultList);

                testCampaignCollection[id] = mock.Object;
            }

            // Intitialize the CampaignManager with the mocked testCampaignCollection
            var campaignManager = new CampaignManager(_campaignManagerSubscriber.Object,
               testCampaignCollection, _campaignManagerValidatorCollection.Object.ToArray(), _campaignManagerDecorator.Object,
                _campaignManagerResolver.Object, _campaignManagerPublisher.Object, _loggerClient.Object);

            try
            {
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