namespace Filter.Interface
{
    using System;
    using System.IO;

    public interface IFilter
    {
        // Process the Lead through Filter
        void ProcessLead(Stream lead);
    }
}
