namespace LMS.LoggerClientEventTypeControl.Implementation
{
    using LMS.LoggerClientEventTypeControl.Interface;
    using LMS.LoggerClientEventTypeControl.Interface.Constants;
    using System;
    using System.Collections.Concurrent;

    public class LoggerClientEventTypeControl: ILoggerClientEventTypeControl
    {

        // Create a new concurrent dictionary.
        private static ConcurrentDictionary<LoggerClientEventType.LoggerClientEventTypes, bool>
            _loggerClientEventTypeDict = new ConcurrentDictionary<LoggerClientEventType.LoggerClientEventTypes, bool>();


        public LoggerClientEventTypeControl()
        {
            // TODO Just initialize to true for now
            _loggerClientEventTypeDict.TryAdd(LoggerClientEventType.LoggerClientEventTypes.Debug, true);
            _loggerClientEventTypeDict.TryAdd(LoggerClientEventType.LoggerClientEventTypes.Information, true);
            _loggerClientEventTypeDict.TryAdd(LoggerClientEventType.LoggerClientEventTypes.Warning, true);
            _loggerClientEventTypeDict.TryAdd(LoggerClientEventType.LoggerClientEventTypes.Error, true);
            _loggerClientEventTypeDict.TryAdd(LoggerClientEventType.LoggerClientEventTypes.Fatal, true);

        }

        public bool LoggerEventTypeEnabled(LoggerClientEventType.LoggerClientEventTypes loggingEventType)
        {
            if (_loggerClientEventTypeDict.TryGetValue(loggingEventType, out var returnValue))
                return returnValue;
            else return false;
  
        }

        public void UpdateLoggerEventType(LoggerClientEventType.LoggerClientEventTypes loggingEventType, bool setValue)
        {
            if ((_loggerClientEventTypeDict.TryGetValue(loggingEventType, out var returnValue)))
                _loggerClientEventTypeDict.TryUpdate(loggingEventType, setValue, returnValue);
            
        }

    
    }
}
