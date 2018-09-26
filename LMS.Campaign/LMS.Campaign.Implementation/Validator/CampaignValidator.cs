using System;
using System.Collections.Generic;
using System.Linq;
using Compare.Services.LMS.Controls.Validator.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace Compare.Services.LMS.Campaign.Implementation.Validator
{
    /// <summary>
    /// CampaignValidator class.  This class contains the function ValidLead that
    /// will confirm that the lead has all the initial pre-requisites to process through the
    /// the Campaign.
    /// </summary>
    public class CampaignValidator : IValidator
    {
        readonly ILoggerClient _loggerClient;
        private static string solutionContext = "CampaignValidator";
        private readonly IList<IValidator> _campaignValidators;

        /// <summary>
        /// Constructor for the CampaignCampaignValidator.  
        /// </summary>
        /// <param name="campaignValidators"></param>
        /// <param name="loggerClient"></param>
        public CampaignValidator(IList<IValidator> campaignValidators, ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
            _campaignValidators = campaignValidators;
        }
        /// <summary>
        /// Loop through the Validators and check if the lead is valid.  This function
        /// loops through all the validators, even if it finds one that is not valide and
        /// collects all the reasons why a lead does not qualify for the campaign.
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            string processContext = "ValidLead";
            bool allValid = true;
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Validating the Lead", ProcessContext = processContext, SolutionContext = solutionContext });
            try
            {
                // Validate the lead using the collection of validators
                // Process all validators before returning.
                foreach (var validator in _campaignValidators)
                {
                    var valid = validator.ValidLead(leadEntity);
                    if (!valid)
                    {
                        allValid = false;
                    }
                }
                if (!allValid)
                {
                    var errorStr = String.Join(",", leadEntity.ErrorList.ToArray());
                    _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = $"Validation failed. {errorStr}", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information});
                    return false;
                }
            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject{OperationContext = "Exception in Validation of Lead in Campaign Validator.",ProcessContext = processContext,SolutionContext = solutionContext,Exception = ex,ErrorContext = ex.Message,EventType = LoggerClientEventType.LoggerClientEventTypes.Error});
                return false;
            }

            return true;
        }
    }
}
