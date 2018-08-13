namespace LMS.LoggerClient.Console
{
    using Interface;
    using System;
    using LMS.LoggerClientEventTypeControl.Interface;
    using LMS.LoggerClientEventTypeControl.Interface.Constants;

    public class ConsoleLoggerClient : ILoggerClient
    {
        private readonly ILoggerClientEventTypeControl _loggerClientEventTypeControl;

        public ConsoleLoggerClient(ILoggerClientEventTypeControl loggerClientEventTypeControl)
        {
            _loggerClientEventTypeControl = loggerClientEventTypeControl ?? throw new ArgumentNullException(nameof(loggerClientEventTypeControl)); ;
        }
        public void Log(ILoggerClientObject loggerObject)
        {

            //Check that level of logging is enabled
            if (_loggerClientEventTypeControl.LoggerEventTypeEnabled(loggerObject.EventType))
            {
                ConsoleExtensions.Log(loggerObject);
            }
        }

        public void Log(ILoggerClientErrorObject loggerErrorObject)
        {
            //Check that level of logging is enabled
            if (_loggerClientEventTypeControl.LoggerEventTypeEnabled(loggerErrorObject.EventType))
            {
                ConsoleExtensions.Log(loggerErrorObject);
            }
        }
    }
}
