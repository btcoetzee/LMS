using System;
using System.Collections.Generic;
using System.Text;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;
using Newtonsoft.Json;

namespace LMS.LeadDispatcher.Implementation.Publisher
{
    public class LeadDispatcherPublisher : IPublisher
    {
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "LeadDispatcherPublisher";

        public LeadDispatcherPublisher(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        /// <summary>
        /// Publish Lead to ...... POE? 
        /// </summary>
        /// <param name="leadEntity"></param>
        public void PublishLead(ILeadEntity leadEntity)
        {
            var processContext = "PublishLead";
            // TODO - this will publish to a component outside of LMS
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = JsonConvert.SerializeObject(leadEntity, Newtonsoft.Json.Formatting.Indented), ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
        }
    }
}
