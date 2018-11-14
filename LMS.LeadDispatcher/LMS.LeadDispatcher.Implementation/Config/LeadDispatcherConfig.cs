using System;
using System.Collections.Generic;
using System.Text;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Factory.Interface;
using Compare.Services.LMS.Controls.Factory.Interface.Constants;
using Compare.Services.LMS.Controls.Resolver.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;
using LMS.LeadDispatcher.Implementation.Resolver;
using LMS.LeadDispatcher.Interface;

namespace LMS.LeadDispatcher.Implementation.Config
{
    /// <summary>
    /// Set up the configuration for the LeadDispatcher
    /// </summary>
    public class LeadDispatcherConfig:ILeadDispatcherConfig
    {
        private static string solutionContext = "LeadDispatcherConfig";
        private ILoggerClient _loggerClient;
        private ISubscriber _leadDispatcherSubscriber;
        private IList<IResolver> _resolverCollection;
        private IResolver _leadDispatcherResolver;
        private IPublisher _leadDispatcherPublisher;
        private IDecorator _leadDispatcherDecorator;
        private IPersistor _leadDispatcherPersistor;

        /// <summary>
        /// List of resolvers that are required for the Dispatcher 
        /// </summary>
        public IList<IResolver> ResolverCollection
        {
            get { return _resolverCollection; }
            set { _resolverCollection = value; }
        }
        /// <summary>
        /// Wrapper Resolver that executes the list of resolvers 
        /// </summary>
        public IResolver Resolver
        {
            get { return _leadDispatcherResolver; }
            set { _leadDispatcherResolver = value; }
        }
        /// <summary>
        /// Return the Lead Resolver Publisher
        /// </summary>
        public ISubscriber Subscriber
        {
            get { return _leadDispatcherSubscriber; }
            set { _leadDispatcherSubscriber = value; }
        }

        /// <summary>
        /// Return the Lead Resolver Publisher
        /// </summary>
        public IPublisher Publisher
        {
            get { return _leadDispatcherPublisher; }
            set { _leadDispatcherPublisher = value; }
        }

        /// <summary>
        /// Return the Lead Resolver Decorator
        /// </summary>
        public IDecorator Decorator
        {
            get { return _leadDispatcherDecorator; }
            set { _leadDispatcherDecorator = value; }
        }

        /// <summary>
        /// Return the Lead Resolver Persistor
        /// </summary>
        public IPersistor Persistor
        {
            get { return _leadDispatcherPersistor; }
            set { _leadDispatcherPersistor = value; }
        }

        /// <summary>
        /// Return the Lead Resolver Logger Client
        /// </summary>
        public ILoggerClient LoggerClient
        {
            get { return _loggerClient; }
            set { _loggerClient = value; }
        }        /// <summary>
                 /// Return the Lead Resolver Persistor
                 /// </summary>

        public LeadDispatcherConfig(int leadDispatcherId,
                                    ISubscriber leadDispatcherSubscriber,
                                    IResolverFactory resolverFactory,
                                    IPublisher leadDispatcherPublisher,
                                    IDecorator leadDispatcherDecorator,
                                    IPersistor leadDispatcherPersistor,
                                    ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
            var processContext = "LeadDispatcherConfig";
            try
            {
                _leadDispatcherSubscriber = leadDispatcherSubscriber ?? throw new ArgumentNullException(nameof(leadDispatcherSubscriber));
                var resolverFactoryIn = resolverFactory ?? throw new ArgumentNullException(nameof(resolverFactory));
                // Retrieve the applicable Resolvers from the Factory for this LeadDispatcher
                // Create the LeadDispatcher that is the wrapper if it doesn't exist yet.
                _resolverCollection = resolverFactoryIn.GetResolvers(ResolverComponentUsingFactory.LeadDispatcher, leadDispatcherId);
                if (_leadDispatcherResolver == null)
                {
                    _leadDispatcherResolver = new LeadDispatcherResolver(_resolverCollection, _loggerClient);
                }
                _leadDispatcherPublisher = leadDispatcherPublisher ?? throw new ArgumentNullException(nameof(leadDispatcherPublisher));
                _leadDispatcherDecorator = leadDispatcherDecorator ?? throw new ArgumentNullException(nameof(leadDispatcherDecorator));
                _leadDispatcherPersistor = leadDispatcherPersistor ?? throw new ArgumentNullException(nameof(leadDispatcherPersistor));
            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = "Exception raised during assembly of Lead Resolver Configuration.", ProcessContext = processContext, SolutionContext = solutionContext, Exception = ex, ErrorContext = ex.Message, EventType = LoggerClientEventType.LoggerClientEventTypes.Error });
                throw;
            }

        }
    }
}
