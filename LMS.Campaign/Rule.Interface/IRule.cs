namespace Rule.Interface
{
    using LeadEntity.Interface;
    using System;

    public interface IRule
    {

        //  Accept Lead and Process
        void ProcessLead(ILeadEntity lead);
    }
}
