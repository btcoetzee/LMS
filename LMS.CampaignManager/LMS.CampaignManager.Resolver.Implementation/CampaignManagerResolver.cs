using System.Linq;

namespace LMS.CampaignManager.Resolver.Implementation
{
    using System;
    using LMS.LoggerClient.Interface;
    using LMS.CampaignManager.Resolver.Interface;
    using LMS.LeadEntity.Interface;
    public class CampaignManagerResolver : ICampaignManagerResolver
    {
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "CampaignManagerResolver";

        public CampaignManagerResolver(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        public void ResolveLeads(ILeadEntity[] leadCollection)
        {
            var processContext = "ResolveLeads";
            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Resolving the array of Leads",
                ProcessContext = processContext,
                SolutionContext = SolutionContext
            });

            // Check that there are Leads to Resolve 
            if (leadCollection.Any())
            {
                var ix = 1;
                foreach (var lead in leadCollection)
                {

                    var campaignName = lead.Results.SingleOrDefault((item =>
                                           item.Id == LeadEntity.Interface.Constants.ResultKeys
                                               .CampaignSuccessStatus)) != null ? (lead.Results.SingleOrDefault((item =>
                                                                                       item.Id == LeadEntity.Interface.Constants.ResultKeys
                                                                                           .CampaignSuccessStatus))
                                                                                       ?.Value ?? "Not Named") : "Not Named";
                    
                    _loggerClient.Log(new DefaultLoggerClientObject
                    {
                        OperationContext = $"Lead {ix} is coming from Campaign: {campaignName}",
                        ProcessContext = processContext,
                        SolutionContext = SolutionContext
                    });
                    ix++;
                }

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
