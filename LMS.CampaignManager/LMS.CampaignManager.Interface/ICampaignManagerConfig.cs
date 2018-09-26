using System;
using System.Collections.Generic;
using System.Text;
using Compare.Services.LMS.Campaign.Interface;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Validator.Interface;
namespace Compare.Services.LMS.CampaignManager.Interface
{
    /// <summary>
    /// Interface to define the Configuration that the CampaignManager requires.
    /// These include the wrapper validator, list of validators, subscriber
    /// decorator, wrapper resolver, list of resolvers and publisher
    /// for the Campaign Manager.
    /// </summary>
    public interface ICampaignManagerConfig
    {
        // Return the wrapper validator for the CampaignManager
        IValidator CampaignManagerValidator { get; set; }

        //  Return the list of other validators for the CampaignManager
        IList<IValidator> Validators { get; set; }

        // Return the subscriber for the CampaignManager
        ISubscriber CampaignManagerSubscriber { get; set; }

        // Return the Campaigns managed by the CampaignManager
        ICampaign[] CampaignCollection { get; set; }

        //Return the decorator for the CampaignManager
        IDecorator CampaignManagerDecorator { get; set; }

        // Return the wrapper resolver for the CampaignManager
        IResolver CampaignManagerResolver { get; set; }

        //Return the list of resolvers for the CampaignManager
        IList<IResolver> Resolvers { get; set; }

        // Return the publisher for the CampaignManager
        IPublisher CampaignManagerPublisher { get; set; }
    }
}
