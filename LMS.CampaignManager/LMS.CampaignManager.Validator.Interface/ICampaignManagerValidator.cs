namespace LMS.CampaignManager.Validator.Interface
{
    using LMS.Modules.LeadEntity.Interface;
    using System;
    public interface ICampaignManagerValidator
    {
        bool ValidLead(ILeadEntity lead);

    }
}
