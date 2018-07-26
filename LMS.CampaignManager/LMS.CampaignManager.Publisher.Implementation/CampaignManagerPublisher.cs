namespace LMS.CampaignManager.Publisher.Implementation
{
    using System;
    using Newtonsoft.Json;
    using LMS.CampaignManager.Publisher.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LoggerClient.Interface;
    public class CampaignManagerPublisher : ICampaignManagerPublisher
    {
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "CampaignManagerPublisher";

        public CampaignManagerPublisher(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        public void PublishLead(ILeadEntity leadEntity)
        {
            var processContext = "PublishLead";
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = JsonConvert.SerializeObject(leadEntity, Newtonsoft.Json.Formatting.Indented), ProcessContext = processContext, SolutionContext = SolutionContext });

        }
    }
}
