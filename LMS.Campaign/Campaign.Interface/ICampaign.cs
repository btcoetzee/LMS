using System.Collections.Generic;
using Compare.Services.LMS.Modules.LeadEntity.Interface;

namespace Compare.Services.LMS.Campaign.Interface
{
    public interface ICampaign
    {

        int CampaignId { get; set; }
        string CampaignDescription { get; set; }
        int CampaignPriority { get; set; }


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
