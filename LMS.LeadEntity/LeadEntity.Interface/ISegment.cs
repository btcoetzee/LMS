namespace LMS.LeadEntity.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    /// <summary>
    /// Defines Segments that the Lead belong to.  
    /// For instance, "HighPOP" or "Homeowner".
    /// </summary>
    public interface ISegment
    {
        string type { get; }
    }
}
