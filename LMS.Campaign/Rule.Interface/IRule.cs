using Compare.Services.LMS.Modules.LeadEntity.Interface;

namespace Compare.Services.LMS.Rule.Interface
{
    public interface IRule
    {
        //  Accept Lead and Process
        bool ConstraintMet(ILeadEntityImmutable leadEntity);
    }
}
