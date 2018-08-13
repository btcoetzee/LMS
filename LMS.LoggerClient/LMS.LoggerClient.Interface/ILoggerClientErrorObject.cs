namespace LMS.LoggerClient.Interface
{
    using System;
    using LMS.LoggerClientEventTypeControl.Interface.Constants;

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

        // EventType being logged
        LoggerClientEventType.LoggerClientEventTypes EventType { get; set; }

    }
}
