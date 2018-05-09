namespace Campaign.UnitTests.Campaign
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices.ComTypes;
    using System.Text;
    using global::Campaign.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class CampaignTests
    {
        /// <summary> 
        /// The container 
        /// </summary> 
        private static IServiceProvider _campaignServiceProvider;
        //private static IServiceCollection _campaignService;

        private Mock<ICampaign> _campaign;

        [TestInitialize]
        public void Initialize()
        {
            _campaign = new Mock<ICampaign>();
            

            _campaignServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(ICampaign), _campaign.Object)
                .BuildServiceProvider(); 
        }

        [TestCleanup]
        public void Cleanup()
        {
            _campaign.VerifyAll();
            _campaign = null;
            _campaignServiceProvider = null;
        }
        /// <summary>
        /// Tests the campaign constructor.
        /// </summary>
        [TestMethod]
        public void TestIfLeadProcessedInCampaignCall()
        {
            var campaign = _campaignServiceProvider.GetService<ICampaign>();
            const string message = "Lead was processed";
            string outputMessage = "";

            //void callbackAction()
            //{
            //    outputMessage = message;
            //}

            _campaign.Setup(c => c.ProcessLead(It.IsAny<Stream>())).Callback(() => {
                outputMessage = message;
            });
            campaign.ProcessLead(new MemoryStream());
            Assert.AreEqual(message, outputMessage);

        }
    }
}
