﻿namespace LMS.LeadEntity.Components
{
    using LMS.LeadEntity.Interface;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public struct DefaultResult : IResult
    {
        public string Id { get; }
        public object Value { get; }

        public DefaultResult(string id, object value)
        {
            Id = id;
            Value = value;
        }

    }
}
