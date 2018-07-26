namespace LMS.CampaignManager.Decorator.Interface
{
    using System;
    using System.Collections.Generic;
    using LMS.LeadEntity.Interface;

    public interface ICampaignManagerDecorator 
    {
        void DecorateLead(ILeadEntity leadEntity, List<IResult> campaignManagerResultList);
    }
}
