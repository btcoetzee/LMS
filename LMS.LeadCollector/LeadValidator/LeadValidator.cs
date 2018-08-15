namespace LMS.LeadValidator.Implementation
{
    using System.Linq;
    using System;
    using LMS.Validator.Interface;
    using LMS.LoggerClient.Interface;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;

    public class LeadValidator : IValidator
    {

        public DefaultLoggerClientObject DefaultLoggerClientObject;

        readonly ILoggerClient _loggerClient;
        private static string solutionContext = "LeadValidator";

        public LeadValidator(ILoggerClient loggerClient)
        {          
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

        }

        //public bool ValidLead(LMS.LeadEntity.Interface.ILeadEntity lead)
        public bool ValidLead(ILeadEntity lead)
        {          
            string processContext = "ValidLead";

            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Validating the Lead",
                ProcessContext = processContext,
                SolutionContext = solutionContext
            });

            var errorStr = string.Empty;

            if ((lead.Context == null) || (lead.Context.Length < 0))
            {
                errorStr += "Context Length is Zero \n";
            }

            // Check the value here.
            // try tryParse on guid to see if valid guid.
            try
            {
                var activityGuidValue = lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey)?.Value;
                if ((activityGuidValue == null) || (!Guid.TryParse(activityGuidValue.ToString(), out Guid activityGuid)))
                {
                    errorStr += "ActivityGuid Invalid or Not In Context \n";
                }


                var IdentityGuidValue = lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey)?.Value;
                if ((IdentityGuidValue == null) || (!Guid.TryParse(IdentityGuidValue.ToString(), out Guid identityGuid)))
                {
                    errorStr += "IdentityGuid Invalid or Not In Context \n";
                }


                var SessionGuidValue = lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey)?.Value;
                if ((SessionGuidValue == null) || (!Guid.TryParse(SessionGuidValue.ToString(), out Guid sessionGuid)))
                {
                    errorStr += "SessionGuid Invalid or Not In Context \n";
                }


                var quotedProductKeyValue = lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey)?.Value;
                if ((quotedProductKeyValue == null) || (!int.TryParse(quotedProductKeyValue.ToString(), out int quotedProduct)))
                {
                    errorStr += "QuotedProductKey Invalid or Not In Context \n";
                }
                


            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject
                {
                    OperationContext = "Exception in Validation of Lead",ProcessContext = processContext,SolutionContext = solutionContext,Exception = ex,ErrorContext = ex.Message, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information
                });
                return false;
            }
           

            if (errorStr != String.Empty)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject{OperationContext = "Validation failed",ProcessContext = processContext,SolutionContext = solutionContext,ErrorContext = errorStr, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Error});


                return false;
            }

            return true;

        }
    
    }
}
