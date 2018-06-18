namespace LeadCollector
{
    using Decorator.Interface;
    using LeadEntity.Interface;
    using Publisher.Interface;
    using System;
    using Validator.Interface;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Admiral.Components.Instrumentation.Contract;
    using Compare.Components.Notification.Channels.InProc;
    using Compare.Components.Notification.Contract;
    using Compare.Components.Notification.Publishers;
    using Compare.Components.Notification.Subscribers;

    public class LeadCollector
    {
        /// <summary>
        /// Defining the Members
        /// </summary>
        private readonly IValidator _validLead;

        private readonly IDecorator _decorateLead;
        private readonly IPublisher _publishLead;
        private static IPublisher<string> _notificationPublisher;
        private static InProcNotificationChannel<string> _channel; // This should not be here as publisher contains the channel already
        private static Queue<CampaingValidatorSubscriptionTask> _taskQueue;

        /// <summary>
        /// Constructor for LeadCollector
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="leadValidator"></param>
        /// <param name="leadDecorator"></param>
        /// <param name="leadPublisher"></param>
        public LeadCollector(InProcNotificationChannel<string> channel,
            IValidator leadValidator, IDecorator leadDecorator, IPublisher leadPublisher)
        {
            if (channel != null) _channel = channel;
            _validLead = leadValidator ?? throw new ArgumentNullException(nameof(leadValidator));
            _decorateLead = leadDecorator ?? throw new ArgumentNullException(nameof(leadDecorator));
            _publishLead = leadPublisher ?? throw new ArgumentNullException(nameof(leadPublisher));
        }

        /// <summary>
        /// Process the Lead Collected
        /// </summary>
        /// <param name="lead"></param>
        void CollectLead(ILeadEntity lead)
        {
            //If the lead is valid, decorate and publish 
            if (_validLead.ValidLead(lead).Equals(true))
            {
                // Decorate
                _decorateLead.DecorateLead(lead);

                // Start the Campaign Tasks
                _taskQueue = new Queue<CampaingValidatorSubscriptionTask>();

                // For now - Set Up the Tasks in the CampaingValidators as subscriptions
                // This will be done within the Campaign - these subscriptions will be 
                // waiting on actions published on the channel
                // Take out once set up within Campaign Verification.
                CampaignValidatorSubscriptionTasksStart();

                // Broadcast to the Campaigns
                _publishLead.PublishLead(lead);

                // Apply Lead Resolution
                LeadResolutionValidatorTask().Wait();
            }
        }

        /// <summary>
        /// Lead Resolution Validator Task
        /// Wait for all the Campaign Taks to complete
        /// </summary>
        /// <returns></returns>
        private static async Task LeadResolutionValidatorTask()
        {
            while (_taskQueue.Any())
            {
                var executingTask = _taskQueue.Dequeue();
                Console.WriteLine($"Waiting for task {executingTask.CampaignId} to complete...");
                await executingTask;
            }
        }


        /// <summary>
        /// Campaing Validator Task Class
        /// </summary>
        private class CampaingValidatorSubscriptionTask : Task
        {
            public int CampaignId { get; }

            public CampaingValidatorSubscriptionTask(int id, Action behavior) : base(behavior)
            {
                CampaignId = id;
            }
        }

        /// <summary>
        /// Campaign Validator Subscription Task Start
        /// </summary>
        void CampaignValidatorSubscriptionTasksStart()
        {
            var random = new Random();
            const int taskCount = 5;
            const int baseWait = 500;

            for (var i = 1; i <= taskCount; i++)
            {
                var closure = i;

                var task = new CampaingValidatorSubscriptionTask(closure, () =>
                {
                    var id = closure;

                    var timing = baseWait + random.Next(1000);

                    var subscriber = new Subscriber<string>(_channel, true);

                    if (id % 2 == 0)
                    {
                        Console.WriteLine($"Task {id} is bailing...");
                        //subscriber.DisconnectChannel(); //Don't need to do this with in-proc messages.
                        return;
                    }

                    var running = false;

                    subscriber.AddOnReceiveActionToChannel(message =>
                    {
                        Console.WriteLine($"{id}: Received message: {message} Starting execution...");
                        running = true;
                    });

                    while (!running)
                    {
                    }

                    Console.WriteLine($"Task ID {id} is waiting for {timing} milliseconds...");

                    Thread.Sleep(timing);

                    Console.WriteLine($"Task ID {id} complete!");
                });

                _taskQueue.Enqueue(task);
                task.Start();
            }
        }
    }

}
