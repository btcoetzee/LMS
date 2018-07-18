using System.Globalization;
using System.Runtime.Serialization;

namespace LMS.LoggerClient.Console
{
    using System;
    using Interface;

    public static class ConsoleExtensions
    {
        private static string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        public static void Log(ILoggerClientObject loggerObject)
        {
           Log(loggerObject, ColorSet.StandardLoggingColors);
        }

        public static void Log(ILoggerClientObject loggerObject, ColorSet displayColors)
        {
            WriteLogEntry(GetLogDateTime() + "\t" +
                loggerObject.SolutionContext + "\t" + loggerObject.ProcessContext + "\t" +
                loggerObject.OperationContext, displayColors);
        }

        public static void Log(ILoggerClientErrorObject loggerErrorObject)
        {
            Log(loggerErrorObject, ColorSet.ErrorLoggingColors);
        }

        public static void Log(ILoggerClientErrorObject loggerErrorObject, ColorSet displayColors)
        {
            WriteLogEntry(GetLogDateTime() + "\t" +
                loggerErrorObject.SolutionContext + "\t" + loggerErrorObject.ProcessContext + "\t" +
                loggerErrorObject.OperationContext + "\t" + loggerErrorObject.ErrorContext + "\t" +
                loggerErrorObject.Exception, displayColors);
        }

   
        private static void WriteLogEntry(string message, ColorSet logColors)
        {
            var originalColors = new ColorSet(Console.ForegroundColor, Console.BackgroundColor);

            Console.ForegroundColor = logColors.ForegroundColor;
            Console.BackgroundColor = logColors.BackgroundColor;

            Console.WriteLine(message);

            Console.ForegroundColor = originalColors.ForegroundColor;
            Console.BackgroundColor = originalColors.BackgroundColor;
        }
        #region Log Date and Time
        /// <summary>
        /// Return the Date and Time 
        /// </summary>
        /// <returns></returns>
        private static DateTime GetLogDateTime()
        {
            // RoundtripKind - prevents conversion of UTC times to local times
            return DateTime.ParseExact(DateTime.UtcNow.ToString("s"), DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }
        #endregion
    }

    
}
