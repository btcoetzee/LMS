namespace Campaign.Subscriber
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Campaign.Interface;
    public class CampaignSubscriber : ICampaignSubscriber
    {

        private readonly int _id;
        private static readonly Random _random = new Random();

        public CampaignSubscriber(int id)
        {
            _id = id;
        }
        /// <summary>
        /// Receive the lead from the Channel Subscibed to and let the Campaign process it.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string ReceiveLead(string message)
        {
            var result = $"ID: {_id} -> Thread completed.";

            var delay = _random.Next(1000, 5000);

            Thread.Sleep(delay);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Hello from message ID {_id}! I slept for {delay}ms and received: {message}");

            return result;

            // THIS IS WHERE THE CAMPAIGN.PROCESSLEAD WILL BE CALLED.
        }
    }
}
