using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LoggerClient.Interface
{
    public struct DefaultLoggerClientObject : ILoggerClientObject
    {
        public string SolutionContext { get; set; }
        public string ProcessContext { get; set; }
        public string OperationContext { get; set; }
    }
}
