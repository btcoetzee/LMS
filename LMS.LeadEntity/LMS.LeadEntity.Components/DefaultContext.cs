using LMS.LeadEntity.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LeadEntity.Components
{
    public struct DefaultContext : IContext
    {
        public string Id { get; }
        public object Value { get; }

        public DefaultContext(string id, object value)
        {
            Id = id;
            Value = value;
        }
    }
}
