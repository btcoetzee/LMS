namespace Rule.Interface
{
    using System;
    using System.IO;

    public interface IRule
    {

        //  Accept Lead and Process
        void ProcessLead(Stream lead);
    }
}
