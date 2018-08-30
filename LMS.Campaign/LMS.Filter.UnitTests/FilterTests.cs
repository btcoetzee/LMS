namespace LMS.Filter.UnitTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using LMS.Modules.LeadEntity.Interface;
    using LMS.Filter.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class FilterTests
    {
        private static System.IServiceProvider _filterServiceProvider;
        private Mock<IFilter> _filter;
        private Mock<ILeadEntityImmutable> _leadEntity;


        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Filter and Validator
            _filter = new Mock<IFilter>();
            _leadEntity = new Mock<ILeadEntityImmutable>();

            // Create Service Providers for Filter and 
            _filterServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(IFilter), _filter.Object)
                .BuildServiceProvider();
        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _filter.VerifyAll();
            _filter = null;
            _filterServiceProvider = null;
        }

        /// <summary>
        /// Tests the ProcessLead in Filter Interface.
        /// </summary>
        [TestMethod]
        public void TestIfLeadProcessedInFilterCall()
        {
            var filter = _filterServiceProvider.GetService<IFilter>();
            bool extpectedValue = true;

            // Mock the ProcessLead function to update the message
             _filter.Setup(c => c.ClearedFilter((It.IsAny<ILeadEntityImmutable>()))).Returns(true);

            var actualValue = filter.ClearedFilter(_leadEntity.Object);
            Assert.AreEqual(extpectedValue, actualValue);
        }
    }
}
