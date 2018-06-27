

namespace LMS.CampaignManager.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Compare.Components.Notification.Contract;
    using LMS.LoggerClient.Interface;
    using LMS.Campaign.Interface;
    using LMS.CampaignManager.Interface;

    public class CampaignManager: ICampaignManager
    {
        private readonly IList<ICampaignSubscriber> _campaignSubscriberList;
        private readonly ISubscriber<string> _notificationSubscriber;
        private readonly ILoggerClient _loggerClient;
        private static readonly string[] EmptyResultArray = new string[] { };

        public CampaignManager(ISubscriber<string> notificationSubscriber, IList<ICampaignSubscriber> campaignSubscriberList,  ILoggerClient loggerClient)
        {

            _campaignSubscriberList = campaignSubscriberList ?? throw new ArgumentNullException(nameof(campaignSubscriberList)); 
            _notificationSubscriber = notificationSubscriber ?? throw new ArgumentNullException(nameof(notificationSubscriber));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
            _notificationSubscriber.AddOnReceiveActionToChannel(message => ProcessCampaigns(message));
        }

        public string[] ProcessCampaigns(string message)
        {
            // Check that there are Campaigns to be Managed
            if (!_campaignSubscriberList.Any())
                return EmptyResultArray; 

            // 
            var campaignCnt = _campaignSubscriberList.Count;
            var campaignResults = new string[campaignCnt];
            var tasks = new Task<string>[campaignCnt];
            var index = 0;
            foreach (var campaign in _campaignSubscriberList)
            {
                var task = new Task<string>(() => campaign.ReceiveLead(message));
                tasks[index++] = task;
                task.Start();
            }
            for (var i = 0; i < campaignCnt; i++)
            {
                campaignResults[i] = tasks[i].Result;
            }

            return campaignResults;
        }

        //public void AddCampaign(ICampaignSubscriber newCampaingSubscriber)
        //{
        //    _campaignSubscribers.Add(newCampaingSubscriber);
        //}

        //public string[] ProcessCampaigns(string message)
        //{
        //    var threadCount = _campaignSubscribers.Count;
        //    var responses = new string[threadCount];
        //    var tasks = new Task<string>[threadCount];

        //    var index = 0;

        //    var stopwatch = new Stopwatch();
        //    stopwatch.Reset();
        //    stopwatch.Start();

        //    foreach (var campaignSubscriber in _campaignSubscribers)
        //    {
        //        var task = new Task<string>(() => campaignSubscriber.ReceiveLead(message));
        //        tasks[index++] = task;
        //        task.Start();
        //    }

        //    for (var i = 0; i < threadCount; i++)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Magenta;
        //        Console.WriteLine($"Waiting for task ID {i} to complete. (Elapsed time: {stopwatch.ElapsedMilliseconds}ms)");
        //        responses[i] = tasks[i].Result;
        //    }

        //    stopwatch.Stop();

        //    return responses;
        //}

    }
}
