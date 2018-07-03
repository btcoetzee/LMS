namespace LMS.IoC
{
    using System.Linq;
    using Admiral.Components.Instrumentation.Contract;
    using Admiral.Components.Instrumentation.Logging;
    using Compare.Components.Notification.Channels.InProc;
    using Compare.Components.Notification.Contract;
    using Compare.Components.Notification.Publishers;
    using Compare.Components.Notification.Subscribers;
    using Decorator.Interface;
    using LeadCollector.Implementation;
    using LeadCollector.Interface;
    using LeadEntity.Interface;
    using LoggerClient.Console;
    using LoggerClient.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Publisher.Interface;
    using Validator.Interface;

    public static class Bootstrapper
    {
        public static IServiceCollection BuildUp(this IServiceCollection container)
        {
            container
                .AddLogger()
                .AddNotificationChannel()
                .AddLeadValidator()
                .AddLeadDecorator()
                .AddLeadPublisher()
                .AddLeadCollector()
                .AddLoggerClient();

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
                new InProcNotificationChannel<string>("Lead Channel", provider.GetRequiredService<ILogger>()));

            return container;
        }

        public static IServiceCollection AddNotificationSubscriber(this IServiceCollection container)
        {
            container.AddSingleton<ISubscriber<string>>(provider =>
                new Subscriber<string>(provider.GetRequiredService<INotificationChannel<string>>(), true));

            return container;
        }

        public static IServiceCollection AddNotificationPublisher(this IServiceCollection container)
        {
            container.AddSingleton<IPublisher<string>>(provider =>
                new Publisher<string>(provider.GetServices<INotificationChannel<string>>().ToArray(), true));

            return container;
        }

        public static IServiceCollection AddLeadValidator(this IServiceCollection container)
        {
            var validatorMock = new Mock<IValidator>();
            validatorMock.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);

            var validator = validatorMock.Object;

            container.AddSingleton<IValidator>(validator);

            return container;
        }

        public static IServiceCollection AddLeadDecorator(this IServiceCollection container)
        {
            var decoratorMock = new Mock<IDecorator>();
            decoratorMock.Setup(d => d.DecorateLead(It.IsAny<ILeadEntity>()));

            var decorator = decoratorMock.Object;

            container.AddSingleton<IDecorator>(decorator);
            return container;
        }

        public static IServiceCollection AddLeadPublisher(this IServiceCollection container)
        {
            var publisherMock = new Mock<IPublisher>();
            publisherMock.Setup(p => p.PublishLead(It.IsAny<ILeadEntity>()));

            var publisher = publisherMock.Object;

            container.AddSingleton<IPublisher>(publisher);

            return container;
        }

        public static IServiceCollection AddLeadCollector(this IServiceCollection container)
        {
            container.AddSingleton<ILeadCollector>(provider =>
                new LeadCollector(provider.GetRequiredService<IValidator>(), provider.GetRequiredService<IDecorator>(),
                    provider.GetRequiredService<IPublisher>()));

            return container;
        }

        public static IServiceCollection AddLoggerClient(this IServiceCollection container)
        {
            //The registered type can be changed in the future.
            return container.AddSingleton<ILoggerClient, ConsoleLoggerClient>();
        }
    }
}
