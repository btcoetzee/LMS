namespace LMS.Filter.Interface
{
    using LMS.LeadEntity.Interface;
    using System;
    public interface IFilter
    {
        // Process the Lead through Filter
        void ProcessLead(ILeadEntity lead);
    }
}
