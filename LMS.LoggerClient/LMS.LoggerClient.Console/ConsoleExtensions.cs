using System.Globalization;
using System.Runtime.Serialization;

namespace LMS.LoggerClient.Console
{
    using System;
    using Interface;

    public static class ConsoleExtensions
    {
        private static readonly ColorSet StandardLoggingColors = new ColorSet(ConsoleColor.DarkGreen, ConsoleColor.White);
        private static readonly ColorSet ErrorLoggingColors = new ColorSet(ConsoleColor.Red, ConsoleColor.Yellow);
        private static string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        public static void Log(ILoggerClientObject loggerObject)
        {
            WriteLogEntry(GetLogDateTime() + "\t" +
                loggerObject.SolutionContext + "\t" + loggerObject.ProcessContext + "\t" +
                loggerObject.OperationContext, StandardLoggingColors);
        }

        public static void Log(ILoggerClientErrorObject loggerErrorObject)
        {
            WriteLogEntry(GetLogDateTime() + "\t" +
                loggerErrorObject.SolutionContext + "\t" + loggerErrorObject.ProcessContext + "\t" +
                loggerErrorObject.OperationContext + "\t" + loggerErrorObject.ErrorContext + "\t" +
                loggerErrorObject.Exception, ErrorLoggingColors);
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

    internal struct ColorSet
    {
        internal ConsoleColor ForegroundColor { get; }
        internal ConsoleColor BackgroundColor { get; }

        public ColorSet(ConsoleColor fg, ConsoleColor bg)
        {
            ForegroundColor = fg;
            BackgroundColor = bg;
        }
    }
}
