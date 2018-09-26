using System;
using System.Collections.Generic;
using System.Text;
using Compare.Services.LMS.Campaign.Interface;
using Compare.Services.LMS.CampaignManager.Implementation.Validator;
using Compare.Services.LMS.CampaignManager.Interface;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Factory.Interface;
using Compare.Services.LMS.Controls.Validator.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace Compare.Services.LMS.CampaignManager.Implementation.Config
{

    /// <summary>
    /// Implementation of ICampaignManagerConfig.
    /// This Class creates the Components for the CampaignManager.  The
    /// Components include the Subscriber, Validators, Decorator, Publisher, Campaigns and Resolvers.
    /// This Config is then used by the CampaignManager 
    /// to propagate and decorate the lead through the subscribed Campaigns 
    /// </summary>
    public class CampaignManagerConfig : ICampaignManagerConfig
    {
        // private readonly IControlFactory _controlFactory;
        private readonly ILoggerClient _loggerClient;
        private static string solutionContext = "CampaignConfig";
        private IValidator _campaignManagerValidator;
        private IList<IValidator> _validators;
        private ISubscriber _campaignManagerSubscriber;
        private IDecorator _campaignManagerDecorator;
        private IResolver _campaignManagerResolver;
        private IList<IResolver> _resolvers;
        private IPublisher _campaignManagerPublisher;
        private ICampaign[] _campaignCollection;

        // The CampaignManagerValidator is a wrapper and loops through the Validators defined for the CampaignManager
        public IValidator CampaignManagerValidator
        {
            get { return _campaignManagerValidator; }
            set { _campaignManagerValidator = value; }
        }

        // List of Validators that are defined for the CampaignManager
        public IList<IValidator> Validators
        {
            get { return _validators; }
            set { _validators = value; }
        }

        // The Subscriber for the Campaign Manager
        public ISubscriber CampaignManagerSubscriber
        {
            get { return _campaignManagerSubscriber; }
            set { _campaignManagerSubscriber = value; }
        }

        // The Campaign Collection subscribed for the Campaign Manager
        public ICampaign[] CampaignCollection
        {
            get { return _campaignCollection; }
            set { _campaignCollection = value; }
        }
        // The Subscriber for the Campaign Manager
        public IDecorator CampaignManagerDecorator
        {
            get { return _campaignManagerDecorator; }
            set { _campaignManagerDecorator = value; }
        }

        // The CampaignManagerResolver is a wrapper and loops through the Resolver defined for the CampaignManager
        public IResolver CampaignManagerResolver
        {
            get { return _campaignManagerResolver; }
            set { _campaignManagerResolver = value; }
        }
        //List of Resolvers defined for the CampaingnManager
        public IList<IResolver> Resolvers
        {
            get { return _resolvers; }
            set { _resolvers = value; }
        }

        // The {Subscriberublisher} for the Campaign Manager
        public IPublisher CampaignManagerPublisher
        {
            get { return _campaignManagerPublisher; }
            set { _campaignManagerPublisher = value; }
        }

        /// <summary>
        /// Constructor for CampaignConfig
        /// The CampaignId is used in the factories to retrieve the controls (i.e. Validators, Controllers (Rules & Filters).
        /// </summary>
        /// <param name="campaignManagerId"></param>
        /// <param name="validatorFactory"></param>
        /// <param name="campaignManagerPublisher"></param>
        /// <param name="loggerClient"></param>
        /// <param name="campaignManagerSubscriber"></param>
        /// <param name="campaignCollection"></param>
        /// <param name="campaignManagerDecorator"></param>
        /// <param name="campaignManagerResolver"></param>
        public CampaignManagerConfig(int campaignManagerId, 
                                    IValidatorFactory validatorFactory, 
                                    ISubscriber campaignManagerSubscriber,
                                    ICampaign[] campaignCollection,
                                    IDecorator campaignManagerDecorator,
                                    IResolver campaignManagerResolver, 
                                    IPublisher campaignManagerPublisher, 
                                    ILoggerClient loggerClient)
        {
            if (campaignManagerId <= 0)
            {
                throw new ArgumentException($"Error: {solutionContext}: campaignManagerId = {campaignManagerId}");
            }
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

            var processContext = "CampaignManagerConfig";
            try
            {
                // Subscriber
                _campaignManagerSubscriber = campaignManagerSubscriber ?? throw new ArgumentNullException(nameof(campaignManagerSubscriber));
                // When the subscriber receives a lead, invoke the CampaignManagerDriver
                //TODO: _campaignManagerSubscriber.SetupAddOnReceiveActionToChannel(CampaignManager.CampaignManagerDriver);
                //Retrieve the applicable Validators from the Factory for this campaign - If they are defined,
                // Create the CampaignManagerValidator that is the wrapper.
                var validatorFactoryIn = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
                _validators = validatorFactoryIn.BuildCampaignValidators(campaignManagerId);
                if (_campaignManagerValidator == null)
                {
                    CampaignManagerValidator = new CampaignManagerValidator(Validators, _loggerClient);
                }

                // Campaigns that are managed by the Campaign Manager
                _campaignCollection = campaignCollection ?? throw new ArgumentNullException(nameof(campaignCollection));

                // CampaignManager Decorator
                _campaignManagerDecorator = campaignManagerDecorator ??
                                            throw new ArgumentNullException(nameof(campaignManagerDecorator));

                // So this assignment will change and the Resolver will be created if we start changing it to a collection of resolvers.....
                // This Resolver will become a wrapper.
                _campaignManagerResolver = campaignManagerResolver ??
                                           throw new ArgumentNullException(nameof(campaignManagerResolver));

                // Publisher
                _campaignManagerPublisher = campaignManagerPublisher ??
                                            throw new ArgumentNullException(nameof(campaignManagerPublisher));

            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = "Exception raised during assembly of Campaign Configuration.", ProcessContext = processContext, SolutionContext = solutionContext, Exception = ex, ErrorContext = ex.Message, EventType = LoggerClientEventType.LoggerClientEventTypes.Error });
                throw;
            }


        }
    }
}
