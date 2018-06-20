namespace LMS.CampaignManager
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System;
    using Campaign.Interface;
    using Compare.Components.Notification.Contract;
    using System.Collections.Generic;

    public class CampaignManager
    {
        private readonly List<ICampaignSubscriber> _campaignSubscribers;
        private readonly ISubscriber<string> _notificationSubscriber;

        public CampaignManager(ISubscriber<string> notificationSubscriber)
        {
            _campaignSubscribers = new List<ICampaignSubscriber>();
            _notificationSubscriber = notificationSubscriber;
            _notificationSubscriber.AddOnReceiveActionToChannel(message => ProcessCampaigns(message));
        }

        public void AddCampaign(ICampaignSubscriber newCampaingSubscriber)
        {
            _campaignSubscribers.Add(newCampaingSubscriber);
        }

        public string[] ProcessCampaigns(string message)
        {
            var threadCount = _campaignSubscribers.Count;
            var responses = new string[threadCount];
            var tasks = new Task<string>[threadCount];

            var index = 0;

            var stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();

            foreach (var campaignSubscriber in _campaignSubscribers)
            {
                var task = new Task<string>(() => campaignSubscriber.ReceiveLead(message));
                tasks[index++] = task;
                task.Start();
            }

            for (var i = 0; i < threadCount; i++)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Waiting for task ID {i} to complete. (Elapsed time: {stopwatch.ElapsedMilliseconds}ms)");
                responses[i] = tasks[i].Result;
            }

            stopwatch.Stop();

            return responses;
        }

    }
}
