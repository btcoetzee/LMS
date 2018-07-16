namespace LMS.CampaignManager.Subscriber.Implementation
{
    using System;
    using Compare.Components.Notification.Contract;
    using LMS.CampaignManager.Interface;
    using LMS.CampaignManager.Subscriber.Interface;
    using LMS.LoggerClient.Interface;

    public class CampaignManagerSubscriber : ICampaignManagerSubscriber
    {
        private readonly ICampaignManager _campaignManager;
        private readonly ISubscriber<string> _notificationSubscriber;
        private readonly ILoggerClient _loggerClient;

        public CampaignManagerSubscriber(ISubscriber<string> notificationSubscriber, ICampaignManager campaignManager, ILoggerClient loggerClient)
        {
            _notificationSubscriber = notificationSubscriber ?? throw new ArgumentNullException(nameof(notificationSubscriber));
            _notificationSubscriber.AddOnReceiveActionToChannel(message => ReceiveLead(message));
            _campaignManager = campaignManager ?? throw new ArgumentNullException(nameof(campaignManager));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }
        public void ReceiveLead(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Hello from campaignManager. Received: {message}");

            // No Call the Campaign Manager to Process
            _campaignManager.ProcessCampaigns(message);
        }
    }
}
