namespace LeadDecorator
{
    using Decorator.Interface;
    using LeadEntity.Interface;
    using System;

    public class LeadDecorator : IDecorator
    {
        public void DecorateLead(ILeadEntity lead)
        {
            // Let's decorate here
            //throw new NotImplementedException();
        }
    }
}
