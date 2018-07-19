using System;

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
    using LMS.Publisher.Interface;
    using LMS.Validator.Interface;
    using LMS.LeadValidator.Implementation;
    using LMS.LeadDecorator.Implementation;
    using LMS.LeadPublisher.Implementation;
    using LMS.CampaignManager.Interface;
    using LMS.CampaignManager.Implementation;
    using LMS.CampaignManager.Subscriber.Interface;
    using LMS.CampaignManager.Subscriber.Implementation;
    using LMS.Campaign.Interface;
    using LMS.Campaign.Implementation.BuyClickCampaign;
    using LMS.Campaign.Implementation.BuyClick;
    using LMS.CampaignManager.Resolver.Interface;
    using LMS.CampaignManager.Resolver.Implementation;

    public static class Bootstrapper
    {
        public static IServiceCollection BuildUp(this IServiceCollection container)
        {
            container                
                .AddLogger()
                .AddLoggerClient()
                .AddNotificationChannel()
                .AddNotificationPublisher()
                .AddNotificationSubscriber()
                .AddLeadValidator()
                .AddLeadDecorator()
                .AddLeadPublisher()
                .AddLeadCollector()
                .AddCampaignManagerSubscriber()
                .AddCampaignCollection()
                .AddCampaignManagerResolver()
                .AddCampaignManager();
                

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
            container.AddSingleton<INotificationChannel<ILeadEntity>>(provider =>
                new InProcNotificationChannel<ILeadEntity>("Lead Channel", provider.GetRequiredService<ILogger>()));

            return container;
        }

        public static IServiceCollection AddNotificationSubscriber(this IServiceCollection container)
        {
            container.AddSingleton<ISubscriber<ILeadEntity>>(provider =>
                new Subscriber<ILeadEntity>(provider.GetRequiredService<INotificationChannel<ILeadEntity>>(), true));

            return container;
        }

        public static IServiceCollection AddNotificationPublisher(this IServiceCollection container)
        {
            container.AddSingleton<IPublisher<ILeadEntity>>(provider =>
                new Publisher<ILeadEntity>(provider.GetServices<INotificationChannel<ILeadEntity>>().ToArray(), true));

            //mmm, mate?
            //container.AddSingleton<IPublisher<string>, Publisher<string>>();

            return container;
        }

        public static IServiceCollection AddLeadValidator(this IServiceCollection container)
        {
            //var validatorMock = new Mock<IValidator>();
            //validatorMock.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);

            //var validator = validatorMock.Object;

            //container.AddSingleton<IValidator>(validator);

            container.AddSingleton<IValidator, LeadValidator>();

            return container;
        }

        public static IServiceCollection AddLeadDecorator(this IServiceCollection container)
        {
            //var decoratorMock = new Mock<IDecorator>();
            //decoratorMock.Setup(d => d.DecorateLead(It.IsAny<ILeadEntity>()));

            //var decorator = decoratorMock.Object;

            //container.AddSingleton<IDecorator>(decorator);

            container.AddSingleton<IDecorator, LeadDecorator>();

            return container;
        }

        public static IServiceCollection AddLeadPublisher(this IServiceCollection container)
        {           
            container.AddSingleton<IPublisher, LeadPublisher>();

            return container;
        }

        public static IServiceCollection AddLeadCollector(this IServiceCollection container)
        {

            //container.AddSingleton<ILeadCollector, LeadCollector>();

            // To add the CustomCollerLoggerClient here, each component has the be defined.
            container.AddSingleton<ILeadCollector>(provider => new LeadCollector(
                provider.GetRequiredService<IValidator>(),
                provider.GetRequiredService<IDecorator>(),
                provider.GetRequiredService<IPublisher>(),
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkBlue, ConsoleColor.White),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }

        public static IServiceCollection AddLoggerClient(this IServiceCollection container)
        {
            //The registered type can be changed in the future.
            return container.AddSingleton<ILoggerClient, ConsoleLoggerClient>();
        }

        //public static IServiceCollection AddPublisherNotificationChannel(this IServiceCollection container)
        //{
        //    var notificationPublisher = 

        //    return container;
        //}

        public static IServiceCollection AddCampaignManager(this IServiceCollection container)
        {
            container.AddSingleton<ICampaignManager, CampaignManager>();

            return container;
        }

        public static IServiceCollection AddCampaignManagerSubscriber(this IServiceCollection container)
        {
            container.AddSingleton<ICampaignManagerSubscriber, CampaignManagerSubscriber>();

            return container;
        }

        public static IServiceCollection AddCampaignCollection(this IServiceCollection container)
        {
            container.AddSingleton<ICampaign[]>(provider => new ICampaign[]
            {
                new BuyClickCampaign(provider.GetRequiredService<IValidator>(), provider.GetRequiredService<IDecorator>(), provider.GetRequiredService<ILoggerClient>()) 
            });

            return container;
        }

        public static IServiceCollection AddCampaignManagerResolver(this IServiceCollection container)
        {
            container.AddSingleton<ICampaignManagerResolver, CampaignManagerResolver>();

            return container;
        }
    }
}
