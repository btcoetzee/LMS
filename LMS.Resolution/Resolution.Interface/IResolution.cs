namespace LMS.Resolution.Interface
{
    using LMS.LeadEntity.Interface;
    using System;


    public interface IResolution
    {
        void ResolveLead(ILeadEntity leadEntity);
    }
}
