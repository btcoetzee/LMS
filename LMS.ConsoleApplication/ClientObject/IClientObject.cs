using System.Collections.Generic;

namespace LMS.ConsoleApplication.ClientObject
{
    public interface IClientObject
    {
        List<KeyValuePair<string, object>> ClientObject { get; set; }
    }
}
