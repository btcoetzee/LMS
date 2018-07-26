namespace LMS.CampaignManager.Validator.Interface
{
    using LMS.LeadEntity.Interface;
    using System;
    public interface ICampaignManagerValidator
    {
        bool ValidLead(ILeadEntity lead);

    }
}
