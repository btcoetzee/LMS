namespace Validator.Interface
{
    using System;
    using System.IO;
    public interface IValidator
    {
        // Check if lead is Valid for Processing
        bool ValidLead(Stream lead);
    }
}
