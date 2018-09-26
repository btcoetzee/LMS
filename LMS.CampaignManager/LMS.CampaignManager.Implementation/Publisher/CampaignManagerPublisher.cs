using System;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;
using Newtonsoft.Json;

namespace Compare.Services.LMS.CampaignManager.Implementation.Publisher
{
    /// <summary>
    /// Class to Publish to next component that should be sending out the notification for the lead
    /// </summary>
    public class CampaignManagerPublisher : IPublisher
    {
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "CampaignManagerPublisher";

        public CampaignManagerPublisher(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        /// <summary>
        /// Publish Lead to ...... POE? or another Resolver....
        /// </summary>
        /// <param name="leadEntity"></param>
        public void PublishLead(ILeadEntity leadEntity)
        {
            var processContext = "PublishLead";
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = JsonConvert.SerializeObject(leadEntity, Newtonsoft.Json.Formatting.Indented), ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

        }
    }
}
