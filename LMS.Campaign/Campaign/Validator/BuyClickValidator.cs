namespace LMS.Campaign.Implementation.BuyClick.Validator
{
    using LMS.LeadEntity.Interface;
    using LMS.LoggerClient.Interface;
    using LMS.Validator.Interface;
    using System;
    using System.Linq;

    public class BuyClickValidator : IValidator
    {
        ILoggerClient _loggerClient;
        private static string solutionContext = "BuyClickValidator";

        public BuyClickValidator(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

        }
        public bool ValidLead(ILeadEntity lead)
        {
            string processContext = "ValidLead";
            int pni_Age;

            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Validating the Lead",
                ProcessContext = processContext,
                SolutionContext = solutionContext
            });

            var errorStr = string.Empty;

            try
            {
                if (((lead.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber) == null) ||
                    (lead.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber).Value == null) ||
               (String.IsNullOrEmpty((lead.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber).Value).ToString()))))
                    errorStr += "PhoneNumber Invalid or Not In Properties \n";

                if (((lead.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PNI_Age) == null) ||
                    (lead.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PNI_Age).Value == null) ||
               (!int.TryParse((lead.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PNI_Age).Value).ToString(), out pni_Age))))
                    errorStr += "PNI_Age Invalid or Not In Properties \n";
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
