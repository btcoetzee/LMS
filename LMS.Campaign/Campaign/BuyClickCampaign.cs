namespace LMS.Campaign.BuyClick
{
    using System;
    using System.Collections.Generic;
    using LMS.Campaign.Interface;
    using LMS.CampaignValidator.Interface;
    using LMS.Filter.Interface;
    using LMS.LeadEntity.Components;
    using LMS.LeadEntity.Interface;
    using LMS.LeadEntity.Interface.Constants;
    using LMS.LoggerClient.Interface;
    using LMS.Rule.Interface;

    public class BuyClickCampaign : ICampaign
    {
        private readonly ICampaignValidator _buyClickValidator;
        private readonly IFilter _buyClickFilter;
        private readonly IRule _buyClickRule;
        private readonly ILoggerClient _loggerClient;
        private List<IResult> _campaignResultList;
        private static string solutionContext = "BuyClickCampaign";
        const string ThisCampaignName = "BuyClick Campaign";
        private const int ThisCampaignPriority = 1;
        private const int ThisCampaignId = 1;

        public BuyClickCampaign(ICampaignValidator buyClickValidator, IFilter buyClickFilter, IRule buyClickRule, ILoggerClient loggerClient)
        {
            _buyClickValidator = buyClickValidator ?? throw new ArgumentNullException(nameof(buyClickValidator));
            _buyClickFilter = buyClickFilter ?? throw new ArgumentNullException(nameof(buyClickFilter));
            _buyClickRule = buyClickRule ?? throw new ArgumentNullException(nameof(buyClickRule));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        public string CampaignName => ThisCampaignName;
        public int CampaignPriority => ThisCampaignPriority;

        public int CampaignId => ThisCampaignId;

        public List<IResult> ProcessLead(ILeadEntityImmutable leadEntity)
        {
            if (leadEntity == null) throw new ArgumentNullException(nameof(leadEntity));
            string processContext = "ProcessLead";

            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = $"Processing the Lead in {CampaignName}",ProcessContext = processContext,SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
            _campaignResultList = new List<IResult> { new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampStartKey, DateTime.Now) };
            _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.CampaignNameKey, CampaignName));
            _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.CampaignPriorityKey, CampaignPriority));
            _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.CampaignIdKey, CampaignId));
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Validating the Lead for {CampaignName} ", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
            if (_buyClickValidator.ValidLead(leadEntity).Equals(true))
            {
                // Add that Validation passed to ResultsCollection
                _campaignResultList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));
                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Filter the Lead for {CampaignName} ", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });

                // Check Filters
                if (_buyClickFilter.ClearedFilter(leadEntity))
                {
                    _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.FilterStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Check Campaign Rules {CampaignName} ", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
                    // Check Rules
                    if (_buyClickRule.ValidateForRule(leadEntity))
                    {
                        _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.RuleStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

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
                        _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.RuleStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                        _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Rule Failed", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
                    }


                }
                else
                {
                    // For now show that the lead has not processed successfully through the campaign and should not continue on to the resolver
                    _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.LeadSuccessStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                    _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.FilterStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Filter Failed", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
                }
            }
            else
            {
                // For now show that the lead has not processed successfully through the campaign and should not continue on to the resolver
                _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.LeadSuccessStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                _campaignResultList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Validation Failed", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
            }
            return _campaignResultList;
        }
    }
}
