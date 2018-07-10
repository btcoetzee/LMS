using System;
using System.Globalization;

namespace LMS.LoggerClient.File
{
    using LMS.LoggerClient.Interface;
    using System;
    using System.IO;

    public class FileLoggerClient : ILoggerClient
    {

        string LogFilename = "C:\\null\\LMSLoggingForNow.txt";
        string ErrorLogFilename = "C:\\null\\LMSErrorLoggingForNow.txt";
        string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
       // DateTime.ParseExact(internalBrandResult.ReceivedDateTime.ToString("s"),"yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)

        /// <summary>
        /// Log Output to the LogFile
        /// </summary>
        /// <param name="loggerObject"></param>
        public void Log(ILoggerClientObject loggerObject)
        {
            
            // Append if File Exists
            using (FileStream logFile = new FileStream(LogFilename, FileMode.Append, FileAccess.Write))
            using (StreamWriter streamWriter = new StreamWriter(logFile))
            {
                streamWriter.WriteLine(GetLogDateTime() + "\t" + loggerObject.SolutionContext + "\t" + loggerObject.ProcessContext + "\t" + loggerObject.OperationContext);
                streamWriter.Close();
            }
        }

        public void Log(ILoggerClientErrorObject loggerErrorObject)
        {
            // Append if File Exists
            using (FileStream logFile = new FileStream(ErrorLogFilename, FileMode.Append, FileAccess.Write))
            using (StreamWriter streamWriter = new StreamWriter(logFile))
            {
                streamWriter.WriteLine(GetLogDateTime() + "\t" + loggerErrorObject.SolutionContext + "\t" + loggerErrorObject.ProcessContext + "\t" + loggerErrorObject.OperationContext + "\t" + loggerErrorObject.ErrorContext + "\t" + loggerErrorObject.Exception);
                streamWriter.Close();
            }
        }

        #region Log Date and Time
        /// <summary>
        /// Return the Date and Time 
        /// </summary>
        /// <returns></returns>
        private DateTime GetLogDateTime()
        {
            return DateTime.ParseExact(DateTime.UtcNow.ToString("s"), DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
        }
        #endregion
 
    }
}
