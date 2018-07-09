using LMS.LeadEntity.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LeadEntity.Components
{
    public struct DefaultSegment : ISegment
    {
        public string type { get; }
        public DefaultSegment(string Type)
        {
            type = Type;
        }
    }
}
