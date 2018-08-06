using System.Linq;

namespace LMS.CampaignManager.Decorator.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq.Expressions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using LMS.LoggerClient.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LeadEntity.Components;
    using LMS.CampaignManager.Decorator.Interface;
    using LMS.LeadEntity.Interface.Constants;
    using LMS.CampaignManager.Decorator.Implementation;

    [TestClass]
    public class CampaignManagerDecoratorTests
    {
        private static IServiceProvider _campaignManagerServiceProvider;
        private Mock<ICampaignManagerDecorator> _campaignManagerDecorator;
        private Mock<ILoggerClient> _loggerClient;
        private ILeadEntity _testLeadEntity;
        private List<IResult> _testCampaignManagerResultList;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Components that is part of the CampaignManagerDecorator
            _campaignManagerDecorator = new Mock<ICampaignManagerDecorator>();
            _loggerClient = new Mock<ILoggerClient>();


            // Create Service Providers 
            _campaignManagerServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ICampaignManagerDecorator), _campaignManagerDecorator.Object)
                .AddSingleton(typeof(ILoggerClient), _loggerClient.Object)
                .BuildServiceProvider();

            // Create a leadEntity
            CreateTestLeadEntity();
            CreateTestCampaignManagerResultList();
        }
        void CreateTestLeadEntity()
        {
            _testLeadEntity = new DefaultLeadEntity()
            {
                Context = new IContext[] { },
                Properties = new IProperty[] { },
                Segments = new ISegment[] { },
                ResultCollection = new DefaultResultCollection()
            };

        }
        void CreateTestCampaignManagerResultList() => _testCampaignManagerResultList = new List<IResult>()
        {
            new DefaultResult(ResultKeys.CampaignManagerKeys.SubscriberStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString())
        };

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _campaignManagerDecorator.VerifyAll();
            _campaignManagerDecorator = null;
            _loggerClient.VerifyAll();
            _loggerClient = null;
            _campaignManagerServiceProvider = null;
        }

        #region ConstructorTests

        /// <summary>
        /// Campaing Manager Decorator Constructor Test
        /// </summary>
        [TestMethod]
        public void CampaignManagerDecoratorConstructorTest()
        {
            var decorator = new CampaignManagerDecorator(_loggerClient.Object);
        }

        /// <summary>
        /// Campaign Decorator Constructor Test with Null Logger
        /// </summary>
        [TestMethod]
        public void CampaignManagerDecoratorConstructorTestWithNullLogger()
        {
            try
            {
                var decorator = new CampaignManagerDecorator(null);
                Assert.Fail("An Argument Null Exception is expected when the Logger Client is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: loggerClient", exception.Message.Replace(Environment.NewLine, " "));
            }
            
        }
        #endregion

        /// <summary>
        /// Invoke DecoratorLead with null leadEntity
        /// </summary>
        [TestMethod]
        public void CampaignManagerDecoratorTestWithNullLeadEntity()
        {
            try
            {
                var decorator = new CampaignManagerDecorator(_loggerClient.Object);
                decorator.DecorateLead(null, _testCampaignManagerResultList);
                Assert.Fail("An Argument Null Exception is expected when the leadEntity is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: leadEntity", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        /// <summary>
        /// Invoke DecoratorLead with null ResultList
        /// </summary>
        [TestMethod]
        public void CampaignManagerDecoratorTestWithNullResultList()
        {
            var decorator = new CampaignManagerDecorator(_loggerClient.Object);
            decorator.DecorateLead(_testLeadEntity, null);
        }

        /// <summary>
        /// Invoke DecoratorLead with elements in ResultList
        /// </summary>
        [TestMethod]
        public void CampaignManagerDecoratorTestWithDataInResultList()
        {
            var testCampaingCnt = 2;
            var decorator = new CampaignManagerDecorator(_loggerClient.Object);
            _testCampaignManagerResultList.Add(new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignCountKey, testCampaingCnt));
            _testCampaignManagerResultList.Add(new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignsProcessedStatusKey, ResultKeys.ResultKeysStatusEnum.Processed));
            var expectedResultListCount = _testCampaignManagerResultList.Count + 2; // 2 more elements added in Decorator
            decorator.DecorateLead(_testLeadEntity, _testCampaignManagerResultList);

            // Now check that the testLeadEntity ResultCollection was decorated with expected values.
            Assert.IsNotNull(_testLeadEntity.ResultCollection.CampaignManagerCollection);
            Assert.AreEqual(expectedResultListCount, _testLeadEntity.ResultCollection.CampaignManagerCollection.Length);
            var actualCampaignCnt = _testLeadEntity.ResultCollection.CampaignManagerCollection.SingleOrDefault(item => item.Id == ResultKeys.CampaignManagerKeys.CampaignCountKey)?.Value;
            Assert.AreEqual(testCampaingCnt, actualCampaignCnt);
        }

        /// <summary>
        /// Invoke DecoratorLead with elements in ResultList - The ResultsCollection of the testLeadEntity is null
        /// so the Decorate Lead needs to create this list for the leadEntity.
        /// </summary>
        [TestMethod]
        public void CampaignManagerDecoratorTestWithNullResultsCollection()
        {
            // If there was no results collection - the results collection should be created
            _testLeadEntity.ResultCollection = null;
            var testCampaingCnt = 2;
            var decorator = new CampaignManagerDecorator(_loggerClient.Object);
            _testCampaignManagerResultList.Add(new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignCountKey, testCampaingCnt));
            _testCampaignManagerResultList.Add(new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignsProcessedStatusKey, ResultKeys.ResultKeysStatusEnum.Processed));
            var expectedResultListCount = _testCampaignManagerResultList.Count + 2; // 2 more elements added in Decorator
            decorator.DecorateLead(_testLeadEntity, _testCampaignManagerResultList);

            // Now check that the testLeadEntity ResultCollection was decorated with expected values.
            Assert.IsNotNull(_testLeadEntity.ResultCollection.CampaignManagerCollection);
            Assert.AreEqual(expectedResultListCount, _testLeadEntity.ResultCollection.CampaignManagerCollection.Length);
            var actualCampaignCnt = _testLeadEntity.ResultCollection.CampaignManagerCollection.SingleOrDefault(item => item.Id == ResultKeys.CampaignManagerKeys.CampaignCountKey)?.Value;
            Assert.AreEqual(testCampaingCnt, actualCampaignCnt);
        }

        /// <summary>
        /// Invoke DecoratorLead.  There are already elements in the _testLeadEntity -  ResultCollection.CampaignManagerCollection.
        /// New Values can be added during the decoration but existing should not be lost.
        /// </summary>
        [TestMethod]
        public void CampaignManagerDecoratorTestWithExistingCampaignManagerResultsCollection()
        {
            // Add 1 element to the CampaignManagerCollection
            var expectedCount = 1;
            _testLeadEntity.ResultCollection.CampaignManagerCollection = new IResult[]
            {
                new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignsProcessedStatusKey,
                    ResultKeys.ResultKeysStatusEnum.Processed)
            };
            Assert.AreEqual(expectedCount, _testLeadEntity.ResultCollection.CampaignManagerCollection.Length);

            // Now the rest should work the same and the element above should not be overwritten
            var testCampaingCnt = 2;
            var decorator = new CampaignManagerDecorator(_loggerClient.Object);
            _testCampaignManagerResultList.Add(new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignCountKey, testCampaingCnt));
            var expectedResultListCount = _testLeadEntity.ResultCollection.CampaignManagerCollection.Length + _testCampaignManagerResultList.Count + 2; // 2 more elements added in Decorator
            decorator.DecorateLead(_testLeadEntity, _testCampaignManagerResultList);

            // Now check that the testLeadEntity ResultCollection was decorated with expected values.
            Assert.IsNotNull(_testLeadEntity.ResultCollection.CampaignManagerCollection);
            Assert.AreEqual(expectedResultListCount, _testLeadEntity.ResultCollection.CampaignManagerCollection.Length);
            var actualCampaignCnt = _testLeadEntity.ResultCollection.CampaignManagerCollection
                .SingleOrDefault(item => item.Id == ResultKeys.CampaignManagerKeys.CampaignCountKey)?.Value;
            Assert.AreEqual(testCampaingCnt, actualCampaignCnt);

            // Check that the initialized value are still in the _tetLeadEntity.ResultCollection.CampaignManagerCollection
            var expectedValue = _testLeadEntity.ResultCollection.CampaignManagerCollection.SingleOrDefault(item =>
                item.Id == ResultKeys.CampaignManagerKeys.SubscriberStatusKey)?.Value;
            Assert.AreEqual(expectedValue.ToString(), ResultKeys.ResultKeysStatusEnum.Processed.ToString());
        }

    }
}
