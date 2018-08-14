namespace LMS.Campaign.Prospect.Validator
{
    using System;
    using System.Linq;
    using LMS.CampaignValidator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LoggerClient.Interface;
    using LMS.LoggerClientEventTypeControl.Implementation;

    public class ProspectValidator : ICampaignValidator
    {
        readonly ILoggerClient _loggerClient;
        private static string solutionContext = "ProspectValidator";

        public ProspectValidator(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

        }
        public bool ValidLead(ILeadEntityImmutable leadEntity)
        {
            string processContext = "ValidLead";
            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Validating the Lead",ProcessContext = processContext,SolutionContext = solutionContext, EventType = LMS.LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
            var errorStr = string.Empty;
            try
            {
                var phoneNumberValue = leadEntity.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber)?.Value;
                if (phoneNumberValue == null)
                {
                    errorStr += "PhoneNumber Invalid or Not In Properties of LeadEntityObject\n";
                }
            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject{OperationContext = "\nValidating Phone Number",ProcessContext = processContext,SolutionContext = solutionContext,Exception = ex,ErrorContext = ex.Message, EventType = LMS.LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Error });
                return false;
            }

            if (errorStr != String.Empty)
            {
                _loggerClient.Log(new DefaultLoggerClientObject{OperationContext =errorStr,ProcessContext = processContext,SolutionContext = solutionContext, EventType = LMS.LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
                return false;
            }
            return true;
        }
    }
}
