namespace LeadDecorator
{
    using Decorator.Interface;
    using LeadEntity.Interface;
    using System;

    public class LeadDecorator : IDecorator
    {
        public void DecorateLead(ILeadEntity lead)
        {
            throw new NotImplementedException();
        }
    }
}
