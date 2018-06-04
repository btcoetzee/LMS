namespace Validator.Interface
{
    using LeadEntity.Interface;
    using System;

    public interface IValidator
    {
        // Check if lead is Valid for Processing
        bool ValidLead(ILeadEntity lead);
    }
}
