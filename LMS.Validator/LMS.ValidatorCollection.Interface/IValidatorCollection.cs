namespace LMS.ValidatorCollection.Interface
{
    using LMS.LeadEntity.Interface;
    using System;
    using System.Collections.Generic;

    public interface IValidatorCollection
    {
        // Check if lead is Valid for Processing
        bool ValidLead(ILeadEntity lead);

        string ErrorMessage { get; }

        List<string> ValidatorCollectionClassNameList();
    }
}
