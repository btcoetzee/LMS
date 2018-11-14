using System;
using Compare.Services.LMS.Modules.LeadEntity.Interface;

namespace LMS.LeadDispatcher.Interface
{
    public interface ILeadDispatcher
    {
        void DispatchLead(ILeadEntity leadEntity);
    }
}
