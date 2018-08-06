namespace LMS.CampaignManager.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using LMS.LoggerClient.Interface;
    using LMS.Campaign.Interface;
    using LMS.CampaignManager.Interface;
    using LMS.CampaignManager.Subscriber.Interface;
    using LMS.CampaignManager.Resolver.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LeadEntity.Components;
    using LMS.LeadEntity.Interface.Constants;
    using LMS.CampaignManager.Decorator.Interface;
    using LMS.CampaignManager.Publisher.Interface;
    using LMS.CampaignManager.Validator.Interface;

    public class CampaignManager : ICampaignManager
    {
        private readonly ICampaignManagerSubscriber _campaignManagerSubscriber;
        private readonly ICampaignManagerDecorator _campaignManagerDecorator;
        private readonly ICampaignManagerResolver _campaignManagerResolver;
        private readonly ICampaignManagerPublisher _campaignManagerPublisher;
        private readonly ICampaignManagerValidator[] _campaignManagerValidatorCollection;
        private readonly ICampaign[] _campaignCollection;
        private readonly ILoggerClient _loggerClient;
       // private static readonly List<IResult>[] EmptyResultArray = new List<IResult>[] { };
        private const string SolutionContext = "CampaignManager";
        private List<IResult> _campaignManagerResultList;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="campaignManagerSubscriber"></param>
        /// <param name="campaignCollection"></param>
        /// <param name="campaignManagerValidatorCollection"></param>
        /// <param name="campaignManagerDecorator"></param>
        /// <param name="campaignManagerResolver"></param>
        /// <param name="campaignManagerPublisher"></param>
        /// <param name="loggerClient"></param>
        public CampaignManager(ICampaignManagerSubscriber campaignManagerSubscriber, ICampaign[] campaignCollection,
            ICampaignManagerValidator[] campaignManagerValidatorCollection, ICampaignManagerDecorator campaignManagerDecorator,
            ICampaignManagerResolver campaignManagerResolver, ICampaignManagerPublisher campaignManagerPublisher, ILoggerClient loggerClient)
        {

            _campaignManagerSubscriber = campaignManagerSubscriber ??
                                         throw new ArgumentNullException(nameof(campaignManagerSubscriber));
            _campaignCollection = campaignCollection ?? throw new ArgumentNullException(nameof(campaignCollection));
            // Validators can be Optional
            _campaignManagerValidatorCollection = campaignManagerValidatorCollection;
            _campaignManagerDecorator = campaignManagerDecorator ??
                                        throw new ArgumentNullException(nameof(campaignManagerDecorator));
            _campaignManagerResolver = campaignManagerResolver ??
                                       throw new ArgumentNullException(nameof(campaignManagerResolver));
            _campaignManagerPublisher = campaignManagerPublisher ??
                                       throw new ArgumentNullException(nameof(campaignManagerPublisher));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

            // When the subscriber receives a lead, invoke the CampaingManagerDriver
            _campaignManagerSubscriber.SetupAddOnReceiveActionToChannel(CampaignManagerDriver);
        }

        /// <summary>
        /// Constructor for Campaign with no Campaign Validators 
        /// </summary>
        /// <param name="campaignManagerSubscriber"></param>
        /// <param name="campaignCollection"></param>
        /// <param name="campaignManagerDecorator"></param>
        /// <param name="campaignManagerResolver"></param>
        /// <param name="campaignManagerPublisher"></param>
        /// <param name="loggerClient"></param>
        public CampaignManager(ICampaignManagerSubscriber campaignManagerSubscriber, ICampaign[] campaignCollection,
            ICampaignManagerDecorator campaignManagerDecorator,
            ICampaignManagerResolver campaignManagerResolver, ICampaignManagerPublisher campaignManagerPublisher, ILoggerClient loggerClient)
            : this(campaignManagerSubscriber, campaignCollection, null, campaignManagerDecorator,
                campaignManagerResolver, campaignManagerPublisher, loggerClient)
        {
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
            // Check that there are Campaigns to be Managed
            if (_campaignCollection?.Any() != true)
            {
                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "There are no Campaigns to be Managed.", ProcessContext = processContext, SolutionContext = SolutionContext });
                _campaignManagerResultList.Add(new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignCountKey, 0));
            }

            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Validating Lead Within the Campaign Manager.",ProcessContext = processContext,SolutionContext = SolutionContext});
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

            if (validForTheseCampaigns)
            {
                _campaignManagerResultList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey,
                    ResultKeys.ResultKeysStatusEnum.Processed.ToString()));
                _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Lead is valid for these Campaigns - Start Campaigns.",ProcessContext = processContext,SolutionContext = SolutionContext});

                // Kick off all the campaigns with a task and then resolve following.
                ProcessCampaigns(leadEntity);
            }
            else
            {
                _campaignManagerResultList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey,
                    ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Lead is not valid for these Campaigns.",ProcessContext = processContext,SolutionContext = SolutionContext});
                _campaignManagerDecorator.DecorateLead(leadEntity, _campaignManagerResultList);
            }

        }

        public List<IResult>[] ProcessCampaigns(ILeadEntity leadEntity)
        {
            var processContext = "ProcessCampaigns";
            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Checking for Campaigns to be Managed.",ProcessContext = processContext,SolutionContext = SolutionContext});
     
            // Start the various Campaigns as Tasks
            var campaignCnt = _campaignCollection.Length;
            _campaignManagerResultList.Add(new DefaultResult(ResultKeys.CampaignManagerKeys.CampaignCountKey,
                campaignCnt));
            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = $"There are {campaignCnt} Campaigns to be Managed.",ProcessContext = processContext,SolutionContext = SolutionContext});
            var processCampaignsTask = new Task<List<IResult>[]>(() =>
            {
                var campaignResults = new List<IResult>[campaignCnt];

                var campaignTasks = new Task<List<IResult>>[campaignCnt];
                for (var ix = 0; ix < campaignCnt; ix++)
                {
                    var ixClosure = ix;
                    campaignTasks[ixClosure] =
                        new Task<List<IResult>>(() => _campaignCollection[ixClosure].ProcessLead(leadEntity));
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
        /// <param name="campaignResultCollection"></param>
        public void CampaignManagerProcessResults(ILeadEntity leadEntity, List<IResult>[] campaignResultCollection)
        {
            var processContext = "CampaignManagerProcessResults";
            // Resolve the results collection from the Campaigns
            try
            {
                _campaignManagerResolver.ResolveLeads(leadEntity, campaignResultCollection);

            }
            catch (Exception exception)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject() { OperationContext = "Exception occurred within CampaignManager.ResolveLeads.", ProcessContext = processContext, SolutionContext = SolutionContext, ErrorContext = exception.Message, Exception = exception});
                _campaignManagerResultList.Add(new DefaultResult(ResultKeys.ResolverResultCountKey, ResultKeys.ResultKeysStatusEnum.Failed));
                _campaignManagerDecorator.DecorateLead(leadEntity, _campaignManagerResultList);
                throw exception;
            }

            // Decorate the lead with the CampaignManager logs
            _campaignManagerDecorator.DecorateLead(leadEntity, _campaignManagerResultList);
            
            // Publish the lead to POE
            _campaignManagerPublisher.PublishLead(leadEntity);
        }
    }
}
