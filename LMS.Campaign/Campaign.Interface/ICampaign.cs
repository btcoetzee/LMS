namespace Campaign.Interface
{
    using LeadEntity.Interface;
    using System.IO;

    public interface ICampaign
    {
        //  Accept Lead and Process
        void ProcessLead(ILeadEntity lead);
    }
}
