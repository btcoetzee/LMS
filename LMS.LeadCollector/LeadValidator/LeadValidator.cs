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
        private static IValidator _notificationChannelPublisher;
        private static string solutionContext = "LeadValidator";

        public LeadValidator(ILoggerClient loggerClient)
        {          
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

        }

        //public bool ValidLead(LMS.LeadEntity.Interface.ILeadEntity lead)
        public bool ValidLead(LMS.LeadEntity.Interface.ILeadEntity lead)
        {

            Guid activityGuid;
            Guid identityGuid;
            Guid sessionGuid;
            int quotedProduct;

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

            // Check the value here.
            // try tryParse on guid to see if valid guid.
            try
            {

                if (((lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey) == null) ||
                    (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey).Value == null) ||
               (!Guid.TryParse((lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey).Value).ToString(), out activityGuid))))
                errorStr += "ActivityGuid Invalid or Not In Context \n";

                if (((lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey) == null) || 
                    (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey).Value == null) ||
                    (!Guid.TryParse((lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey).Value).ToString(), out identityGuid))))
                    errorStr += "IdentityGuid Invalid or Not In Context \n";

                if (((lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey) == null) ||
                     (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey).Value == null) ||
                    (!Guid.TryParse((lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey).Value).ToString(), out sessionGuid))))
                    errorStr += "SessionGuid Invalid or Not In Context \n";

                if (((lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey) == null) ||
                    (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey).Value == null) ||
                    (!Int32.TryParse((lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey).Value).ToString(), out quotedProduct)))) 
                    errorStr += "QuotedProductKey Invalid or Not In Context \n";


            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject
                {
                    OperationContext = "\nAn exception occured...",
                    ProcessContext = "LMS.ConsoleApp.Exe",
                    SolutionContext = string.Empty,
                    Exception = ex,
                    ErrorContext = ex.Message

                });
                return false;
            }
           

            if (errorStr != String.Empty)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject
                {
                    OperationContext = "\nHad an error occurred...",
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
