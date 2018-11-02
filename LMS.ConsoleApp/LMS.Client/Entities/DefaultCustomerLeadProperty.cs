using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Client.Entities
{
    public class DefaultCustomerLeadProperty : ICustomerLeadProperty
    {
        public DefaultCustomerLeadProperty(string id, object value)
        {
            Id = id;
            Value = value;
        }

        public string Id { get; }
        public object Value { get; }
    }
}
