namespace LMS.Campaign.Interface
{
    using LMS.LeadEntity.Interface;
    using System.IO;

    public interface ICampaign
    {
   
        string CampaignName { get; }

        //  Accept Lead and Process
        ILeadEntity ProcessLead(ILeadEntity leadEntity);
    }
}
