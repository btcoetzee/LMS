namespace LMS.CampaignManager.Publisher.Interface
{
    using LMS.Modules.LeadEntity.Interface;

    public interface ICampaignManagerPublisher
    {
        void PublishLead(ILeadEntity leadEntity);
    }
}
