using System.Collections.Generic;
using LMS.ClientObject.Interface;

namespace LMS.ClientObject.Implementation
{
    public class DefaultClientObject : IClientObject
    {
        private List<KeyValuePair<string, object>> _clientObjectMembers;

        /// <summary>
        /// Constructor - create Key Value Pair List.
        /// </summary>
        public DefaultClientObject()
        {
            _clientObjectMembers = new List<KeyValuePair<string, object>>();
        }


        public DefaultClientObject(List<KeyValuePair<string, object>> customerLeadProperties)
        {
            _clientObjectMembers = customerLeadProperties;
        }

        public List<KeyValuePair<string, object>> ClientObject
        {
            get => this._clientObjectMembers;
            set => this._clientObjectMembers = value;
        }
    }
}
