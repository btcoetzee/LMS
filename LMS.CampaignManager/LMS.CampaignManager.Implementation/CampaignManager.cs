using System.Runtime.Serialization;

namespace LMS.CampaignManager.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LMS.LoggerClient.Interface;
    using LMS.Campaign.Interface;
    using LMS.CampaignManager.Interface;

    public class CampaignManager : ICampaignManager
    {
        private readonly ICampaign[] _campaignArray;
        private readonly ILoggerClient _loggerClient;
        private static readonly string[] EmptyResultArray = new string[] { };

        public CampaignManager(ICampaign[] campaignArray, ILoggerClient loggerClient)
        {
            _campaignArray = campaignArray ?? throw new ArgumentNullException(nameof(campaignArray));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        public string[] ProcessCampaigns(string message)
        {
            // Check that there are Campaigns to be Managed
            if (!_campaignArray.Any())
                return EmptyResultArray;

            // Start the various Campaigns as Tasks
            var campaignCnt = _campaignArray.Length;
            //var campaignResults = new string[campaignCnt];
            var processCampaignsTask = new Task<string[]>(() =>
            {
                var campaignResults = new string[campaignCnt];

                var campaignTasks = new Task<string>[campaignCnt];
                for (var ix = 0; ix < campaignCnt; ix++)
                {
                    var ixClosure = ix;
                    campaignTasks[ix] = new Task<string>(() => _campaignArray[ixClosure].ProcessLead(message));
                    campaignTasks[ix].Start();
                }
                for (var i = 0; i < campaignCnt; i++)
                {
                    campaignResults[i] = campaignTasks[i].Result;
                }

                return campaignResults;
            });

            // Now resolve like something below
            //processCampaignsTask.ContinueWith(async resultsCollection => _resolver.resolveLeadsFromCampaigns(await resultsCollection) );

            processCampaignsTask.Start();
            return processCampaignsTask.Result;

       
        }



    }
}
