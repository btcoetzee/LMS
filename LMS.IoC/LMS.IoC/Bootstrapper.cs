using System;
using System.Collections.Generic;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
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
using Compare.Services.LMS.Modules.CampaignManager.Implementation.Persistor;
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
using LMS.LeadDispatcher.Implementation.Config;
using LMS.LeadDispatcher.Implementation.Decorator;
using LMS.LeadDispatcher.Implementation.Persistor;
using LMS.LeadDispatcher.Implementation.Publisher;
using LMS.LeadDispatcher.Implementation.Subscriber;
using LeadDispatcher = LMS.LeadDispatcher.Implementation.LeadDispatcher;
using LMS.LeadDispatcher.Interface;


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
        static Dictionary<string, INotificationChannel<ILeadEntity>> NotificationChannelDictionary = new Dictionary<string, INotificationChannel<ILeadEntity>>();
        private const string LeadCollectorNotificationChannelKey = "LeadCollectorChannel";
        private const string CampaignManagerNotificationChannelKey = "CampaignManagerChannel";
        static Dictionary<string, ISubscriber<ILeadEntity>> SubscriberDictionary = new Dictionary<string, ISubscriber<ILeadEntity>>();
        private const string LeadCollectorSubscriberKey = "LeadCollectorSubscriber";
        private const string CampaignManagerSubscriberKey = "CampaignManagerSubscriber";
        static Dictionary<string, IPublisher<ILeadEntity>> PublisherDictionary = new Dictionary<string, IPublisher<ILeadEntity>>();
        private const string LeadCollectorPublisherKey = "LeadCollectorPublisher";
        private const string CampaignManagerPublisherKey = "CampaignManagerPublisher";

        //private static INotificationChannel<ILeadEntity> _leadCollectorNotificationChannel;
        //private static ISubscriber<ILeadEntity> _leadCollectorSubscriber;
        //private static IPublisher<ILeadEntity> _leadCollectorPublisher;
        //private static INotificationChannel<ILeadEntity>[] _campaignManagerNotificationChannel;
        //private static ISubscriber<ILeadEntity> _campaignManagerSubscriber;
        //private static IPublisher<ILeadEntity> _campaignManagerPublisher;

        public static IServiceCollection BuildUp(this IServiceCollection container)
        {
            container
                .AddLogger()
                .AddLoggerClient()
                .AddLoggerClientEventTypeControl()
                .AddNotificationChannel()
                .AddNotificationPublisher()
                .AddNotificationSubscriber()
                .AddDataProvider()
                .AddFactory()
                .AddLeadCollector()
                .AddLeadDispatcherConfig()
                .AddLeadDispatcher()
                .AddCampaignConfig()
                .AddCampaignCollection()
                .AddCampaignManagerConfig()
                .AddCampaignManager();
            return container;
        }

        #region Logger
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

        public static IServiceCollection AddLoggerClient(this IServiceCollection container)
        {
            //The registered type can be changed in the future.
            return container.AddSingleton<ILoggerClient, ConsoleLoggerClient>();
        }
        #endregion

        #region NotificationPubSub
        public static IServiceCollection AddNotificationChannel(this IServiceCollection container)
        {
   
            // Two Channels - LeadCollector to CampaignManager AND CampaignManager to LeadDispatcher
            // Create the dictionary entries for Channels
            NotificationChannelDictionary.Add(LeadCollectorNotificationChannelKey,
                new InProcNotificationChannel<ILeadEntity>("Lead Collector Channel",
                    container.BuildServiceProvider().GetService<ILogger>()));
            NotificationChannelDictionary.Add(CampaignManagerNotificationChannelKey,
                new InProcNotificationChannel<ILeadEntity>("Campaign Manager Channel",
                    container.BuildServiceProvider().GetService<ILogger>()));

            // Add the Services
            container.AddSingleton<Dictionary<string, INotificationChannel<ILeadEntity>>>(
                NotificationChannelDictionary);
     


            return container;
        }
        public static IServiceCollection AddNotificationSubscriber(this IServiceCollection container)
        {

            // Two Subscribers - LeadCollector to CampaignManager AND CampaignManager to LeadDispatcher
            // Create the dictionary entries for Subscribers
            SubscriberDictionary.Add(LeadCollectorSubscriberKey,
                new Subscriber<ILeadEntity>(
                    container.BuildServiceProvider().GetService<Dictionary<string, INotificationChannel<ILeadEntity>>>()
                        .FirstOrDefault(p => p.Key == LeadCollectorNotificationChannelKey).Value, true));
            SubscriberDictionary.Add(CampaignManagerSubscriberKey,
                new Subscriber<ILeadEntity>(
                    container.BuildServiceProvider().GetService<Dictionary<string, INotificationChannel<ILeadEntity>>>()
                        .FirstOrDefault(p => p.Key == CampaignManagerNotificationChannelKey).Value, true));

            // Add the Services
            container.AddSingleton<Dictionary<string, ISubscriber<ILeadEntity>>>(
                SubscriberDictionary);

         

            return container;
        }
        public static IServiceCollection AddNotificationPublisher(this IServiceCollection container)
        {
            // Two Publishers - LeadCollector to CampaignManager AND CampaignManager to LeadDispatcher
            // Create the dictionary entries for Publishers
            PublisherDictionary.Add(LeadCollectorPublisherKey,
                new Publisher<ILeadEntity>(
                    container.BuildServiceProvider()
                        .GetServices<Dictionary<string, INotificationChannel<ILeadEntity>>>()
                        .SelectMany(
                            d => d.Where(p => p.Key == LeadCollectorNotificationChannelKey).Select(p => p.Value))
                        .ToArray(), true));

            PublisherDictionary.Add(CampaignManagerPublisherKey,
                new Publisher<ILeadEntity>(
                    container.BuildServiceProvider()
                        .GetServices<Dictionary<string, INotificationChannel<ILeadEntity>>>()
                        .SelectMany(
                            d => d.Where(p => p.Key == CampaignManagerNotificationChannelKey).Select(p => p.Value))
                        .ToArray(), true));
      
            // Add the Services
            container.AddSingleton<Dictionary<string, IPublisher<ILeadEntity>>>(
                PublisherDictionary);

            return container;
        }
        #endregion

        #region LeadCollector
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
                    container.BuildServiceProvider().GetService<Dictionary<string, IPublisher<ILeadEntity>>>()
                        .FirstOrDefault(p => p.Key == LeadCollectorPublisherKey).Value,
                    new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
                        ColorSet.ErrorLoggingColors)),
                new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
                    ColorSet.ErrorLoggingColors)));

            return container;
        }
        #endregion

        #region DataProvider
        public static IServiceCollection AddDataProvider(this IServiceCollection container)
        {
            container.AddSingleton<IControllerDataProvider>(provider => new ControllerDataProvider(provider.GetRequiredService<ILoggerClient>()));
            container.AddSingleton<IResolverDataProvider>(provider => new ResolverDataProvider(provider.GetRequiredService<ILoggerClient>()));
            return container;
        }

        #endregion

        #region Factory
        public static IServiceCollection AddFactory(this IServiceCollection container)
        {
            container.AddSingleton<IValidatorFactory>(provider => new ValidatorFactory(
                provider.GetRequiredService<IControllerDataProvider>(),
                provider.GetRequiredService<ILoggerClient>()));
            container.AddSingleton<IControllerFactory>(provider => new ControllerFactory(
                provider.GetRequiredService<IControllerDataProvider>(),
                provider.GetRequiredService<ILoggerClient>()));
            container.AddSingleton<IResolverFactory>(provider => new ResolverFactory(
                provider.GetRequiredService<IResolverDataProvider>(),
                provider.GetRequiredService<ILoggerClient>()));

            return container;
        }
        #endregion

        #region CampaignManager
        public static IServiceCollection AddCampaignManager(this IServiceCollection container)
        {
            // container.AddSingleton<ICampaignManager, CampaignManager>();
            // Custom color logging
            container.AddSingleton<ICampaignManager>(provider => new CampaignManager(1, "Buy Click Campaign Manager",
                provider.GetRequiredService<ICampaignManagerConfig>()));

            return container;
        }

        public static IServiceCollection AddCampaignManagerConfig(this IServiceCollection container)
        {

            container.AddSingleton<ICampaignManagerConfig>(provider => new CampaignManagerConfig(1,
                provider.GetRequiredService<IValidatorFactory>(),
                provider.GetRequiredService<IResolverFactory>(),
                new CampaignManagerSubscriber(
                    //provider.GetRequiredService<ISubscriber<ILeadEntity>>(),
                    container.BuildServiceProvider().GetService<Dictionary<string, ISubscriber<ILeadEntity>>>()
                        .FirstOrDefault(p => p.Key == LeadCollectorSubscriberKey).Value,
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
                new CampaignManagerPersistor(new FileLoggerClient("CMLog.txt", "CMErrorLog.txt")),
                new CampaignManagerPublisher(container.BuildServiceProvider().GetService<Dictionary<string, IPublisher<ILeadEntity>>>()
                        .FirstOrDefault(p => p.Key == CampaignManagerPublisherKey).Value, 
                    new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                        ColorSet.ErrorLoggingColors)),
                provider.GetRequiredService<ILoggerClient>()));

            return container;
        }

        #endregion

        #region LeadDispatcher
        public static IServiceCollection AddLeadDispatcherConfig(this IServiceCollection container)
        {
            container.AddSingleton<ILeadDispatcherConfig>(provider => new LeadDispatcherConfig(1,
                new LeadDispatcherSubscriber(container.BuildServiceProvider().GetService<Dictionary<string, ISubscriber<ILeadEntity>>>()
                        .FirstOrDefault(p => p.Key == LeadCollectorSubscriberKey).Value,
                    new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
                        ColorSet.ErrorLoggingColors)),
                provider.GetRequiredService<IResolverFactory>(),
                new LeadDispatcherPublisher(provider.GetRequiredService<ILoggerClient>()),
                new LeadDispatcherDecorator(provider.GetRequiredService<ILoggerClient>()),
                //   new LeadDispatcherPersistor(provider.GetRequiredService<ILoggerClient>()),
                //   Instead let the persister write out the LeadEntityObject to a file for now.
                new LeadDispatcherPersistor(new FileLoggerClient("LDLog.txt", "LDErrorLog.txt")),
                provider.GetRequiredService<ILoggerClient>()));

            return container;
        }
        public static IServiceCollection AddLeadDispatcher(this IServiceCollection container)
        {
            container.AddSingleton<ILeadDispatcher>(provider => new LeadDispatcher.Implementation.LeadDispatcher(1, "BuyClick Dispatcher",
                provider.GetRequiredService<ILeadDispatcherConfig>()));
            return container;
        }


        #endregion


        #region Campaign
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
        #endregion

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
        //public static IServiceCollection AddLeadValidator(this IServiceCollection container)
        //{
        //    // container.AddSingleton<IValidator, LeadValidator>();

        //    // Custom color
        //    container.AddSingleton<IValidator>(provider => new LeadValidator(provider.GetRequiredService<IValidatorFactory>(),
        //        new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
        //            ColorSet.ErrorLoggingColors)));



        //    return container;
        //}
        //public static IServiceCollection AddLeadDecorator(this IServiceCollection container)
        //{
        //    //container.AddSingleton<IDecorator, LeadDecorator>();

        //    // Custom color
        //    container.AddSingleton<IDecorator>(provider => new LeadDecorator(
        //        new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
        //            ColorSet.ErrorLoggingColors)));

        //    return container;
        //}
        //public static IServiceCollection AddLeadPublisher(this IServiceCollection container)
        //{
        //    // container.AddSingleton<IPublisher, LeadPublisher>();

        //    // Custom color
        //    container.AddSingleton<IPublisher>(provider => new LeadPublisher(
        //        provider.GetRequiredService<IPublisher<ILeadEntity>>(),
        //        new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkYellow, ConsoleColor.Black),
        //            ColorSet.ErrorLoggingColors))); ;
        //    return container;
        //}
        //public static IServiceCollection AddCampaignManagerSubscriber(this IServiceCollection container)
        //{
        //    //container.AddSingleton<ICampaignManagerSubscriber, CampaignManagerSubscriber>();
        //    // Custom color logging
        //    container.AddSingleton<ISubscriber>(provider => new CampaignManagerSubscriber(
        //        provider.GetRequiredService<ISubscriber<ILeadEntity>>(),
        //        new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
        //            ColorSet.ErrorLoggingColors)));
        //    return container;
        //}
        //public static IServiceCollection AddCampaignManagerResolver(this IServiceCollection container)
        //{
        //    // container.AddSingleton<ICampaignManagerResolver, CampaignManagerResolver>();
        //    // Custom color logging
        //    container.AddSingleton<IResolver>(provider => new CampaignManagerResolver(
        //       new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
        //            ColorSet.ErrorLoggingColors)));

        //    return container;
        //}
        //public static IServiceCollection AddCampaignManagerPublisher(this IServiceCollection container)
        //{

        //    // Custom color logging
        //    container.AddSingleton<IPublisher>(provider => new CampaignManagerPublisher(
        //        new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
        //            ColorSet.ErrorLoggingColors)));

        //    return container;
        //}
        //public static IServiceCollection AddCampaignManagerDecorator(this IServiceCollection container)
        //{
        //    //container.AddSingleton<ICampaignManagerDecorator, CampaignManagerDecorator>();
        //    // Custom color logging
        //    container.AddSingleton<IDecorator>(provider => new CampaignManagerDecorator(
        //        new CustomColorLoggerClient(new ColorSet(ConsoleColor.DarkGray, ConsoleColor.Black),
        //            ColorSet.ErrorLoggingColors)));

        //    return container;
        //}
        //container.AddSingleton<INotificationChannel<ILeadEntity>>(provider =>
        //    new InProcNotificationChannel<ILeadEntity>("Lead Collector Channel", provider.GetRequiredService<ILogger>()));
        //container.AddSingleton<INotificationChannel<ILeadEntity>>(provider =>
        //    _leadCollectorNotificationChannel[0] = new InProcNotificationChannel<ILeadEntity>("Lead Collector Channel", provider.GetRequiredService<ILogger>()));
        //container.AddSingleton<INotificationChannel<ILeadEntity>>(provider =>
        //    _campaignManagerNotificationChannel[0] = new InProcNotificationChannel<ILeadEntity>("Campaign Manager Channel", provider.GetRequiredService<ILogger>()));
        //container.AddSingleton<INotificationChannel<ILeadEntity>>(provider =>
        //    NotificationChannelDictionary.FirstOrDefault(p => p.Key == "LeadCollectorChannel").Value);
        //container.AddSingleton<INotificationChannel<ILeadEntity>>(provider =>
        //    NotificationChannelDictionary.FirstOrDefault(p => p.Key == "CampaignManagerChannel").Value);

    }
}
