namespace LMS.LeadEntity.Components
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using LMS.LeadEntity.Interface;
    public class DefaultLeadEntity : ILeadEntity
    {
        public IContext[] Context { get; set; }
        public IProperty[] Properties { get; set; }
        public ISegment[] Segments { get; set; }
        public IResultCollection ResultCollection { get; set; }
    }
}
