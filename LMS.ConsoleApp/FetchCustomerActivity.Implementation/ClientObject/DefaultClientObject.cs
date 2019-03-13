using System.Collections.Generic;

namespace FetchCustomerActivity.Implementation.ClientObject
{
    public class DefaultClientObject : IClientObject
    {
        private List<KeyValuePair<string, object>> _clientObjectMembers = new List<KeyValuePair<string, object>>();

        /// <summary>
        /// Constructor - create Key Value Pair List.
        /// </summary>
        public DefaultClientObject()
        {
            _clientObjectMembers = new List<KeyValuePair<string, object>>();
        }


        public DefaultClientObject(List<KeyValuePair<string, object>> customerLeadProperties)
        {
            _clientObjectMembers = new List<KeyValuePair<string, object>>();
            foreach (var pair in customerLeadProperties)
            {
                _clientObjectMembers.Add(new KeyValuePair<string, object>(pair.Key, pair.Value));
            }
        }

        public List<KeyValuePair<string, object>> ClientObject
        {
            get => this._clientObjectMembers;
            set => this._clientObjectMembers = value;
        }
    }
}
