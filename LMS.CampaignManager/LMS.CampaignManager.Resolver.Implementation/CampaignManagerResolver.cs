using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace LMS.CampaignManager.Resolver.Implementation
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LMS.LoggerClient.Interface;
    using LMS.CampaignManager.Resolver.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LeadEntity.Components;
    using LMS.LeadEntity.Interface.Constants;
    public class CampaignManagerResolver : ICampaignManagerResolver
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
        /// Using the results collection list from each of the campaigns
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <param name="campaignResultCollectionList"></param>
        public void ResolveLeads(ILeadEntity leadEntity, List<IResult>[] campaignResultCollectionList)
        {
            var processContext = "ResolveLeads";
            if (campaignResultCollectionList?.Any() != true)
            {
                _campaignManagerResultList = new List<IResult> { new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampStartKey, DateTime.Now) };
                _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "There are no Leads to resolve",ProcessContext = processContext,SolutionContext = SolutionContext});
                return;
            }

            _loggerClient.Log(new DefaultLoggerClientObject {OperationContext = "Resolving the campaign results list",ProcessContext = processContext,SolutionContext = SolutionContext});

            // Check that there are Leads to Resolve 
            if (campaignResultCollectionList.Any())
            {

                // Choose the campaign info with the highest priority
                var ix = 0;
                var priorityIx = 0;
                var highestPriority = Int32.MaxValue;
                // Create array for the number of campaigns that executed.
                leadEntity.ResultCollection.CampaignCollection = new IResult[campaignResultCollectionList.Length][];
                foreach (var resultList in campaignResultCollectionList)
                {
                    leadEntity.ResultCollection.CampaignCollection[ix] = new IResult[resultList.Count];
                    leadEntity.ResultCollection.CampaignCollection[ix] = (resultList.ToArray());

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
                                Int32.TryParse(priorityStr, out int priorityInt);
                                if (priorityInt < highestPriority)
                                {
                                    priorityIx = ix;
                                    highestPriority = priorityInt;
                                }
                            }
                        }

                    }
                    ix++;
                }

                // Check that there was a preferred campaign
                if (highestPriority != Int32.MaxValue)
                {
                    leadEntity.ResultCollection.PreferredCampaignCollection = leadEntity.ResultCollection.CampaignCollection[priorityIx];
                    var campaignName = leadEntity.ResultCollection.PreferredCampaignCollection.SingleOrDefault(r => r.Id == ResultKeys.CampaignKeys.CampaignNameKey)
                        ?.Value;
                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Lead selected is coming from Campaign: {campaignName} ", ProcessContext = processContext, SolutionContext = SolutionContext });

                }
            }
            else
            {
                _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "There are no Leads to Resolve.",ProcessContext = processContext,SolutionContext = SolutionContext});
            }

        }
    }
}
