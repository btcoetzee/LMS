namespace CampaignManagerSubscriber
{
    using System;
    using System.Threading;
    using CampaignManager.Interface;

    public class CampaignManagerSubscriber : ICampaignManagerSubscriber
    {
        private static readonly Random _random = new Random();
        private readonly ICampaignManager _campaignManager;

        public CampaignManagerSubscriber(ICampaignManager campaignManager)
        {
            _campaignManager = campaignManager;
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
