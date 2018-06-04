namespace Decorator.UnitTests
{

    using Decorator.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.IO;
    using System.Text;

    [TestClass]
    public class DecoratorTests
    {
        private static System.IServiceProvider _decoratorServiceProvider;
        private Mock<IDecorator> _decorator;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            // Mock the Decorator
 
            _decorator = new Mock<IDecorator>();

            // Create Service Providers 
            _decoratorServiceProvider = new ServiceCollection()
                .AddSingleton(typeof(IDecorator), _decorator.Object)
                .BuildServiceProvider();
        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
             _decorator.VerifyAll();
            _decorator = null;
            _decoratorServiceProvider = null;
        }
        [TestMethod]
        public void TestDecoratorApplied()
        {
            // the decorator
            var decorator = _decoratorServiceProvider.GetService<IDecorator>();

            // Set up the lead msg and decorated lead
            const string leadMessage = "This is the lead message.";
            const string decorationMessage = " Decoration.";
            const string decoratedLeadMessage = leadMessage + decorationMessage;
            byte[] leadMessageByteArray = Encoding.UTF8.GetBytes(leadMessage);
            byte[] decoratedLeadMessageByteArray = Encoding.UTF8.GetBytes(decoratedLeadMessage);
            var lead = new MemoryStream(leadMessageByteArray);
            var decoratedLead = new MemoryStream();
           

            // Mock the decorator lead function to decorate the lead - The text is copied to the lead
            _decorator.Setup(c => c.DecorateLead(It.IsAny<Stream>())).Callback(() => {
                decoratedLead = new MemoryStream(decoratedLeadMessageByteArray);
            });

            // Send a valid stream parameter and check that lead is decorated
            decorator.DecorateLead(lead);

            // Read the stream returned
            StreamReader reader = new StreamReader(decoratedLead);
            string decoratedLeadString = reader.ReadToEnd();

            // Verify that the lead carries the lead and the decoration
            Assert.IsTrue(decoratedLeadString.Contains(leadMessage));
            Assert.IsTrue(decoratedLeadString.Contains(decorationMessage));
        }
    }
}
