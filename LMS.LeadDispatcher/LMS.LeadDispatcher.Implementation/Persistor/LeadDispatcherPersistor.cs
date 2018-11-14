using System;
using System.Collections.Generic;
using System.Text;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;
using Newtonsoft.Json;

namespace LMS.LeadDispatcher.Implementation.Persistor
{
    public class LeadDispatcherPersistor : IPersistor
    {
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "LeadDispatcherPersistor";

        public LeadDispatcherPersistor(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        /// <summary>
        /// Persist Lead into DB TBD.
        /// </summary>
        /// <param name="leadEntity"></param>
        public void PersistLead(ILeadEntity leadEntity)
        {
            // TODO - Implementation
            var processContext = "PersistLead..................... TBD";
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = JsonConvert.SerializeObject(leadEntity, Newtonsoft.Json.Formatting.Indented), ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

        }
    }
}
