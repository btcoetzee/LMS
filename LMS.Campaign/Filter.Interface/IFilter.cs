

namespace Compare.Services.LMS.Filter.Interface
{
    public interface IFilter
    {
        // Process the Lead through Filter
        bool ConstraintMet(ILeadEntityImmutable lead);
    }
}
