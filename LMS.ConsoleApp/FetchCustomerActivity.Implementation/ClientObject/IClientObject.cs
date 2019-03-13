using System.Collections.Generic;

namespace FetchCustomerActivity.Implementation.ClientObject
{
    public interface IClientObject
    {
        List<KeyValuePair<string, object>> ClientObject { get; set; }
    }
}
