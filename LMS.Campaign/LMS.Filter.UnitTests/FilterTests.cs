using Compare.Services.LMS.Filter.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Compare.Services.LMS.Filter.UnitTests
{
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
             _filter.Setup(c => c.ConstraintMet((It.IsAny<ILeadEntityImmutable>()))).Returns(true);

            var actualValue = filter.ConstraintMet(_leadEntity.Object);
            Assert.AreEqual(extpectedValue, actualValue);
        }
    }
}
