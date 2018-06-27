namespace LMS.Campaign.Interface
{
  
    using System.IO;

    public interface ICampaign
    {
   
        string CampaignName { get; set; }

        //  Accept Lead and Process
        string ProcessLead(string message);
        //string ProcessLead(ILeadEntity lead);  // Eventually will be processing lead
    }
}
