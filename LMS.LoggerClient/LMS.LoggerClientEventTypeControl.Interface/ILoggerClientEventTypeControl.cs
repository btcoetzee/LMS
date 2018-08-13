namespace LMS.LoggerClientEventTypeControl.Interface
{
    using System;
    using LMS.LoggerClientEventTypeControl.Interface.Constants;
    public interface ILoggerClientEventTypeControl
    {
        /// <summary>
        /// Return true/false if LoggerClientEventType enabled/disabled
        /// </summary>
        /// <param name="loggingEventType"></param>
        /// <returns></returns>
        bool LoggerEventTypeEnabled(LoggerClientEventType.LoggerClientEventTypes loggingEventType);

        /// <summary>
        /// Set the LoggerClientEventType to be enabled or disabled
        /// </summary>
        /// <param name="loggingEventType"></param>
        /// <param name="setValue"></param>
        void UpdateLoggerEventType(LoggerClientEventType.LoggerClientEventTypes loggingEventType, bool setValue);

    }
}
