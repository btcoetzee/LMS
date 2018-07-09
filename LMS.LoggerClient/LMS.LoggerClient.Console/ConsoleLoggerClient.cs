namespace LMS.LoggerClient.Console
{
    using Interface;

    public class ConsoleLoggerClient : ILoggerClient
    {
        public void Log(ILoggerClientObject loggerObject)
        {
            ConsoleExtensions.Log(loggerObject);
        }

        public void Log(ILoggerClientErrorObject loggerErrorObject)
        {
            ConsoleExtensions.Log(loggerErrorObject);
        }
    }
}
