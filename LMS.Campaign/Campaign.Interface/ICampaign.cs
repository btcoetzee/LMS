namespace LMS.Campaign.Interface
{
    using LMS.LeadEntity.Interface;
    using System.IO;
    using System.Collections.Generic;

    public interface ICampaign
    {
        string CampaignName { get; }
        int CampaignPriority { get; }

        //  Accept Lead and Process and return the list of results
        List<IResult> ProcessLead(ILeadEntity leadEntity);
    }
}
