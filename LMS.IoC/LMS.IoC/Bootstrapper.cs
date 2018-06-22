using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.IoC
{
    using Admiral.Components.Instrumentation.Contract;
    using Admiral.Components.Instrumentation.Logging;
    using Compare.Components.Notification.Channels.InProc;
    using Compare.Components.Notification.Contract;
    using LeadEntity.Interface;
    using Microsoft.Extensions.DependencyInjection;

    public static class Bootstrapper
    {
        public static IServiceCollection BuildUp(this IServiceCollection container)
        {
            container
                .AddLogger()
                .AddNotificationChannel();

            return container;
        }

        public static IServiceCollection AddLogger(this IServiceCollection container)
        {
            var logger = new Logger
            {
                AppDomainName = "Lead Management Service",
                ProcessName = "Lead Management Service"
            };

            container.AddSingleton<ILogger>(logger);

            return container;
        }

        public static IServiceCollection AddNotificationChannel(this IServiceCollection container)
        {
            container.AddSingleton<INotificationChannel<string>>(provider =>
                new InProcNotificationChannel<string>("Lead Channel", provider.GetService<ILogger>()));

            return container;
        }

        public static IServiceCollection
    }
}
