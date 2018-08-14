namespace LMS.CampaignManager.Validator.Implementation
{
    using System;
    using LMS.LoggerClient.Interface;
    using LMS.CampaignManager.Validator.Interface;
    using LMS.LeadEntity.Interface;

    public class CampaignManagerValidator:ICampaignManagerValidator
    {
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "CampaignManagerValidator";
        public CampaignManagerValidator(ILoggerClient loggerClient)
        {
           _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        public bool ValidLead(ILeadEntity lead)
        {
            var processContext = "ValidLead";
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Validating the lead.", ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
            return true;

        }
    }
}
