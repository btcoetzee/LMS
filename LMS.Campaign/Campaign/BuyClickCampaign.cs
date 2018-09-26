using System;
using System.Collections.Generic;
using Compare.Services.LMS.Modules.LeadEntity.Components;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;
using Compare.Services.LMS.Campaign.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace Compare.Services.LMS.Campaign.BuyClick
{
    public class BuyClickCampaign : ICampaign
    {
        private readonly ICampaignConfig _campaignConfig;
        private readonly ILoggerClient _loggerClient;
        private List<IResult> _campaignResultList;
        private static string solutionContext = "BuyClickCampaign";

        public string CampaignDescription { get => CampaignDescription; set => CampaignDescription = value; }

        public int CampaignPriority { get => CampaignPriority; set => CampaignPriority = value; }

        public int CampaignId { get => CampaignId; set => CampaignId = value; }

        public BuyClickCampaign(int campaignId, string campaignDescription, int campaignPriority, ICampaignConfig campaignConfig, ILoggerClient loggerClient)
        {
            //TODO: Validate against DB for existing campaignId? - Don't have to b/c CampaignManager calls out and retrieved from DB ---EVENTUALLY!!!!
            CampaignId = campaignId > 0
                ? campaignId
                : throw new ArgumentException($"Error: {solutionContext}: campaignId = {campaignId}");
            CampaignDescription = campaignDescription ?? throw new ArgumentNullException(nameof(campaignDescription));
            CampaignPriority = campaignPriority > 0
                ? campaignPriority
                : throw new ArgumentException($"Error: {solutionContext}: campaignPriority = {campaignPriority}");
            _campaignConfig = campaignConfig ?? throw new ArgumentNullException(nameof(campaignConfig));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        /// <summary>
        /// Process the lead through the Campaign to see if the Lead Qualifies through the
        /// Validators, Controls (Filters and Rules).  
        /// The Result List is the return from the Campaign to the Campaign Manager.  
        /// The Result List is a key value pair list to indicate if a lead qualifies or not.
        /// The Validators and Controls Components outcomes are logged in the Result List.  
        /// Ultimately the ResultKeys.CampaignKeys.LeadSuccessStatusKey indicates the final 
        /// outcome following the procesing through the Validators and Controls.
        /// The Campaign Manager uses the Result List to decide if the Lead qualifies for
        /// further processing within the Campaign Manager Resolver.
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns>List&lt;IResult&gt;</returns>
        public List<IResult> ProcessLead(ILeadEntityImmutable leadEntity)
        {

            if (leadEntity == null) throw new ArgumentNullException(nameof(leadEntity));
            string processContext = "ProcessLead";
            _loggerClient.Log(new DefaultLoggerClientObject {OperationContext = $"Processing the Lead in {CampaignDescription}",ProcessContext = processContext,SolutionContext = solutionContext,EventType = LoggerClientEventType.LoggerClientEventTypes.Information});

            try
            {
                // Create Campaign Result List and insert Campaign Details
                _campaignResultList = new List<IResult> { new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampStartKey, DateTime.Now) };
                _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.CampaignIdKey, CampaignId));
                _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.CampaignNameKey, CampaignDescription));
                _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.CampaignPriorityKey, CampaignPriority));

                // To use the Validators/Controls (Rules and Filters) the provisional mutable leadEntity needs to be passed
                ILeadEntity tmpMutableLeadEntity = new DefaultLeadEntityImmutableProvision(leadEntity);
                // Validate the Lead
                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Validating the Lead for {CampaignDescription} ", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                if (_campaignConfig.CampaignValidator.ValidLead(tmpMutableLeadEntity).Equals(true))
                {
                    // Add that Validation passed to ResultsCollection
                    _campaignResultList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey,
                        ResultKeys.ResultKeysStatusEnum.Processed.ToString()));
                }
                else
                {
                    // For now show that the lead has not processed successfully through the campaign and should not continue on to the resolver
                    _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.LeadSuccessStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                    _campaignResultList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "LeadEntity failed during Campaign Validation", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                    return _campaignResultList;
                }

                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Passing lead through CampaignController Constraints in {CampaignDescription} ", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                if (_campaignConfig.CampaignController.ConstraintMet(tmpMutableLeadEntity))
                {
                    _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.ControlStatusKey,
                        ResultKeys.ResultKeysStatusEnum.Processed.ToString()));
                }
                else
                {
                    // For now show that the lead has not processed successfully through the campaign and should not continue on to the resolver
                    _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.LeadSuccessStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                    _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.ControlStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "LeadEntity Failed Constraints in Controller", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                    return _campaignResultList;
                }

                // What else can we retrieve here????
                _campaignResultList.Add((new DefaultResult(ResultKeys.CampaignKeys.CampaignMessageHandlerKey, $"MessageHandlerFor{CampaignDescription}")));

                // The lead processed successfully through the whole campaign and should continue on to the resolver
                _campaignResultList.Add(new DefaultResult(ResultKeys.CampaignKeys.LeadSuccessStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

                // Add the end time to the result list 
                _campaignResultList.Add(new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampEndKey, DateTime.Now));

            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = "Exception raised during Campaign Processing.", ProcessContext = processContext, SolutionContext = solutionContext, Exception = ex, ErrorContext = ex.Message, EventType = LoggerClientEventType.LoggerClientEventTypes.Error });
                throw;
            }
  
            return _campaignResultList;
        }
    }

}
