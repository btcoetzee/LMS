using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Client.Entities
{
    public interface ICustomerLead
    {
        ICustomerLeadProperty[] CustomerLeadProperty { get; set; }
    }
}
