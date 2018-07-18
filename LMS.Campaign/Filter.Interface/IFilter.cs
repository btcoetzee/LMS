namespace LMS.Filter.Interface
{
    using LMS.LeadEntity.Interface;
    using System;
    public interface IFilter
    {
        // Process the Lead through Filter
        bool ClearedFilter(ILeadEntity lead);
    }
}
