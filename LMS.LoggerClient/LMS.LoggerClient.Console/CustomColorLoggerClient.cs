using System;
using System.Collections.Generic;
using System.Text;
using LMS.LoggerClient.Interface;

namespace LMS.LoggerClient.Console
{
    public class CustomColorLoggerClient : ILoggerClient
    {
        private readonly ColorSet _standardLoggingColors;
        private readonly ColorSet _errorLoggingColors;

        public CustomColorLoggerClient(ColorSet standardLoggingColors, ColorSet errorLoggingColors)
        {
            _standardLoggingColors = standardLoggingColors;
            _errorLoggingColors = errorLoggingColors;
        }


        public void Log(ILoggerClientObject loggerObject)
        {
            ConsoleExtensions.Log(loggerObject, _standardLoggingColors);
        }

        public void Log(ILoggerClientErrorObject loggerErrorObject)
        {
            ConsoleExtensions.Log(loggerErrorObject, _errorLoggingColors);
        }
    }
}
