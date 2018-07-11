namespace LMS.LeadValidator.Implementation
{
    using System.Linq;
    using System;
    using LMS.Validator.Interface;
    using LMS.LoggerClient.Interface;
    using LMS.Decorator.Interface;

    public class LeadValidator : IValidator
    {

        public DefaultLoggerClientObject defaultLoggerClientObject;

        ILoggerClient _loggerClient;
        private static IDecorator _notificationChannelPublisher;
        private static string solutionContext = "LeadValidator";

        public LeadValidator(ILoggerClient loggerClient)
        {          
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

        }

        //public bool ValidLead(LMS.LeadEntity.Interface.ILeadEntity lead)
        public bool ValidLead(LMS.LeadEntity.Interface.ILeadEntity lead)
        {

            string processContext = "ValidLead";

            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Validating the Lead",
                ProcessContext = processContext,
                SolutionContext = solutionContext
            });

            var errorStr = string.Empty;

            if (lead.Context.Length < 0)
                    errorStr += "Context Length is Zero \n";
              

            

            if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey) == null)
                errorStr += "ActivityGuid Invalid \n";
    
            if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey) == null)
                errorStr += "IdentityGuid Invalid \n";

            if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey) == null)
                errorStr += "SessionGuid Invalid \n";

            if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey) == null)
                errorStr += "QuotedProductKey Invalid \n";

            // Check the value here.
            Guid activityGuid;
            Guid identityGuid;
            Guid sessionGuid;
            int quotedProduct;


            if (!Guid.TryParse(lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey)
                .Select(i => i.Value).ToString(), out activityGuid))
                errorStr += "ActivityGuid can't be passed \n";

            if (!Guid.TryParse(lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey)
                .Select(i => i.Value).ToString(), out identityGuid))
                errorStr += "IdentityGuid can't be passed \n";

            if (!Guid.TryParse(lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey)
                .Select(i => i.Value).ToString(), out sessionGuid))
                errorStr += "SessionGuid can't be passed \n";

            if (!Int32.TryParse(lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey)
                .Select(i => i.Value).ToString(), out quotedProduct))
                errorStr += "QuotedProductKey can't be passed \n";

            // try tryParse on guid to see if valid guid.

            if (errorStr != String.Empty)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject
                {
                    OperationContext = "Had an error occurred...",
                    ProcessContext = "LMS.ConsoleApp.Exe",
                    SolutionContext = string.Empty,
                    ErrorContext = errorStr

                });


                return false;
            }

            return true;

        }
    
    }
}
