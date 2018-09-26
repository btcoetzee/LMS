using System;
using System.Collections.Generic;
using System.Linq;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Modules.LeadEntity.Components;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace Compare.Services.LMS.CampaignManager.Implementation.Resolver
{
    /// <summary>
    /// This class will eventually become the wrapper for all resolvers.  Currently
    /// it looks at the campaign priority for the successful campaigns and 
    /// assigns the prefferred campaign using the priority only.
    /// </summary>
    public class CampaignManagerResolver : IResolver
    {
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "CampaignManagerResolver";
        private List<IResult> _campaignManagerResultList;

        public CampaignManagerResolver(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        /// <summary>
        /// Resolve the leads
        /// Using the results collection list from each of the campaigns - assign the successful campaign to the 
        /// Preferred ResultList where the campaign was successful. 
        /// </summary>
        /// <param name="leadEntity"></param>
        public void ResolveLead(ILeadEntity leadEntity)
        {
            var processContext = "ResolveLead";
            if ((leadEntity.ResultCollection.CampaignCollection == null) || leadEntity.ResultCollection.CampaignCollection.Length == 0)
            {
                _campaignManagerResultList = new List<IResult> { new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampStartKey, DateTime.Now) };
                _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "There are no Leads to resolve",ProcessContext = processContext,SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                return;
            }

            _loggerClient.Log(new DefaultLoggerClientObject {OperationContext = "Resolving the campaign results list",ProcessContext = processContext,SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information }) ;
            // Choose the campaign info with the highest priority and successful
            var ix = 0;
            var priorityIx = 0;
            var highestPriority = Int32.MaxValue;
            var campaignResultCollectionList = leadEntity.ResultCollection.CampaignCollection.ToList();
            foreach (var resultList in campaignResultCollectionList)
            {
                // If the campaign processed successfully, then seek the Campaign that was successful and has the highest priority
                var campaignSuccessStatus = resultList.SingleOrDefault(r => r.Id == ResultKeys.CampaignKeys.LeadSuccessStatusKey)
                    ?.Value;
                if (campaignSuccessStatus != null)
                {
                    if (campaignSuccessStatus.ToString() == ResultKeys.ResultKeysStatusEnum.Processed.ToString())
                    {
                        var value = resultList
                            .SingleOrDefault(r => r.Id == ResultKeys.CampaignKeys.CampaignPriorityKey)
                            ?.Value;
                        var priorityStr = value?.ToString();
                        if (priorityStr != null)
                        {
                            if (Int32.TryParse(priorityStr, out int priorityInt))
                            {
                                if (priorityInt < highestPriority)
                                {
                                    //This is the array position for the campaign with the highest priority
                                    priorityIx = ix;
                                    highestPriority = priorityInt;
                                }
                            }
                        }
                    }
                }
                ix++;
            }

            // Check that there was a preferred campaign and assign the PreferredCampaignCollection
            if (highestPriority != Int32.MaxValue)
            {
                leadEntity.ResultCollection.PreferredCampaignCollection = leadEntity.ResultCollection.CampaignCollection[priorityIx];
                var campaignName = leadEntity.ResultCollection.PreferredCampaignCollection.SingleOrDefault(r => r.Id == ResultKeys.CampaignKeys.CampaignNameKey)
                    ?.Value;
                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Lead selected is coming from Campaign: {campaignName} ", ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

            }
            else
            {
                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Did not find valid priority within Campaign Result List", ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

            }
  
  
        }
    }
}
