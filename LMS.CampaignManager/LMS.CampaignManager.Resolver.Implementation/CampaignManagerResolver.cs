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
    using LMS.Decorator.Interface;
    public class CampaignManagerResolver : ICampaignManagerResolver
    {
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "CampaignManagerResolver";
        private readonly IDecorator _campaignManagerDecorator;
        private List<IResult> _campaignManagerResultList;

        public CampaignManagerResolver(IDecorator campaignManagerDecorator, ILoggerClient loggerClient)
        {
            _campaignManagerDecorator = campaignManagerDecorator ??
                                        throw new ArgumentNullException(nameof(campaignManagerDecorator));
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
                foreach (var resultList in campaignResultCollectionList)
                {
                    leadEntity.ResultCollection.CampaignCollection[ix] = resultList.ToArray();
                    var priorityStr = resultList.SingleOrDefault(r => r.Id == ResultKeys.CampaignKeys.CampaignPriorityKey) != null
                            ? resultList.SingleOrDefault(r => r.Id == ResultKeys.CampaignKeys.CampaignPriorityKey)
                                ?.Value.ToString(): null;
                    Int32.TryParse(priorityStr, out int priority);
                    if (priority < highestPriority)
                    {
                        priorityIx = ix;
                    }
                    ix++;
                }

                leadEntity.ResultCollection.PreferredCampaignCollection = leadEntity.ResultCollection.CampaignCollection[priorityIx];
                _loggerClient.Log(new DefaultLoggerClientObject 
                {
                    // TBD OperationContext = $"Lead {ix} is coming from Campaign: {campaignName} and phone number is: {phoneNumber} ",
                    ProcessContext = processContext,
                    SolutionContext = SolutionContext
                });

            }
            else
            {
                _loggerClient.Log(new DefaultLoggerClientObject
                {
                    OperationContext = "There are no Leads to Resolve.",
                    ProcessContext = processContext,
                    SolutionContext = SolutionContext
                });
            }

        }
    }
}
