namespace LMS.Publisher.UnitTests
{
    using System;
    using System.IO;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using LMS.Publisher.Interface;
    using LMS.Modules.LeadEntity.Interface;

    [TestClass]
    public class PublisherUnitTests
    {
        /// <summary> 
        /// The container 
        /// </summary> 
        private static IServiceProvider _publisherServiceProvider;
        //private static IServiceCollection _publisherService;

        private Mock<IPublisher> _publisher;
        private Mock<ILeadEntity> _leadEntity;

        [TestInitialize]
        public void Initialize()
        {
            _publisher = new Mock<IPublisher>();
            _leadEntity = new Mock<ILeadEntity>();
            _publisherServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(IPublisher), _publisher.Object)
                .BuildServiceProvider();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _publisher.VerifyAll();
            _publisher = null;
            _publisherServiceProvider = null;
        }
        /// <summary>
        /// Test that the PublishLead function call is invoked as expected.
        /// </summary>
        [TestMethod]
        public void TestLeadPublishCall()
        {
            var publisher = _publisherServiceProvider.GetService<IPublisher>();

            // Mock the PublishLead Function and verify that it was called as expected.
            _publisher.Setup(c => c.PublishLead(It.IsAny<ILeadEntity>())).Verifiable();

            // Invoke the Publish function
            publisher.PublishLead(_leadEntity.Object);
        }

    }
}
