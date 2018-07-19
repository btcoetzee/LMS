namespace LMS.CampaignManager.Interface
{
    using LMS.LeadEntity.Interface;

    public interface ICampaignManager
    {
        // Manage the Campaign
        void CampaignManagerDriver(ILeadEntity leadEntity);

        // Process through all the campaigns
        ILeadEntity[] ProcessCampaigns(ILeadEntity leadEntity);
    }
}

