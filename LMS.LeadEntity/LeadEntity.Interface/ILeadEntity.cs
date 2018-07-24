namespace LMS.LeadEntity.Interface
{
    using System;

    public interface ILeadEntity: ILeadEntityImmutable
    {
        IResultCollection ResultCollection { get; set; }

    }
}
