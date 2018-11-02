using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Client.Entities
{
    public class DefaultCustomerLead : ICustomerLead
    {
        private ICustomerLeadProperty[] _customerLeadProperties;

        public DefaultCustomerLead(ICustomerLeadProperty[] customerLeadProperties)
        {
            _customerLeadProperties = customerLeadProperties;
        }

        public ICustomerLeadProperty[] CustomerLeadProperty
        {
            get { return this._customerLeadProperties; }
            set { this._customerLeadProperties = value; }
        }
    }
}
