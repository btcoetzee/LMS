namespace LMS.Campaign.Interface
{
    using LMS.Modules.LeadEntity.Interface;
    using System.IO;
    using System.Collections.Generic;

    public interface ICampaign
    {
        string CampaignName { get; }
        int CampaignPriority { get; }
       int CampaignId { get; }

        //  Accept Lead and Process and return the list of results
        /// <summary>
        /// Accept Lead and Process it through the Campaign and return the list of results.
        /// Campaigns have Immutable Lead Object as the object will not be updated within the Campaigns.
        /// The Results are returned to the Campaign Manager and it will update the Object as required.
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        List<IResult> ProcessLead(ILeadEntityImmutable leadEntity);
    }
}
