namespace LMS.CampaignManager.Publisher.Interface
{
    using LMS.LeadEntity.Interface;
    public interface ICampaignManagerPublisher
    {
        void PublishLead(ILeadEntity leadEntity);
    }
}
