using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LoggerClient.Interface
{
    public interface ILoggerClientErrorObject
    {
 
        // Name of Soluction
        string SolutionContext { get; set; }

        // Name of Process
        string ProcessContext { get; set; }

        // Operation that failed
        string OperationContext { get; set; }

        // Error Message
        string ErrorContext { get; set; }

        // Exeception
        Exception Exception { get; set; }


    }
}
