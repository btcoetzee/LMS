namespace LMS.Decorator.Interface
{
    using LeadEntity.Interface;

    public interface IDecorator
    {
        // Decorate the Lead
        void DecorateLead(ILeadEntity lead);
    }
}
