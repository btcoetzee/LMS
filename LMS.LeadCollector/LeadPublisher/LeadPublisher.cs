namespace LMS.LeadPublisher.Implementation
{
    using System;
    using Compare.Components.Notification.Contract;
    using LoggerClient.Interface;
    using LeadEntity.Interface;
    using Newtonsoft.Json;
    using Publisher.Interface;

    public class LeadPublisher : IPublisher
    {


       private static ILoggerClient  _loggerClient;
        ILeadEntity _leadEntity;
        private static IPublisher<string> _notificationChannelPublisher;
        private static string solutionContext = "LeadPublisher";

        public LeadPublisher(IPublisher<string> notificationChannelPublisher,  ILoggerClient loggerClient)
        {
            _notificationChannelPublisher = notificationChannelPublisher ?? throw new ArgumentNullException(nameof(notificationChannelPublisher));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

            
        }

        public void PublishLead(ILeadEntity leadEntity)
        {
            string processContext = "PublishLead";

            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Publishing the Lead",
                ProcessContext = processContext,
                SolutionContext = solutionContext
            });

            _leadEntity = leadEntity;



            // Pass leadEntity onto channel to be picked up by Subscribed Campaign Managers
            _notificationChannelPublisher.BroadcastMessage(JsonConvert.SerializeObject(leadEntity));
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




