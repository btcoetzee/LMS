namespace LMS.Decorator.Interface
{
    using System.Collections.Generic;
    using LeadEntity.Interface;

    public interface IDecorator
    {
        // Decorate the Lead
        void DecorateLead(ILeadEntity leadEntity, List<IResult> resultList);
    }
}
