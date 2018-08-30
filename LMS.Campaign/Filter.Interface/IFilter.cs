namespace LMS.Filter.Interface
{

    using LMS.Modules.LeadEntity.Interface;
    using System;
    public interface IFilter
    {
        // Process the Lead through Filter
        bool ClearedFilter(ILeadEntityImmutable lead);
    }
}
