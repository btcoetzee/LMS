using System.Collections.Generic;

namespace LMS.CampaignManager.Interface
{
    using LMS.LeadEntity.Interface;

    public interface ICampaignManager
    {
        // Manage the Campaign
        void CampaignManagerDriver(ILeadEntity leadEntity);

        // Process through all the campaigns
        List<IResult>[] ProcessCampaigns(ILeadEntity leadEntity);
    }
}

