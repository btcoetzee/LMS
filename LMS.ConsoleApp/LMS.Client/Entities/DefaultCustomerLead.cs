using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Client.Entities
{
    public class DefaultCustomerLead : ICustomerLead
    {
        private List<KeyValuePair<string, object>> _customerLeadProperties;

        /// <summary>
        /// Constructor - create Key Value Pair List.
        /// </summary>
        public DefaultCustomerLead()
        {
            _customerLeadProperties = new List<KeyValuePair<string, object>>();
        }


        public DefaultCustomerLead(List<KeyValuePair<string, object>> customerLeadProperties)
        {
            _customerLeadProperties = customerLeadProperties;
        }

        public List<KeyValuePair<string, object>> CustomerLeadProperty
        {
            get => this._customerLeadProperties;
            set => this._customerLeadProperties = value;
        }
    }
}
