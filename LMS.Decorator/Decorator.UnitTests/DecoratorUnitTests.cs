namespace LMS.Decorator.UnitTests
{
    using System;
    using System.IO;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using LMS.LeadEntity.Interface;
    using LMS.Decorator.Interface;


    [TestClass]
    public class DecoratorUnitTests
    {
        /// <summary> 
        /// The container 
        /// </summary> 
        private static IServiceProvider _decoratorServiceProvider;
        //private static IServiceCollection _decoratorService;

        private Mock<IDecorator> _decorator;
        private Mock<ILeadEntity> _leadEntity;

        [TestInitialize]
        public void Initialize()
        {
            _decorator = new Mock<IDecorator>();
            _leadEntity = new Mock<ILeadEntity>();
            _decoratorServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(IDecorator), _decorator.Object)
                .BuildServiceProvider();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _decorator.VerifyAll();
            _decorator = null;
            _decoratorServiceProvider = null;
        }
        /// <summary>
        /// Test that the DecorateLead function call is invoked as expected.
        /// </summary>
        [TestMethod]
        public void TestLeadDecorateCall()
        {
            var decorator = _decoratorServiceProvider.GetService<IDecorator>();

            // Mock the PublishLead Function and verify that it was called as expected.
            _decorator.Setup(c => c.DecorateLead(It.IsAny<ILeadEntity>())).Verifiable();

            // Invoke the Publish function
            decorator.DecorateLead(_leadEntity.Object);
        }

    }
}
