namespace LMS.LeadDecorator.UnitTests
{
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using LMS.LeadDecorator.Implementation;
    using LMS.LoggerClient.Interface;
    using LMS.LeadEntity.Components;
    using System.Collections.Generic;

    [TestClass]
    public class LeadDecoratorUnitTests
    {
        private static IServiceProvider _serviceProvider;
        private Mock<IDecorator> _decorator;
        private Mock<ILeadEntity> _leadEntity;
        private Mock<ILoggerClient> _loggingClient;
        private ILeadEntity _testLeadEntity;

        [TestInitialize]
        public void Initialize()
        {
            _decorator = new Mock<IDecorator>();
            _leadEntity = new Mock<ILeadEntity>();
            _loggingClient = new Mock<ILoggerClient>();
            _serviceProvider = new ServiceCollection().AddSingleton(typeof(IDecorator), _decorator.Object).AddSingleton(typeof(ILoggerClient), _loggingClient.Object).BuildServiceProvider();

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
            public List<string> ErrorList { get; set; }
        }

        //struct TestLeadEntityResultClass : IResult
        //{
        //    public TestLeadEntityResultClass(string id, object value)
        //    {
        //        Id = id;
        //        Value = value;
        //    }

        //    public string Id { get; private set; }

        //    public object Value { get; private set; }
        //}

        void CreateLeadEntity()
        {
            _testLeadEntity = new TestLeadEntityClass()
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

        [TestCleanup]
        public void Cleanup()
        {
            _leadEntity.VerifyAll();
            _leadEntity = null;
            _decorator.VerifyAll();
            _decorator = null;
            _loggingClient.VerifyAll();
            _loggingClient = null;
        }

        //[TestMethod]
        //public void TestMethod1()
        //{
        //    var decorator = _serviceProvider.GetService<IDecorator>();
        //    bool expectedValue = true;

        //    _decorator.Setup(v => v.DecorateLead(It.IsAny<ILeadEntity>())).Equals(expectedValue);

        //    //var actualValue = decorator.DecorateLead(_leadEntity.Object);


        //}

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AllNullParamLeadDecoratorConstructorTest()
        {
            new Implementation.LeadDecorator(null);
        }

        ///// <summary>
        ///// Mocked the parameter lead Decorator constructor test.
        ///// </summary>
        //[TestMethod]
        //public void ValidLeadWithEmptyContextTest()
        //{
        //    var decorator = new LeadDecorator(_loggingClient.Object);

        //    _testLeadEntity.ResultCollection = null;
        //    bool expectedValue = false;

        //    var isValid = decorator.DecorateLead(_testLeadEntity, null);

        //    Assert.AreEqual(expectedValue, isValid);

        //}

    }
}
