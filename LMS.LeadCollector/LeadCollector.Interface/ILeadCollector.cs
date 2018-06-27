namespace LMS.LeadCollector.Interface
{
    using LMS.LeadEntity.Interface;
    using System;

    public interface ILeadCollector
    {
        void CollectLead(ILeadEntity lead);
    }
}
