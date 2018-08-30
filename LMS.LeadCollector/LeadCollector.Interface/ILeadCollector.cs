namespace LMS.LeadCollector.Interface
{
    using LMS.Modules.LeadEntity.Interface;
    using System;

    public interface ILeadCollector
    {
        void CollectLead(ILeadEntity lead);
    }
}
