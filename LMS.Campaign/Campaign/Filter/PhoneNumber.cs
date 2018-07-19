namespace LMS.Campaign.Implementation.BuyClickCampaign.Filter
{
    using LMS.Filter.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LoggerClient.Interface;
    using System;
    using System.Linq;

    public class PhoneNumber : IFilter
    {

        public DefaultLoggerClientObject defaultLoggerClientObject;

        ILoggerClient _loggerClient;
        private static IFilter _notificationChannelPublisher;
        private static string solutionContext = "PhoneNumberFilter";

        public PhoneNumber(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        public bool ClearedFilter(ILeadEntity lead)
        {
            int phoneNumber;

            string processContext = "ClearedFilter";

            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Checking the Phone Number Filter",
                ProcessContext = processContext,
                SolutionContext = solutionContext
            });

            var errorStr = string.Empty;

            try
            {
                if (((lead.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber) == null) ||
                    (lead.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber).Value == null) ||
               (!int.TryParse((lead.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber).Value).ToString(), out phoneNumber))))
                    errorStr += "PhoneNumber Invalid or Not In Properties \n";

                
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
