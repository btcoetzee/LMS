using System;
using System.Collections.Generic;
using Compare.Services.LMS.CampaignConfig.Interface;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Factory.Interface;
using Compare.Services.LMS.Controls.Validator.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace Compare.Services.LMS.CampaignConfig.Implementation
{
    /// <summary>
    /// Implementation of ICampaignConfig.
    /// This Class creates the Controls Config for the Campaign.  The
    /// Controls include the Validators and Controllers (Rules and Filters).
    /// This Config is then consumed by the Campaign so that the Campaign
    /// can check the LeadEntity against these Controls.
    /// </summary>
    //public class CampaignConfig : ICampaignConfig
    //{
    //    // private readonly IControlFactory _controlFactory;
    //    private readonly ILoggerClient _loggerClient;
    //    private static string solutionContext = "CampaignConfig";

    //    public IController Control { get; set; }
    //    // The CampaignValidator loops through the Validators and captures the result
    //    public IValidator CampaignValidator { get { return CampaignValidator; } set => CampaignValidator = value; }
    //    // List of Validators that are defined for the Campaign
    //    public IList<IValidator> Validators{ get { return Validators; } set => Validators = value; }
    //    public IController CampaignController{ get { return CampaignController; } set => CampaignController = value; }
    //    public IList<IController> Controllers { get { return Controllers ; } set => Controllers = value; }

    //    /// <summary>
    //    /// Constructor for CampaignConfig
    //    /// The CampaignId is used in the factories to retrieve the controls (i.e. Validators, Controllers (Rules & Filters).
    //    /// </summary>
    //    /// <param name="campaignId"></param>
    //    /// <param name="validatorFactory"></param>
    //    /// <param name="controllerFactory"></param>

    //    /// <param name="loggerClient"></param>
    //    public CampaignConfig(int campaignId, IValidatorFactory validatorFactory, IControllerFactory controllerFactory, ILoggerClient loggerClient)
    //    {
    //        if (campaignId <= 0)
    //        {
    //            throw new ArgumentException($"Error: {solutionContext}: campaignId = {campaignId}");
    //        }
    
    //        //Retrieve the applicable Validators from the Factory for this campaign
    //        var validatorFactoryIn = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
    //        Validators = validatorFactoryIn.BuildCampaignValidators(campaignId);

    //        //Retrieve the applicable Controllers from the Factory for this campaign
    //        var controllerFactoryIn = controllerFactory ?? throw new ArgumentNullException(nameof(controllerFactory));
    //        Controllers = controllerFactoryIn.BuildCampaignControllers(campaignId);
    //        CampaignController = campaignController ?? throw new ArgumentNullException(nameof(campaignController));
    //        _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

    //        if (CampaignValidator == null)
    //            //CampaignValidator = campaignValidator ?? throw new ArgumentNullException(nameof(campaignValidator));
    //            CampaignValidator = new CampaignValidator(Validators, _loggerClient);
    //    }
    //}
}
