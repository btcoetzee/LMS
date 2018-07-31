namespace LMS.Campaign.BuyClick.Rule
{
    using System;
    using System.Linq;
    using LMS.Rule.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LoggerClient.Interface;
    public class BuyClickRule : IRule
    {
        readonly ILoggerClient _loggerClient;
        private static string solutionContext = "BuyClickRule";
        // Mock an array of ActivityGuids that could have come from the database - for these activity guids, emails have been sent out in 
        // the last 3 days - If the incomiing leadEntity has a Guid in this array the Filter should return false;
        readonly bool _mockedExcludePriorInsurance = false;

        public BuyClickRule(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        public bool ValidateForRule(ILeadEntityImmutable leadEntity)
        {
            string processContext = "ValidateForRule";

            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Validating the Lead", ProcessContext = processContext, SolutionContext = solutionContext });
            var errorStr = string.Empty;
            try
            {
                var priorInsuranceValue = leadEntity.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PriorInsuranceKey)?.Value;
              
                if ((priorInsuranceValue == null) || (!bool.TryParse(priorInsuranceValue.ToString(), out bool priorInsurance)))
                {
                    errorStr += "Prior Insurance not set In Properties of LeadEntityObject\n";
                }
                else
                {
                    if (priorInsurance == _mockedExcludePriorInsurance)
                    {
                        errorStr += $"Lead failed Rules Processing - Prior Insurance set to {_mockedExcludePriorInsurance}.\n";
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = "\nPrior Insurance", ProcessContext = processContext, SolutionContext = solutionContext, Exception = ex, ErrorContext = ex.Message });
                return false;
            }

            if (errorStr != String.Empty)
            {
                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = errorStr, ProcessContext = processContext, SolutionContext = solutionContext });
                return false;
            }
            return true;
        }
    }
}
