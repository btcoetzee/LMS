using System.Collections.Generic;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Resolver.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace LMS.LeadDispatcher.Interface
{
    public interface ILeadDispatcherConfig
    {
        // Return the subscriber for the Lead Dispatcher
        ISubscriber Subscriber { get; set; }

        //  Return the resolver for the Lead Dispatcher - 
        // This is the wrapper Resolver that executes the required resolvers 
        IResolver Resolver { get; set; }

        //  Return the list of resolvers for the Lead Dispatcher
        IList<IResolver> ResolverCollection { get; set; }

        //Return the decorator for the Lead Dispatcher
        IDecorator Decorator { get; set; }

        // Return the publisher for the Lead Dispatcher
        IPublisher Publisher { get; set; }

        // Return the subscriber for the Lead Dispatcher
        IPersistor Persistor { get; set; }

        // Return the logger client for the Lead Dispatcher
        ILoggerClient LoggerClient { get; set; }
    }
}
