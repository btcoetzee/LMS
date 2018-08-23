namespace LMS.Validator.Interface
{
    using System;
    using LMS.LeadEntity.Interface;

    public interface IValidator
    {
        // Check if lead is Valid for Processing
        bool ValidLead(ILeadEntity lead);
 
    }
}
