namespace LMS.CampaignManager.Resolver.Interface
{
    using System.Collections.Generic;
    using LMS.Modules.LeadEntity.Interface;

    public interface ICampaignManagerResolver
    {
        void ResolveLeads(ILeadEntity leadEntity, List<IResult>[] campaignResultCollection);
    }
}
