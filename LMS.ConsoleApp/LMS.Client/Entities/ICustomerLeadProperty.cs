using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Client.Entities
{
    public interface ICustomerLeadProperty
    {
        string Id { get; }
        object Value { get; }
    }
}
