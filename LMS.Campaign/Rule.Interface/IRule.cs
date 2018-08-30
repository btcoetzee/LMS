namespace LMS.Rule.Interface
{
    using LMS.Modules.LeadEntity.Interface;
    using System;

    public interface IRule
    {
        //  Accept Lead and Process
        bool ValidateForRule(ILeadEntityImmutable leadEntity);
    }
}
