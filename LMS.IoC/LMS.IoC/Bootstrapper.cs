using System;
using System.Collections.Generic;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Factory.Implementation;
using Compare.Services.LMS.Controls.Factory.Interface;
using Compare.Services.LMS.Controls.Validator.Interface;
using Compare.Services.LMS.Modules.Campaign.Implementation;
using Compare.Services.LMS.Modules.Campaign.Implementation.Config;
using Compare.Services.LMS.Modules.Campaign.Interface;
using Compare.Services.LMS.Modules.CampaignManager.Implementation;
using Compare.Services.LMS.Modules.CampaignManager.Implementation.Config;
using Compare.Services.LMS.Modules.CampaignManager.Implementation.Decorator;
using Compare.Services.LMS.Modules.CampaignManager.Implementation.Publisher;
using Compare.Services.LMS.Modules.CampaignManager.Implementation.Resolver;
using Compare.Services.LMS.Modules.CampaignManager.Implementation.Subscriber;
using Compare.Services.LMS.Modules.CampaignManager.Interface;
using Compare.Services.LMS.Modules.DataProvider.Implementation;
using Compare.Services.LMS.Modules.DataProvider.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Implementation;
using Compare.Services.LMS.Modules.LoggerClient.Interface;
using Compare.Services.LMS.Modules.Preamble.Implementation;
using Compare.Services.LMS.Modules.Preamble.Interface;


namespace LMS.IoC
{
    using System.Linq;
    using Admiral.Components.Instrumentation.Contract;
    using Admiral.Components.Instrumentation.Logging;
    using Compare.Components.Notification.Channels.InProc;
    using Compare.Components.Notification.Contract;
    using Compare.Components.Notification.Publishers;
    using Compare.Components.Notification.Subscribers;
    using Microsoft.Extensions.DependencyInjection;


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
               // .AddLeadValidator()
               // .AddLeadDecorator()
               // .AddLeadPublisher()
                .AddLeadCollector()
                .AddDataProvider()
                .AddFactory()
                .AddCampaignConfig()
                .AddCampaignCollection()
               // .AddCampaignManagerSubscriber()
               // .AddCampaignManagerDecorator()
               // .AddCampaignManagerResolver()
               // .AddCampaignManagerPublisher()
                .AddCampaignManagerConfig()
                .AddCampaignManager();
            //.AddCampaignValidator()
            //.AddCampaignManagerValidatorCollection()
            //.AddCampaignFilter()
            //.AddCampaignRule(); 



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
            var logger = new LoggerClientEventTypeControl();

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
                new LeadValidator(provider.GetRequiredService<IValidatorFactory>(),
                    new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
                        ColorSet.ErrorLoggingColors)),
                new LeadDecorator(
                    new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
                        ColorSet.ErrorLoggingColors)),
                new LeadPublisher(
                    provider.GetRequiredService<IPublisher<ILeadEntity>>(),
                    new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
                        ColorSet.ErrorLoggingColors)),
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }

        public static IServiceCollection AddDataProvider(this IServiceCollection container)
        {
            container.AddSingleton<IValidatorDataProvider>(provider => new ValidatorDataProvider(provider.GetRequiredService<ILoggerClient>()));
            container.AddSingleton<IControllerDataProvider>(provider => new ControllerDataProvider(provider.GetRequiredService<ILoggerClient>()));
            return container;
        }

        public static IServiceCollection AddFactory(this IServiceCollection container)
        {
            container.AddSingleton<IValidatorFactory>(provider => new ValidatorFactory(
                provider.GetRequiredService<IValidatorDataProvider>(),
                provider.GetRequiredService<ILoggerClient>()));

            container.AddSingleton<IControllerFactory>(provider => new ControllerFactory(
                provider.GetRequiredService<IControllerDataProvider>(),
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
            container.AddSingleton<ICampaignManager>(provider => new CampaignManager(1,
                provider.GetRequiredService<ICampaignManagerConfig>(),
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));
              

            return container;
        }

        public static IServiceCollection AddCampaignManagerConfig(this IServiceCollection container)
        {

            //container.AddSingleton<ICampaignManagerConfig>(provider => new CampaignManagerConfig(1,
            //    provider.GetRequiredService<IValidatorFactory>(),
            //    provider.GetRequiredService<ISubscriber>(),
            //    provider.GetRequiredService<ICampaign[]>(),
            //    new CampaignManagerDecorator(provider.GetRequiredService<ILoggerClient>()),
            //    provider.GetRequiredService<IResolver>(),
            //    provider.GetRequiredService<IPublisher>(),
            //    provider.GetRequiredService<ILoggerClient>()));
            container.AddSingleton<ICampaignManagerConfig>(provider => new CampaignManagerConfig(1,
                provider.GetRequiredService<IValidatorFactory>(),
                new CampaignManagerSubscriber(
                    provider.GetRequiredService<ISubscriber<ILeadEntity>>(),
                    new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                        ColorSet.ErrorLoggingColors)),
                new ICampaign[]
                {
                    // Custom color logging
                    new Campaign(1, "Buy Click Campaign", 1,
                        provider.GetRequiredService<ICampaignConfig>(),
                        new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),ColorSet.ErrorLoggingColors)),
                },
                new CampaignManagerDecorator(provider.GetRequiredService<ILoggerClient>()),
                new CampaignManagerResolver(
                    new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                        ColorSet.ErrorLoggingColors)),
                new CampaignManagerPublisher(
                    new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                        ColorSet.ErrorLoggingColors)),
                provider.GetRequiredService<ILoggerClient>()));

            return container;
        }
        public static IServiceCollection AddCampaignManagerSubscriber(this IServiceCollection container)
        {
            //container.AddSingleton<ICampaignManagerSubscriber, CampaignManagerSubscriber>();
            // Custom color logging
            container.AddSingleton<ISubscriber>(provider => new CampaignManagerSubscriber(
                provider.GetRequiredService<ISubscriber<ILeadEntity>>(),
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));
            return container;
        }
        public static IServiceCollection AddCampaignManagerResolver(this IServiceCollection container)
        {
            // container.AddSingleton<ICampaignManagerResolver, CampaignManagerResolver>();
            // Custom color logging
            container.AddSingleton<IResolver>(provider => new CampaignManagerResolver(
               new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }
        public static IServiceCollection AddCampaignManagerPublisher(this IServiceCollection container)
        {
            
            // Custom color logging
            container.AddSingleton<IPublisher>(provider => new CampaignManagerPublisher(
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }
        public static IServiceCollection AddCampaignManagerDecorator(this IServiceCollection container)
        {
            //container.AddSingleton<ICampaignManagerDecorator, CampaignManagerDecorator>();
            // Custom color logging
            container.AddSingleton<IDecorator>(provider => new CampaignManagerDecorator(
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }
        public static IServiceCollection AddCampaignConfig(this IServiceCollection container)
        {

            container.AddSingleton<ICampaignConfig>(provider => new CampaignConfig(1,
                provider.GetRequiredService<IValidatorFactory>(),
                provider.GetRequiredService<IControllerFactory>(),
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),
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
                    new Campaign(1, "Buy Click Campaign", 1, 
                                            provider.GetRequiredService<ICampaignConfig>(),
                                            new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),ColorSet.ErrorLoggingColors)),
                    
                    //new ProspectCampaign(provider.GetServices<ICampaignValidator>().FirstOrDefault(validator => validator is ProspectValidator),
                    //    new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),ColorSet.ErrorLoggingColors))
                    //provider.GetServices<IValidator>().FirstOrDefault(validator => validator is CampaignValidator
        });

            return container;
        }
        //public static IServiceCollection AddCampaignManagerValidatorCollection(this IServiceCollection container)
        //{
        //    container.AddSingleton<ICampaignManagerValidator[]>(provider => new ICampaignManagerValidator[]
        //    {
               
        //        //new CampaignManagerValidator(provider.GetRequiredService<ILoggerClient>())
        //        // Custom color logging
        //        new LMS.CampaignManager.Validator.Implementation.CampaignManagerValidator(new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
        //        ColorSet.ErrorLoggingColors))
        //});

        //    return container;
        //}
        //public static IServiceCollection AddCampaignValidator(this IServiceCollection container)
        //{
        //    //var validatorMock = new Mock<IValidator>();
        //    //validatorMock.Setup(v => v.ValidLead(It.IsAny<ILeadEntity>())).Returns(true);
        //    //var validator = validatorMock.Object;
        //    //container.AddSingleton<IValidator>(validator);
        //    //container.AddSingleton<ICampaignValidator, BuyClickValidator>();

        //    // Custom color logging
        //    container.AddSingleton<IValidator>(provider => new CampaignValidator(
        //        new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),
        //            ColorSet.ErrorLoggingColors)));

        //    // Custom color logging
        //    container.AddSingleton<ICampaignValidator>(provider => new ProspectValidator(
        //        new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),
        //            ColorSet.ErrorLoggingColors)));
        //    return container;
        //}

        //public static IServiceCollection AddCampaignController(this IServiceCollection container)
        //{
          
        //    // Custom color logging
        //    container.AddSingleton<IController>(provider => new CampaignController(
        //        new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),
        //            ColorSet.ErrorLoggingColors)));
            
        //    return container;
        //}

        //public static IServiceCollection AddCampaignRule(this IServiceCollection container)
        //{

        //    // Custom color logging
        //    container.AddSingleton<IRule>(provider => new BuyClickRule(
        //        new CustomColorLoggerClient(new ColorSet(ConsoleColor.Cyan, ConsoleColor.Black),
        //            ColorSet.ErrorLoggingColors)));

        //    return container;
        //}
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
