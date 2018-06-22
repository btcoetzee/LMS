namespace Resolution.Interface
{
    using System;
    using LeadEntity.Interface;

    public interface IResolution
    {
        void ResolveLead(ILeadEntity leadEntity);
    }
}
