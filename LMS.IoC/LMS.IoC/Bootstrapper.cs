using System;
using StackExchange.Redis;

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
    using LoggerClient.Console;
    using LoggerClient.Interface;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using LMS.Campaign.Interface;
    using LMS.Publisher.Interface;
    using LMS.Validator.Interface;
    using LMS.LeadValidator.Implementation;
    using LMS.LeadDecorator.Implementation;
    using LMS.LeadPublisher.Implementation;
    using LMS.CampaignManager.Interface;
    using LMS.CampaignManager.Implementation;
    using LMS.CampaignManager.Subscriber.Interface;
    using LMS.CampaignManager.Subscriber.Implementation;
    using LMS.CampaignManager.Resolver.Interface;
    using LMS.CampaignManager.Resolver.Implementation;
    using LMS.CampaignManager.Decorator.Implementation;
    using LMS.CampaignManager.Decorator.Interface;
    using LMS.CampaignManager.Validator.Implementation;
    using LMS.CampaignManager.Validator.Interface;
    using LMS.CampaignManager.Publisher.Interface;
    using LMS.CampaignManager.Publisher.Implementation;
    using LMS.Campaign.BuyClick.Validator;
    using LMS.Campaign.BuyClick;
    using LMS.CampaignValidator.Interface;
    using LMS.Campaign.Prospect;
    using LMS.Campaign.Prospect.Validator;
    using LMS.Filter.Interface;
    using LMS.Campaign.BuyClick.Filter;
    using LMS.Rule.Interface;
    using LMS.Campaign.BuyClick.Rule;
    using LMS.LoggerClientEventTypeControl.Interface;
    using LMS.ValidatorFactory.Interface;
    using LMS.DataProvider;
    using LMS.ValidatorDataProvider.Interface;
    using LMS.Modules.LeadEntity.Interface;

    public static class Bootstrapper
    {
        public static IServiceCollection BuildUp(this IServiceCollection container)
        {
            container
                .AddLogger()
                .AddLoggerClient()
                .AddLoggerClientEventTypeControl()
                .AddNotificationChannel()
                .AddNotificationPublisher()
                .AddNotificationSubscriber()
                .AddLeadValidator()
                .AddLeadDecorator()
                .AddLeadPublisher()
                .AddLeadCollector()
                .AddValidatorFactory()
                .AddDataProvider()
                .AddCampaignManagerSubscriber()
                .AddCampaignCollection()
                .AddCampaignManagerValidatorCollection()
                .AddCampaignManagerDecorator()
                .AddCampaignManagerResolver()
                .AddCampaignManagerPublisher()
                .AddCampaignManager()
                .AddCampaignValidator()
                .AddCampaignManagerValidatorCollection()
                .AddCampaignFilter()
                .AddCampaignRule(); 

                

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

        public static IServiceCollection AddLoggerClientEventTypeControl(this IServiceCollection container)
        {
            var logger = new LoggerClientEventTypeControl.Implementation.LoggerClientEventTypeControl();

            container.AddSingleton<ILoggerClientEventTypeControl>(logger);

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
           // container.AddSingleton<IValidator, LeadValidator>();

            // Custom color
            container.AddSingleton<IValidator>(provider => new LeadValidator(provider.GetRequiredService<IValidatorFactory>(),
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));


       
            return container;
        }
        public static IServiceCollection AddLeadDecorator(this IServiceCollection container)
        {
            //container.AddSingleton<IDecorator, LeadDecorator>();

            // Custom color
            container.AddSingleton<IDecorator>(provider => new LeadDecorator(
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }
        public static IServiceCollection AddLeadPublisher(this IServiceCollection container)
        {
            // container.AddSingleton<IPublisher, LeadPublisher>();

            // Custom color
            container.AddSingleton<IPublisher>(provider => new LeadPublisher(
                provider.GetRequiredService<IPublisher<ILeadEntity>>(),
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors))); ;
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
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }

        public static IServiceCollection AddDataProvider(this IServiceCollection container)
        {
            container.AddSingleton<IValidatorDataProvider>(provider => new ValidatorDataProvider());

            return container;
        }

        public static IServiceCollection AddValidatorFactory(this IServiceCollection container)
        {
            container.AddSingleton<IValidatorFactory>(provider => new ValidatorFactory(
                provider.GetRequiredService<IValidatorDataProvider>(),
                provider.GetRequiredService<ILoggerClient>()));

            return container;
        }

        public static IServiceCollection AddLoggerClient(this IServiceCollection container)
        {
            //The registered type can be changed in the future.
            return container.AddSingleton<ILoggerClient, ConsoleLoggerClient>();
        }
        public static IServiceCollection AddCampaignManager(this IServiceCollection container)
        {
            // container.AddSingleton<ICampaignManager, CampaignManager>();
            // Custom color logging
            container.AddSingleton<ICampaignManager>(provider => new CampaignManager(
                provider.GetRequiredService<ICampaignManagerSubscriber>(),
                provider.GetRequiredService<ICampaign[]>(),
                provider.GetRequiredService<ICampaignManagerValidator[]>(),
                provider.GetRequiredService<ICampaignManagerDecorator>(),
                provider.GetRequiredService<ICampaignManagerResolver>(),
                provider.GetRequiredService<ICampaignManagerPublisher>(),
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));
              

            return container;
        }
        public static IServiceCollection AddCampaignManagerSubscriber(this IServiceCollection container)
        {
            //container.AddSingleton<ICampaignManagerSubscriber, CampaignManagerSubscriber>();
            // Custom color logging
            container.AddSingleton<ICampaignManagerSubscriber>(provider => new CampaignManagerSubscriber(
                provider.GetRequiredService<ISubscriber<ILeadEntity>>(),
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));
            return container;
        }
        public static IServiceCollection AddCampaignManagerResolver(this IServiceCollection container)
        {
            // container.AddSingleton<ICampaignManagerResolver, CampaignManagerResolver>();
            // Custom color logging
            container.AddSingleton<ICampaignManagerResolver>(provider => new CampaignManagerResolver(
               new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }
        public static IServiceCollection AddCampaignManagerPublisher(this IServiceCollection container)
        {
            
            // Custom color logging
            container.AddSingleton<ICampaignManagerPublisher>(provider => new CampaignManagerPublisher(
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }
        public static IServiceCollection AddCampaignManagerDecorator(this IServiceCollection container)
        {
            //container.AddSingleton<ICampaignManagerDecorator, CampaignManagerDecorator>();
            // Custom color logging
            container.AddSingleton<ICampaignManagerDecorator>(provider => new CampaignManagerDecorator(
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }
        public static IServiceCollection AddCampaignCollection(this IServiceCollection container)
        {
            container.AddSingleton<ICampaign[]>(provider => new ICampaign[]
            {
                    //new BuyClickCampaign(provider.GetRequiredService<BuyClickValidator>(), provider.GetRequiredService<ILoggerClient>()) 
                    //new BuyClickCampaign(provider.GetRequiredService<ICampaignValidator>(), provider.GetRequiredService<ILoggerClient>())


                    // Custom color logging
                    new BuyClickCampaign(provider.GetServices<ICampaignValidator>().FirstOrDefault(validator => validator is BuyClickValidator),
                                            provider.GetRequiredService<IFilter>(),
                                            provider.GetRequiredService<IRule>(),
                                            new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),ColorSet.ErrorLoggingColors)),
                    
                    new ProspectCampaign(provider.GetServices<ICampaignValidator>().FirstOrDefault(validator => validator is ProspectValidator),
                        new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),ColorSet.ErrorLoggingColors))


            });

            return container;
        }
        public static IServiceCollection AddCampaignManagerValidatorCollection(this IServiceCollection container)
        {
            container.AddSingleton<ICampaignManagerValidator[]>(provider => new ICampaignManagerValidator[]
            {
               
                //new CampaignManagerValidator(provider.GetRequiredService<ILoggerClient>())
                // Custom color logging
                new LMS.CampaignManager.Validator.Implementation.CampaignManagerValidator(new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                ColorSet.ErrorLoggingColors))
        });

            return container;
        }
        public static IServiceCollection AddCampaignValidator(this IServiceCollection container)
        {
            //var validatorMock = new Mock<IValidator>();
            //validatorMock.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);
            //var validator = validatorMock.Object;
            //container.AddSingleton<IValidator>(validator);
            //container.AddSingleton<ICampaignValidator, BuyClickValidator>();

            // Custom color logging
            container.AddSingleton<ICampaignValidator>(provider => new BuyClickValidator(
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            // Custom color logging
            container.AddSingleton<ICampaignValidator>(provider => new ProspectValidator(
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));
            return container;
        }

        public static IServiceCollection AddCampaignFilter(this IServiceCollection container)
        {
          
            // Custom color logging
            container.AddSingleton<IFilter>(provider => new BuyClickFilter(
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));
            
            return container;
        }

        public static IServiceCollection AddCampaignRule(this IServiceCollection container)
        {

            // Custom color logging
            container.AddSingleton<IRule>(provider => new BuyClickRule(
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }
        //public static IServiceCollection AddCampaignValidators(this IServiceCollection container)
        //{
        //    container.AddSingleton<BuyClickValidator>(provider => new BuyClickValidator(provider.GetRequiredService<ILoggerClient>()));

        //    return container;
        //}

        //public static IServiceCollection AddCampaignDecorators(this IServiceCollection container)
        //{

        //    container.AddSingleton<BuyClickDecorator>(provider => new BuyClickDecorator(provider.GetRequiredService<ILoggerClient>()));

        //    return container;
        //}
    }
}
