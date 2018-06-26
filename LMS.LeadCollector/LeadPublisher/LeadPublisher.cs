namespace LeadPublisher
{
    using LeadEntity.Interface;
    using Publisher.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Admiral.Components.Instrumentation.Contract;
    using Compare.Components.Notification.Contract;
    using LMS.LoggerClient.Interface;
    using Campaign.Interface;

    public class LeadPublisher : IPublisher
    {
        ILoggerClient  _loggerClient;
        ILeadEntity _leadEntity;
        private static INotificationChannel<string> _notificationChannel; 

        public LeadPublisher(INotificationChannel<string> notificationChannel,  ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient;
            _notificationChannel = notificationChannel;
        }

        public void PublishLead(ILeadEntity leadEntity)
        {
            _leadEntity = leadEntity;

            // Pass leadEntity onto channel to be picked up by Subscribed Campaign Managers
            _notificationChannel.Transmit("message");
        }


        //const int intialCampaigns = 3;
            //const int additionalCampaigns = 2;

            //for (var i = 0; i < intialCampaigns; i++)
            //    _campaignManager.AddCampaign((new CampaignSubscriber(i)));

            //Console.ForegroundColor = ConsoleColor.Cyan;
            //Console.WriteLine($"Manager preloaded with {intialCampaigns}.\n");

            //var resultData = _campaignManager.ProcessCampaigns("Start your engines!");

            //Console.ForegroundColor = ConsoleColor.Yellow;
            //foreach (var result in resultData)
            //    Console.WriteLine(result);

            //Console.ForegroundColor = ConsoleColor.Cyan;
            //Console.WriteLine($"\nAdding {additionalCampaigns} more.\n");

            //for (var i = 0; i < additionalCampaigns; i++)
            //    _campaignManager.AddCampaign(new CampaignSubscriber(i + intialCampaigns));

            //resultData = _campaignManager.ProcessCampaigns("Start your engines for more campaigns!");

            //Console.ForegroundColor = ConsoleColor.Yellow;
            //foreach (var result in resultData)
            //    Console.WriteLine(result);


            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine("\nPress any key to quit...");
            //Console.ReadKey();

        //}

    }
}




