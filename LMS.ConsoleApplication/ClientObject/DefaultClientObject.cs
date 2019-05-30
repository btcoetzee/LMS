using System.Collections.Generic;

namespace LMS.ConsoleApplication.ClientObject
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

            if (_clientObjectMembers == null)
            {
                _clientObjectMembers = new List<KeyValuePair<string, object>>();
            }
           
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
