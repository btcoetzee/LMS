using System.Collections.Generic;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Validator.Interface;

namespace Compare.Services.LMS.CampaignConfig.Interface
{
    /// <summary>
    /// Interface to define the Configuration that the Campaign Needs
    /// These include the Validators and Controllers (Filters and Rules)
    /// </summary>
    public interface ICampaignConfig
    {
        IValidator CampaignValidator { get; set; }
        IList<IValidator> Validators { get; set; }

        IController CampaignController { get; set; }
        IList<IController> Controllers { get; set;  }
    }
}
