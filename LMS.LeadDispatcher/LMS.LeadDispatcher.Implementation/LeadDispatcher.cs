using LMS.LeadDispatcher.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Compare.Services.LMS.Modules.LeadEntity.Components;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;
using Compare.Services.LMS.Modules.LoggerClient.Interface;
using LMS.LeadDispatcher.Implementation.Config;

namespace LMS.LeadDispatcher.Implementation
{
    public class LeadDispatcher : ILeadDispatcher
    {
        private int _leadDispatcherId;
        private readonly ILeadDispatcherConfig _leadDispatcherConfig;
        private List<ILeadEntityObjectContainer> _leadDispatcherResultList;
        private static string solutionContext = "LeadDispatcher";

        public string LeadDispatcherName { get; set; }

        public int LeadDispatcherId
        {
            get { return _leadDispatcherId; }
            set => _leadDispatcherId = value;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leadDispatcherId"></param>
        /// <param name="leadDispatcherName"></param>
        /// <param name="leadDispatcherConfig"></param>
        public LeadDispatcher(int leadDispatcherId, string leadDispatcherName, ILeadDispatcherConfig leadDispatcherConfig)
        {
            LeadDispatcherId = leadDispatcherId > 0
                ? leadDispatcherId
                : throw new ArgumentException($"Error: {solutionContext}: leadDispatcherId = {leadDispatcherId}");
            LeadDispatcherName = leadDispatcherName ?? throw new ArgumentNullException(nameof(leadDispatcherName));
            _leadDispatcherConfig = leadDispatcherConfig ?? throw new ArgumentNullException(nameof(leadDispatcherConfig));

            // When the subscriber receives a lead, invoke the DispatchLead function
            _leadDispatcherConfig.Subscriber.SetupAddOnReceiveActionToChannel(DispatchLead);
        }

        /// <summary>
        /// Dispatch Lead consists of the the following:
        /// Process the lead through the List of Resolvers to see if the Lead Qualifies to continue
        /// Resolvers.  
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns>List&lt;IResult&gt;</returns>
        public void DispatchLead (ILeadEntity leadEntity)
        {

            if (leadEntity == null) throw new ArgumentNullException(nameof(leadEntity));
            string processContext = "DispatchLead";
            _leadDispatcherConfig.LoggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Processing the Lead in {LeadDispatcherName}", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
            try
            {
                // Create LeadDispatcher Result List and insert LeadDispatcher Details
                _leadDispatcherResultList = new List<ILeadEntityObjectContainer> { new DefaultLeadEntityObjectContainer(ResultKeys.DiagnosticKeys.TimeStampStartKey, DateTime.Now) };
                _leadDispatcherResultList.Add(new DefaultLeadEntityObjectContainer(ResultKeys.LeadDispatcherKeys.LeadDispatcherIdKey, LeadDispatcherId));
                _leadDispatcherResultList.Add(new DefaultLeadEntityObjectContainer(ResultKeys.LeadDispatcherKeys.LeadDispatcherNameKey, LeadDispatcherName));

                // Verify that there are Resolvers to be Executed.
                if (_leadDispatcherConfig.ResolverCollection?.Any() != true)
                {
                    _leadDispatcherConfig.LoggerClient.Log(new DefaultLoggerClientObject { OperationContext = "There are no Resolvers to be Executed.", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                    _leadDispatcherResultList.Add(new DefaultLeadEntityObjectContainer(ResultKeys.ResolverCountKey, 0));
                    _leadDispatcherConfig.Decorator.DecorateLead(leadEntity, _leadDispatcherResultList);
                    return;
                }

                //Resolve the Lead
                if (_leadDispatcherConfig.Resolver.ResolveLead(leadEntity).Equals(false))
                {
                    _leadDispatcherResultList.Add(new DefaultLeadEntityObjectContainer(ResultKeys.ResolverStatusKey,
                        ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                    _leadDispatcherConfig.LoggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Lead was not resolved in LeadDispatcher.", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                    _leadDispatcherConfig.Decorator.DecorateLead(leadEntity, _leadDispatcherResultList);
                    return;
                }

                // Decorate the Lead
                _leadDispatcherConfig.Decorator.DecorateLead(leadEntity, _leadDispatcherResultList);
                _leadDispatcherConfig.LoggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Decorated the Lead in {LeadDispatcherName}", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

                // Persist the Lead
                _leadDispatcherConfig.Persistor.PersistLead(leadEntity);
                _leadDispatcherConfig.LoggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Persisted the Lead in {LeadDispatcherName}", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                // Publish the Lead
                _leadDispatcherConfig.Publisher.PublishLead(leadEntity);
                _leadDispatcherConfig.LoggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Published the Lead in {LeadDispatcherName}", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

            }
            catch (Exception ex)
            {
                _leadDispatcherConfig.LoggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = "Exception raised during LeadDispatcher Processing.", ProcessContext = processContext, SolutionContext = solutionContext, Exception = ex, ErrorContext = ex.Message, EventType = LoggerClientEventType.LoggerClientEventTypes.Error });
                throw;
            }

        }
    }
}
