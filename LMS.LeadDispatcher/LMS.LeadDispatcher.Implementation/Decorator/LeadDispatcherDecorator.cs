using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Modules.LeadEntity.Components;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace LMS.LeadDispatcher.Implementation.Decorator
{
    public class LeadDispatcherDecorator : IDecorator
    {
        private static ILoggerClient _loggerClient;
        private static string solutionContext = "LeadDispatcherDecorator";
        public LeadDispatcherDecorator(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }
        /// <summary>
        /// Decorate the Lead by adding the Lead Dispatcher Results List to the Results Collection
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <param name="leadDispatcherResultList"></param>
        public void DecorateLead(ILeadEntity leadEntity, List<ILeadEntityObjectContainer> leadDispatcherResultList)
        {
            if (leadEntity == null) throw new ArgumentNullException(nameof(leadEntity));
            if (leadDispatcherResultList?.Any() == true)
            {
                string processContext = "DecorateLead";

                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Decorating the Lead", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

                // The Results Collection was not created yet - shouldn't happen
                if (leadEntity.ResultCollection == null)
                    leadEntity.ResultCollection = new DefaultResultCollection();

                // Add the decorator result time & status
                leadDispatcherResultList.Add(new DefaultLeadEntityObjectContainer(ResultKeys.DiagnosticKeys.TimeStampEndKey, DateTime.Now));
                leadDispatcherResultList.Add(new DefaultLeadEntityObjectContainer(ResultKeys.DecoratorStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

                // If there has been previous LeadDispatcher results - add them to the list and then assign as array
                if (leadEntity.ResultCollection.LeadDispatcherCollection != null)
                    leadDispatcherResultList.AddRange(leadEntity.ResultCollection.LeadDispatcherCollection.ToList());

                // Assign all Lead Dispatcher Results that were Recorded
                leadEntity.ResultCollection.LeadDispatcherCollection = leadDispatcherResultList.ToArray();

            }
        }
    }
}
