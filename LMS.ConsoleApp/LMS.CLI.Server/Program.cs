namespace LMS.CLI.Server
{
    using Modules.LeadEntity.Interface;
    using System;
    using System.Linq;
    using System.Threading;
    using Compare.Components.Notification.Contract;
    using Compare.Components.Notification.NotificationChannels.Redis;
    using Compare.Components.Notification.Subscribers;
    using LeadCollector.Interface;
    using Modules.LeadEntity.Components;
    using Modules.LeadEntity.Interface.Constants;
    using Newtonsoft.Json;

    public class Program
    {
        private static readonly MockedLeadCollector LeadCollector = new MockedLeadCollector();
        private static bool _keepProcessing = true;
        private static int _leadsReceived = 0;
        private static ISubscriber<string> _leadSubscriber;

        public static void Main(string[] args)
        {
            _leadSubscriber = new Subscriber<string>(new RedisNotificationChannel("LMS", "Redis", "LMS"), true);
            _leadSubscriber.AddOnReceiveActionToChannel(message =>
            {
                var deserializedEntity = JsonConvert.DeserializeObject<DefaultLeadEntity>(message);
                LeadCollector.CollectLead(deserializedEntity);
            });
            _leadSubscriber.AddOnReceiveActionToChannel(_ => _leadsReceived++);
            _leadSubscriber.AddOnReceiveActionToChannel(_ => _keepProcessing = _leadsReceived < 5);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Redis connection established?: {_leadSubscriber.ChannelStatus}");
            Console.WriteLine("Waiting for a lead...\n");

            while (_keepProcessing)
            {
                Thread.Sleep(100); 
            }
        }
    }

    internal class MockedLeadCollector : ILeadCollector
    {
        public void CollectLead(ILeadEntity lead)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Received lead entity!");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
                $"Lead entity activity ID: {lead.Properties.Where(prop => prop.Id == ContextKeys.ActivityGuidKey).Select(prop => prop.Value)}\n");
        }
    }
}
