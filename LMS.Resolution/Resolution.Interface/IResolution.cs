namespace LMS.Resolution.Interface
{
    using LMS.Modules.LeadEntity.Interface;
    using System;


    public interface IResolution
    {
        void ResolveLead(ILeadEntity leadEntity);
    }
}
