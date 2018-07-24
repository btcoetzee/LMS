using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LeadEntity.Interface
{
    public interface IResult
    {
        string Id { get; }
        object Value { get; }
    }
}
