namespace Campaign.Subscriber
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Campaign.Interface;
    class Subscriber : ICampaignSubscriber
    {

        private readonly int _id;
        private static readonly Random _random = new Random();

        public Subscriber(int id)
        {
            _id = id;
        }
        public string ReceiveLead(string message)
        {
            var result = $"ID: {_id} -> Thread completed.";

            var delay = _random.Next(1000, 5000);

            Thread.Sleep(delay);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Hello from message ID {_id}! I slept for {delay}ms and received: {message}");

            return result;

            /// THIS IS WHERE THE CAMPAIGN.PROCESSLEAD WILL BE CALLED.
        }
    }
}
