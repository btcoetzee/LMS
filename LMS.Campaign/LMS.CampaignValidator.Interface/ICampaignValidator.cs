namespace LMS.CampaignValidator.Interface
{

    using Compare.Services.LMS.Modules.LeadEntity.Interface;

    public interface ICampaignValidator 
    {
        // Using the Immutable LeadEntity Object
        bool ValidLead(ILeadEntityImmutable lead);

    }
}
