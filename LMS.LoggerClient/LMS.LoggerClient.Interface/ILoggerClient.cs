namespace LMS.LoggerClient.Interface
{

    using System;
    public interface ILoggerClient
    {
        void Log(ILoggerClientObject loggerObject);

        void Log(ILoggerClientErrorObject loggerErrorObject);
    }
}
