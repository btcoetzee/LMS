﻿namespace LMS.Campaign.BuyClick.Validator
{
    using System;
    using System.Linq;
    using LMS.CampaignValidator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LoggerClient.Interface;
    public class BuyClickValidator : ICampaignValidator
    {
        readonly ILoggerClient _loggerClient;
        private static string solutionContext = "BuyClickValidator";

        public BuyClickValidator(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

        }
        public bool ValidLead(ILeadEntityImmutable leadEntity)
        {
            string processContext = "ValidLead";
            int pni_Age;

            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Validating the Lead",ProcessContext = processContext,SolutionContext = solutionContext});
            var errorStr = string.Empty;
            try
            {
                var value = leadEntity.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber)?.Value;
                if (value != null && ((leadEntity.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber) == null) ||
                    (String.IsNullOrEmpty(value.ToString()))))
                    errorStr += "PhoneNumber Invalid or Not In Properties of LeadEntityObject\n";

                var age = leadEntity.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PNI_Age)?.Value;
                if (age != null && ((leadEntity.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PNI_Age) == null) ||
                    (!int.TryParse(age.ToString(), out pni_Age))))
                    errorStr += "PNI_Age Invalid or Not In Properties of LeadEntityObject\n";
            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject{OperationContext = "\nValidating Phone Number and PNI Age",ProcessContext = processContext,SolutionContext = solutionContext,Exception = ex,ErrorContext = ex.Message});
                return false;
            }

            if (errorStr != String.Empty)
            {
                _loggerClient.Log(new DefaultLoggerClientObject{OperationContext =errorStr,ProcessContext = processContext,SolutionContext = solutionContext});
                return false;
            }
            return true;
        }
    }
}
