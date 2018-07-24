namespace LMS.LeadDecorator.UnitTests
{
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;

    [TestClass]
    public class LeadDecoratorUnitTests
    {
        private static IServiceProvider _serviceProvider;
        private Mock<IDecorator> _decorator;
        private Mock<ILeadEntity> _leadEntity;

        [TestInitialize]
        public void Initialize()
        {
            _decorator = new Mock<IDecorator>();
            _leadEntity = new Mock<ILeadEntity>();
            _serviceProvider = new ServiceCollection().AddSingleton(typeof(IDecorator), _decorator.Object).BuildServiceProvider();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _leadEntity.VerifyAll();
            _leadEntity = null;
            _decorator.VerifyAll();
            _decorator = null;
        }

        //[TestMethod]
        //public void TestMethod1()
        //{
        //    var decorator = _serviceProvider.GetService<IDecorator>();
        //    bool expectedValue = true;

        //    _decorator.Setup(v => v.DecorateLead(It.IsAny<ILeadEntity>())).Equals(expectedValue);

        //    //var actualValue = decorator.DecorateLead(_leadEntity.Object);
            

        //}
    }
}
