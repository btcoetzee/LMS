using LMS.LeadEntity.Interface;

namespace LMS.LeadDecorator.Implementation
{
    using LMS.Decorator.Interface;
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
