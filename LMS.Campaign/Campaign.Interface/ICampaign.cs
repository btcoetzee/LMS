namespace Campaign.Interface
{
    using System.IO;

    public interface ICampaign
    {
        //  Accept Lead and Process
        void ProcessLead(Stream lead);
    }
}
