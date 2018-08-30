
namespace LMS.LeadDecorator.Implementation
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LMS.Modules.LeadEntity.Interface;
    using LMS.Decorator.Interface;
    using LMS.Modules.LeadEntity.Components;
    using LMS.Modules.LeadEntity.Interface.Constants;
    using LMS.LoggerClient.Interface;


    public class LeadDecorator : IDecorator
    {

        private static ILoggerClient _loggerClient;
        private static string solutionContext = "LeadDecorator";

        public LeadDecorator(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));            
        }

        
        public void DecorateLead(ILeadEntity leadEntity, List<IResult> leadCollectorResultList)
        {
            string processContext = "DecorateLead";

            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Decorating the Lead", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });

            // Create LeadCollector Results
            if (leadEntity.ResultCollection == null)
                leadEntity.ResultCollection = new DefaultResultCollection();

            // If there has been LeadCollectorCollection Decoration
            if (leadEntity.ResultCollection.LeadCollectorCollection != null)
                leadCollectorResultList.AddRange(leadEntity.ResultCollection.LeadCollectorCollection.ToList());

            // Add Decorator Processed
            leadCollectorResultList.Add(new DefaultResult(ResultKeys.DecoratorStatusKey , ResultKeys.ResultKeysStatusEnum.Processed.ToString()) );

            // Assign all Lead Collector Results that were Recorded
            leadEntity.ResultCollection.LeadCollectorCollection = leadCollectorResultList.ToArray();
        }
    }
}
