using System.Collections.Generic;
using Compare.Services.LMS.Modules.LeadEntity.Interface;

namespace Compare.Services.LMS.CampaignManager.Interface
{
    /// <summary>
    /// Interface that defines a Campaign Manager
    /// </summary>
    public interface ICampaignManager
    {

        // Unique Id Assigned to the Campaign Manager
        int CampaignManagerId { get; set; }

        // Description for the Campaign Manager
        string CampaignManagerDescription { get; set; }

        // Manage the Campaigns - Driver function to kick off the processing of the Campaigns
        void CampaignManagerDriver(ILeadEntity leadEntity);

        // Process through all the campaigns - 
        // The result list returned, shows status of the lead following its processing through the campaign
        IList<IResult>[] ProcessCampaigns(ILeadEntity leadEntity);
    }
}

