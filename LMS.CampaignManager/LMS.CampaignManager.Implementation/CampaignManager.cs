using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Compare.Services.LMS.Campaign.Interface;
using Compare.Services.LMS.CampaignManager.Interface;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Validator.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Components;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace Compare.Services.LMS.CampaignManager.Implementation
{
    public class CampaignManager : ICampaignManager
    {
        private int _campaignManagerId;
        private readonly ICampaignManagerConfig _campaignManagerConfig;
    
        private readonly ILoggerClient _loggerClient;
       // private static readonly List<IResult>[] EmptyResultArray = new List<IResult>[] { };
        private const string SolutionContext = "CampaignManager";
        private List<IResult> _campaignManagerResultList;


        public int CampaignManagerId
        {
            get { return _campaignManagerId; }
            set { _campaignManagerId = value; }
        }
        public string CampaignManagerDescription { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="campaignManagerId"></param>
        /// <param name="campaignManagerConfig"></param>
        /// <param name="loggerClient"></param>
        public CampaignManager(int campaignManagerId, ICampaignManagerConfig campaignManagerConfig, ILoggerClient loggerClient)
        {
            _campaignManagerId = campaignManagerId > 0
                ? campaignManagerId
                : throw new ArgumentException($"Error: {SolutionContext}: campaignId = {campaignManagerId}");
            _campaignManagerConfig = campaignManagerConfig ??
                                         throw new ArgumentNullException(nameof(campaignManagerConfig));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

            // When the subscriber receives a lead, invoke the CampaignManagerDriver
            _campaignManagerConfig.CampaignManagerSubscriber.SetupAddOnReceiveActionToChannel(CampaignManagerDriver);
        }

        /// <summary>
        /// Campaign Manager Driver to control campaigns
        /// </summary>
        /// <param name="leadEntity"></param>
        public void CampaignManagerDriver(ILeadEntity leadEntity)
        {
            if (leadEntity == null) throw new ArgumentNullException(nameof(leadEntity));

            var processContext = "CampaignManagerDriver";
            _campaignManagerResultList = new List<IResult> { new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampStartKey, DateTime.Now) };
            _campaignManagerResultList.Add(new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignManagerIdKey, CampaignManagerId));
            // Check that there are Campaigns to be Managed
            if (_campaignManagerConfig.CampaignCollection?.Any() != true)
            {
                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "There are no Campaigns to be Managed.", ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                _campaignManagerResultList.Add(new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignCountKey, 0));
                _campaignManagerConfig.CampaignManagerDecorator.DecorateLead(leadEntity, _campaignManagerResultList);
                return;
            }

            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Validating Lead Within the Campaign Manager.",ProcessContext = processContext,SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
            // Validate the Lead to see if it should be sent through campaigns
            if (_campaignManagerConfig.CampaignManagerValidator != null)
            {
                if (_campaignManagerConfig.CampaignManagerValidator.ValidLead(leadEntity).Equals(false))
                {
                    _campaignManagerResultList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey,
                        ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Lead is not valid for these Campaigns.", ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                    _campaignManagerConfig.CampaignManagerDecorator.DecorateLead(leadEntity, _campaignManagerResultList);
                    return;
                }
            }
            _campaignManagerResultList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey,
                ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Lead is valid for these Campaigns - Start Campaigns.", ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

            // Kick off all the campaigns with a task and then resolve following.
            ProcessCampaigns(leadEntity);

 
        }

        public IList<IResult>[] ProcessCampaigns(ILeadEntity leadEntity)
        {
            var processContext = "ProcessCampaigns";
            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Checking for Campaigns to be Managed.",ProcessContext = processContext,SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
     
            // Start the various Campaigns as Tasks
            var campaignCnt = _campaignManagerConfig.CampaignCollection.Length;
            _campaignManagerResultList.Add(new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignCountKey,
                campaignCnt));
            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = $"Campaign Count to be Managed: {campaignCnt}",ProcessContext = processContext,SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
            var processCampaignsTask = new Task<List<IResult>[]>(() =>
            {
                var campaignResults = new List<IResult>[campaignCnt];

                var campaignTasks = new Task<List<IResult>>[campaignCnt];
                for (var ix = 0; ix < campaignCnt; ix++)
                {
                    var ixClosure = ix;
                    campaignTasks[ixClosure] =
                        new Task<List<IResult>>(() => _campaignManagerConfig.CampaignCollection[ixClosure].ProcessLead(leadEntity));
                    campaignTasks[ixClosure].Start();
                }

                for (var i = 0; i < campaignCnt; i++)
                {
                    var ixClosure = i;
                    campaignResults[ixClosure] = campaignTasks[ixClosure].Result;
                }

                return campaignResults;
            });

            _campaignManagerResultList.Add(new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignsProcessedStatusKey,
                ResultKeys.ResultKeysStatusEnum.Processed.ToString()));


            // When all the campaigns are finished processing - call the function to Process all the Results from the different campaigns.
            processCampaignsTask.ContinueWith(async resultsCollection =>
                CampaignManagerProcessResults(leadEntity, (await resultsCollection)));

            // Start all the campaign taskis
            processCampaignsTask.Start();

            return processCampaignsTask.Result;

        }

        /// <summary>
        /// Campaign Manager Function to Process the Results from the Various Campaigns
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <param name="campaignResultCollectionList"></param>
        public void CampaignManagerProcessResults(ILeadEntity leadEntity, List<IResult>[] campaignResultCollectionList)
        {
            var processContext = "CampaignManagerProcessResults";
            // Resolve the results collection from the Campaigns
            try
            {
                // Create array for the number of campaigns that executed.
                leadEntity.ResultCollection.CampaignCollection = new IResult[campaignResultCollectionList.Length][];
                // Assign the results list to the leadEntity
                var ix = 0;
                foreach (var resultList in campaignResultCollectionList)
                {
                    // LeadEntity now holds result for each Campaign it was processed through
                    leadEntity.ResultCollection.CampaignCollection[ix] = new IResult[resultList.Count];
                    leadEntity.ResultCollection.CampaignCollection[ix] = (resultList.ToArray());
                    ix++;
                }
                // Resolve the lead
                _campaignManagerConfig.CampaignManagerResolver.ResolveLead(leadEntity);

            }
            catch (Exception exception)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject() { OperationContext = "Exception occurred within CampaignManager.ResolveLeads.", ProcessContext = processContext, SolutionContext = SolutionContext, ErrorContext = exception.Message, Exception = exception, EventType = LoggerClientEventType.LoggerClientEventTypes.Error });
                _campaignManagerResultList.Add(new DefaultResult(ResultKeys.ResolverResultCountKey, ResultKeys.ResultKeysStatusEnum.Failed));
                _campaignManagerConfig.CampaignManagerDecorator.DecorateLead(leadEntity, _campaignManagerResultList);
                throw;
            }

            // Decorate the lead with the CampaignManager logs
            _campaignManagerConfig.CampaignManagerDecorator.DecorateLead(leadEntity, _campaignManagerResultList);

            // Publish the lead to .....POE?
            _campaignManagerConfig.CampaignManagerPublisher.PublishLead(leadEntity);
        }
    }
}
