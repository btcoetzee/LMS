
namespace LMS.Campaign.Prospect
{
    using System;
    using System.Collections.Generic;
    using LMS.LeadEntity.Interface;
    using LMS.Campaign.Interface;
    using LMS.CampaignValidator.Interface;
    using LMS.LoggerClient.Interface;
    using LMS.LeadEntity.Components;
    using LMS.LeadEntity.Interface.Constants;

    public class ProspectCampaign:ICampaign
    {
        private readonly ICampaignValidator _prospectClickValidator;
        private readonly ILoggerClient _loggerClient;
        private List<IResult> _campaignResultList;
        private static string solutionContext = "ProspectCampaign";
        const string ThisCampaignName = "Prospective Campaign";
        private const int ThisCampaignPriority = 2;
        private const int ThisCampaignId = 1;

        public ProspectCampaign(ICampaignValidator prospectClickValidator, ILoggerClient loggerClient)
        {
            _prospectClickValidator = prospectClickValidator ?? throw new ArgumentNullException(nameof(prospectClickValidator));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        public string CampaignName => ThisCampaignName;
        public int CampaignPriority => ThisCampaignPriority;

        public int CampaignId => ThisCampaignId;

        public List<IResult> ProcessLead(ILeadEntityImmutable leadEntity)
        {
            if (leadEntity == null) throw new ArgumentNullException(nameof(leadEntity));
            string processContext = "ProcessLead";

            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Processing the Lead in {CampaignName}", ProcessContext = processContext, SolutionContext = solutionContext });
            _campaignResultList = new List<IResult> { new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampStartKey, DateTime.Now) };
            _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.CampaignNameKey, CampaignName));
            _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.CampaignPriorityKey, CampaignPriority));
            _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.CampaignIdKey, CampaignId));
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Validating the Lead for {CampaignName} ", ProcessContext = processContext, SolutionContext = solutionContext });
            if (_prospectClickValidator.ValidLead(leadEntity).Equals(true))
            {
                // Add that Validation passed to ResultsCollection
                _campaignResultList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

                // FILTERS - Add to Constructor also etc.

                // RULES - Add to Constructor also etc.

                // This is where the actaul messagehandler will be retrieved
                _campaignResultList.Add((new DefaultResult(ResultKeys.CampaignKeys.CampaignMessageHandlerKey, $"MessageHandlerFor{CampaignName}")));

                // For now show that the lead processed successfully through the whole campaign and should continue on to the resolver
                _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.LeadSuccessStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

                // Add the end time to the 
                _campaignResultList.Add(new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampEndKey, DateTime.Now));

            }
            else
            {
                // For now show that the lead has not processed successfully through the campaign and should not continue on to the resolver
                _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.LeadSuccessStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Validation Failed", ProcessContext = processContext, SolutionContext = solutionContext });
                _campaignResultList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
            }
            return _campaignResultList;
        }
    }
}
