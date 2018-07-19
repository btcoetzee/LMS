namespace LMS.CampaignManager.Resolver.Interface
{
    using LMS.LeadEntity.Interface;
    public interface ICampaignManagerResolver
    {
        void ResolveLeads(ILeadEntity[] leadCollection);
    }
}
