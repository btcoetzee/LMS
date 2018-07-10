using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LoggerClient.Interface
{
    public struct DefaultLoggerClientErrorObject : ILoggerClientErrorObject
    {
        public string SolutionContext { get; set; }
        public string ProcessContext { get; set; }
        public string OperationContext { get; set; }
        public string ErrorContext { get; set; }
        public Exception Exception { get; set; }
    }
}
