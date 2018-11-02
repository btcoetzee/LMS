using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Client.Entities
{
    public interface ICustomerLead
    {
        List<KeyValuePair<string, object>> CustomerLeadProperty { get; set; }
    }
}
