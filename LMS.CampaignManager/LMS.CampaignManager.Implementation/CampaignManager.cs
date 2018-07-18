﻿namespace LMS.CampaignManager.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Runtime.Serialization;
    using LMS.LoggerClient.Interface;
    using LMS.Campaign.Interface;
    using LMS.CampaignManager.Interface;
    using LMS.CampaignManager.Subscriber.Interface;
    using LMS.CampaignManager.Subscriber.Implementation;
    using LMS.Validator.Interface;
    using LMS.CampaignManager.Resolver.Interface;
    using LMS.LeadEntity.Interface;

    public class CampaignManager : ICampaignManager
    {
        private readonly ICampaignManagerSubscriber _campaignManagerSubscriber;
        private readonly ICampaignManagerResolver _campaignManagerResolver;
        private readonly IValidator[] _campaignManagerValidatorCollection;
        private readonly ICampaign[] _campaignCollection;
        private readonly ILoggerClient _loggerClient;
        private static readonly string[] EmptyResultArray = new string[] { };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="campaignManagerSubscriber"></param>
        /// <param name="campaignCollection"></param>
        /// <param name="campaignManagerValidatorCollection"></param>
        /// <param name="campaignManagerResolver"></param>
        /// <param name="loggerClient"></param>
        public CampaignManager(ICampaignManagerSubscriber campaignManagerSubscriber, ICampaign[] campaignCollection, 
                               IValidator[] campaignManagerValidatorCollection, 
                               ICampaignManagerResolver campaignManagerResolver, ILoggerClient loggerClient)
        {

            _campaignManagerSubscriber = campaignManagerSubscriber ??
                                         throw new ArgumentNullException(nameof(campaignManagerSubscriber));
            _campaignCollection = campaignCollection ?? throw new ArgumentNullException(nameof(campaignCollection));
            // Can be Optional
            _campaignManagerValidatorCollection = campaignManagerValidatorCollection;
            _campaignManagerResolver = campaignManagerResolver ??
                                       throw new ArgumentNullException(nameof(campaignManagerResolver));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

            // When the subscriber receives a lead, invoke the CampaingManagerDriver
            _campaignManagerSubscriber.SetupAddOnReceiveActionToChannel(CampaignManagerDriver);
        }

        /// <summary>
        /// Constructor for Campaign with no Campaign Validators 
        /// </summary>
        /// <param name="campaignManagerSubscriber"></param>
        /// <param name="campaignCollection"></param>
        /// <param name="campaignManagerResolver"></param>
        /// <param name="loggerClient"></param>
        public CampaignManager(ICampaignManagerSubscriber campaignManagerSubscriber, ICampaign[] campaignCollection, 
                                ICampaignManagerResolver campaignManagerResolver, ILoggerClient loggerClient) 
            : this (campaignManagerSubscriber, campaignCollection, null, campaignManagerResolver, loggerClient )
        {
        }

        /// <summary>
        /// Campaign Manager Driver to control campaigns
        /// </summary>
        /// <param name="leadEntity"></param>
        public void CampaignManagerDriver(ILeadEntity leadEntity)
        {
            bool validForTheseCampaigns = true;
            // Validate the Lead to see if it should be sent through campaigns
            if (_campaignManagerValidatorCollection != null)
            {
                foreach (var validator in _campaignManagerValidatorCollection)
                {
                    if (!validator.ValidLead(leadEntity))
                    {
                        // Do some logging & decorating?
                        validForTheseCampaigns = false;
                        break;
                    }
                }
            }

            if  (validForTheseCampaigns)
            {
                // Kick off all the campaigns with a task and then resolve following.
                ProcessCampaigns(leadEntity);

            }
 
            
        }

        public string[] ProcessCampaigns(ILeadEntity leadEntity)
        {
            // Check that there are Campaigns to be Managed
            if (!_campaignCollection.Any())
                return EmptyResultArray;

            // Start the various Campaigns as Tasks
            var campaignCnt = _campaignCollection.Length;
 
            var processCampaignsTask = new Task<string[]>(() =>
            {
                var campaignResults = new string[campaignCnt];

                var campaignTasks = new Task<string>[campaignCnt];
                for (var ix = 0; ix < campaignCnt; ix++)
                {
                    var ixClosure = ix;
                    campaignTasks[ix] = new Task<string>(() => _campaignCollection[ixClosure].ProcessLead(leadEntity.ToString()));
                    campaignTasks[ix].Start();
                }
                for (var i = 0; i < campaignCnt; i++)
                {
                    campaignResults[i] = campaignTasks[i].Result;
                }

                return campaignResults;
            });

            // Now resolve like something below
            processCampaignsTask.ContinueWith(async resultsCollection =>
                _campaignManagerResolver.ResolveLeads((await resultsCollection)));

            processCampaignsTask.Start();
            return processCampaignsTask.Result;
       
        }



    }
}