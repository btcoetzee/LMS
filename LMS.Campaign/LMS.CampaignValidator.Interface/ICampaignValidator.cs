namespace LMS.CampaignValidator.Interface
{

    using LMS.Modules.LeadEntity.Interface;

    public interface ICampaignValidator 
    {
        // Using the Immutable LeadEntity Object
        bool ValidLead(ILeadEntityImmutable lead);

    }
}
