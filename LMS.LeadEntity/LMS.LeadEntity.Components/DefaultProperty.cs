using LMS.LeadEntity.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LeadEntity.Components
{
    public struct DefaultProperty : IProperty
    {
        public string Id { get; }
        public object Value { get; }

        public DefaultProperty(string id, object value)
        {
            Id = id;
            Value = value;
        }
    }
}
