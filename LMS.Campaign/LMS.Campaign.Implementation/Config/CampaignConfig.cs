using System;
using System.Collections.Generic;
using Compare.Services.LMS.Campaign.Implementation.Controller;
using Compare.Services.LMS.Campaign.Implementation.Validator;
using Compare.Services.LMS.Campaign.Interface;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Factory.Interface;
using Compare.Services.LMS.Controls.Validator.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace Compare.Services.LMS.Campaign.Implementation.Config
{
    /// <summary>
    /// Implementation of ICampaignConfig.
    /// This Class creates the Controls Config for the Campaign.  The
    /// Controls include the Validators and Controllers (Rules and Filters).
    /// This Config is then used by the Campaign so that the Campaign
    /// can check the LeadEntity against these Validators and Controllers.
    /// </summary>
    public class CampaignConfig : ICampaignConfig
    {
        // private readonly IControlFactory _controlFactory;
        private readonly ILoggerClient _loggerClient;
        private static string solutionContext = "CampaignConfig";
        private IValidator _campaignValidator;
        private IList<IValidator> _validators;
        private IController _campaignController;
        private IList<IController> _controllers;

        // The CampaignValidator is a wrapper and loops through the Validators defined for the Campaign
        public IValidator CampaignValidator
        {
            get { return _campaignValidator; }
            set { _campaignValidator = value; }
        }

        // List of Validators that are defined for the Campaign
        public IList<IValidator> Validators {
            get { return _validators; }
            set { _validators = value; }
        }

    // The CampaignController is a wrapper and loops through the Rules/Filters defined fro the Campaign
        public IController CampaignController
        {
            get { return _campaignController; }
            set { _campaignController = value; }
        }

        //List of Controllers (Rules and Filters) defined for the Campaingn.
        public IList<IController> Controllers
        {
            get { return _controllers; }
            set { _controllers = value; }
        }


        /// <summary>
        /// Constructor for CampaignConfig
        /// The CampaignId is used in the factories to retrieve the controls (i.e. Validators, Controllers (Rules & Filters).
        /// </summary>
        /// <param name="campaignId"></param>
        /// <param name="validatorFactory"></param>
        /// <param name="controllerFactory"></param>
        /// <param name="loggerClient"></param>
        public CampaignConfig(int campaignId, IValidatorFactory validatorFactory, IControllerFactory controllerFactory, ILoggerClient loggerClient)
        {
            if (campaignId <= 0)
            {
                throw new ArgumentException($"Error: {solutionContext}: campaignId = {campaignId}");
            }
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

            var processContext = "CampaignConfig";
            try
            {
                //Retrieve the applicable Validators from the Factory for this campaign
                var validatorFactoryIn = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
                _validators = validatorFactoryIn.BuildCampaignValidators(campaignId);
                if (_campaignValidator == null)
                {
                    _campaignValidator = new CampaignValidator(Validators, _loggerClient);

                }

                //Retrieve the applicable Controllers from the Factory for this campaign
                var controllerFactoryIn = controllerFactory ?? throw new ArgumentNullException(nameof(controllerFactory));
                _controllers = controllerFactoryIn.BuildCampaignControllers(campaignId);
                if (_campaignController == null)
                {
                    _campaignController = new CampaignController(Controllers, _loggerClient);
                }

            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = "Exception raised during assembly of Campaign Configuration.", ProcessContext = processContext, SolutionContext = solutionContext, Exception = ex, ErrorContext = ex.Message, EventType = LoggerClientEventType.LoggerClientEventTypes.Error });
                throw;
            }
         

        }
    }
}
