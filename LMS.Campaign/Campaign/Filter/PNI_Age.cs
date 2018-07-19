namespace LMS.Campaign.Implementation.BuyClickCampaign.Filter
{
    using LMS.Filter.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LoggerClient.Interface;
    using System;
    using System.Linq;

    public class PNI_Age : IFilter
    {
        public DefaultLoggerClientObject defaultLoggerClientObject;

        ILoggerClient _loggerClient;
        private static IFilter _notificationChannelPublisher;
        private static string solutionContext = "PNI_AgeFilter";

        public PNI_Age(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        public bool ClearedFilter(ILeadEntity lead)
        {
            int pni_Age;

            string processContext = "ClearedFilter";

            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Checking the PNI Age Filter",
                ProcessContext = processContext,
                SolutionContext = solutionContext
            });

            var errorStr = string.Empty;

            try
            {
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
