namespace LMS.LeadDecorator.Implementation
{
    using LMS.LeadEntity.Interface;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Components;
    using System;
    using System.Linq;
    using LMS.LeadEntity.Interface.Constants;

    public class LeadDecorator : IDecorator
    {
        public void DecorateLead(ILeadEntity lead)
        {
            if (lead.Results == null)
                lead.Results = new IResults[0];

            var resultsList = lead.Results.ToList();

            resultsList.Add(new DefaultResult(ResultKeys.ResultTimeStampKey, DateTime.Now));
        }
    }
}
