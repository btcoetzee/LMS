using System;
using System.Collections.Generic;

namespace LMS.ClientObject.Interface
{
    public interface IClientObject
    {
        List<KeyValuePair<string, object>> ClientObject { get; set; }
    }
}
