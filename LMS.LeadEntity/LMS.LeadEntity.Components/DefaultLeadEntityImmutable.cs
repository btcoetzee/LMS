namespace LMS.LeadEntity.Components
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using LMS.LeadEntity.Interface;

    public class DefaultLeadEntityImmutable : ILeadEntityImmutable
    {
        private readonly ISegment[] _segments;
        private readonly IProperty[] _properties;
        private readonly IContext[] _context;

        public DefaultLeadEntityImmutable(IContext[] context, IProperty[] properties, ISegment[] segments)
        {
            _segments = segments;
            _properties = properties;
            _context = context;
        }

        public IContext[] Context => _context;
        public IProperty[] Properties => _properties;
        public ISegment[] Segments => _segments;
    }
}
