namespace LMS.CampaignManager.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using CampaignManager.Implementation;
    using LMS.Campaign.Interface;
    using LMS.LoggerClient.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class CampaignManagerTests
    {

        private static IServiceProvider _campaignManagerServiceProvider;
        private Mock<List<ICampaign>> _campaignArray;
        private Mock<ILoggerClient> _loggerClient;
        private static readonly string[] EmptyResultArray = new string[] { };

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Campaigns and Logger Client
            _campaignArray = new Mock<List<ICampaign>> ();
            _loggerClient = new Mock<ILoggerClient>();

            // Create Service Providers 
            _campaignManagerServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(List<ICampaign>), _campaignArray)
                .AddSingleton(typeof(ILoggerClient), _loggerClient.Object)
                .BuildServiceProvider();
        }
        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _campaignArray.VerifyAll();
            _campaignArray = null;
            _loggerClient.VerifyAll();
            _loggerClient = null;
            _campaignManagerServiceProvider = null;
        }

        /// <summary>
        /// Campaing Constructor Test
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorTest()
        {
            var campaignManager = new CampaignManager(_campaignArray.Object.ToArray(), _loggerClient.Object);
        }

 
        /// <summary>
        /// Campaing Constructor Test with a Null Campaign List.
        /// </summary>
        [TestMethod]
        public void CampaignManagerConstructorNullCampaignListTest()
        {

            try
            {
                var campaignManager = new CampaignManager(null, _loggerClient.Object);
                Assert.Fail("An Argument Null Exception is expected when the campaignArray is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: campaignArray", exception.Message.Replace(Environment.NewLine, " "));
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
                var campaignManager = new CampaignManager(_campaignArray.Object.ToArray(), null);
                Assert.Fail("An Argument Null Exception is expected when the LoggerClient is null");
            }
            catch (Exception exception)
            {
                Assert.AreEqual(typeof(ArgumentNullException), exception.GetType());
                Assert.AreEqual("Value cannot be null. Parameter name: loggerClient", exception.Message.Replace(Environment.NewLine, " "));
            }
        }

        /// <summary>
        /// Campaing Constructor Test with a No Campaigns.
        /// </summary>
        [TestMethod]
        public void CampaignManagerProcessEmptyCampaignTest()
        {
            // No Campaigns set up, so result list should be empty
            var campaignManager = new CampaignManager(_campaignArray.Object.ToArray(), _loggerClient.Object);
            var resultList = campaignManager.ProcessCampaigns("This is the lead");
            Assert.AreEqual(resultList.Length, 0);
        }

        /// <summary>
        /// Execute the Campaign Manager with multiple campaigns.
        /// </summary>
        [TestMethod]
        public void CampaignManagerProcessMultipleCampaignTest()
        {
            const string testMessage = "Hello from Milo! :-)";
            const string responseFormat = "Campaign {0} complete.";

            const int campaignCount = 3;
            var campaigns = new ICampaign[campaignCount];

            for (var campaignIndex = 0; campaignIndex < campaignCount; campaignIndex++)
            {
                var id = campaignIndex;
                var mock = new Mock<ICampaign>();
                mock.Setup(campaign => campaign.ProcessLead((It.IsIn(testMessage))))
                    .Returns(string.Format(responseFormat, id));

                campaigns[campaignIndex] = mock.Object;
            }

            var campaignManager = new CampaignManager(campaigns, new Mock<ILoggerClient>().Object);

            var results = campaignManager.ProcessCampaigns(testMessage);

            // Check that results were created
            Assert.IsNotNull(results);

            // Check for expected result message
            var campaignIx = 2;
            Assert.AreEqual(results[campaignIx], string.Format(responseFormat, campaignIx));

            // Check for expected number of results
            Assert.AreEqual(campaignCount, results.Length);
        }

     
    }
}