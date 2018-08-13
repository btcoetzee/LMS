namespace LMS.LoggerClient.Interface
{
    using LMS.LoggerClientEventTypeControl.Interface.Constants;

    public interface ILoggerClientObject
    {
       // Name of Soluction
        string SolutionContext { get; set; }

        // Name of Process
        string ProcessContext { get; set; }

        // Operation being logged
        string OperationContext { get; set; }

        // EventType being logged
        LoggerClientEventType.LoggerClientEventTypes EventType { get; set; }
    }

}
