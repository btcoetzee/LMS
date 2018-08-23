namespace LMS.ResolutionValidator
{
    using LMS.LeadEntity.Interface;
    using LMS.Validator.Interface;
    using System;
 
    public class ResolutionValidator : IValidator
    {

        public bool ValidLead(ILeadEntity lead)
        {
            throw new NotImplementedException();
        }

        public string ErrorMessage { get; }

  
    }
}
