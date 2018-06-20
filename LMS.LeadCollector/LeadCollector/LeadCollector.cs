namespace LeadCollector
{
    using System;
    using Decorator.Interface;
    using LeadEntity.Interface;
    using Publisher.Interface;
    using Validator.Interface;
    using Campaign.Interface;
    using Resolution.Interface;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
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
        private readonly IValidator _leadValidator;
        private readonly IDecorator _leadDecorator;
        private readonly IPublisher _leadPublisher;
        private readonly IResolution _leadResolver;

        
        private static InProcNotificationChannel<string> _channel; // TBD This should not be here as publisher contains the channel already
        private static Queue<CampaignSubscriptionTask> _taskQueue;

        /// <summary>
        /// Constructor for LeadCollector
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="leadValidator"></param>
        /// <param name="leadDecorator"></param>
        /// <param name="leadPublisher"></param>
        /// <param name="leadResolver"></param>
        public LeadCollector(InProcNotificationChannel<string> channel, IValidator leadValidator, IDecorator leadDecorator, IPublisher leadPublisher, IResolution leadResolver)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _leadValidator = leadValidator ?? throw new ArgumentNullException(nameof(leadValidator));
            _leadDecorator = leadDecorator ?? throw new ArgumentNullException(nameof(leadDecorator));
            _leadPublisher = leadPublisher ?? throw new ArgumentNullException(nameof(leadPublisher));
            _leadResolver = leadResolver ?? throw new ArgumentNullException(nameof(leadResolver));
        }

        /// <summary>
        /// Process the Lead Collected
        /// </summary>
        /// <param name="lead"></param>
        void CollectLead(ILeadEntity lead)
        {
            //If the lead is valid, decorate and publish 
            if (_leadValidator.ValidLead(lead).Equals(true))
            {
                // Decorate
                _leadDecorator.DecorateLead(lead);

                // TBD Start the Campaign Tasks
                _taskQueue = new Queue<CampaignSubscriptionTask>();

                // TBD For now - Set Up the Tasks in the CampaingValidators as subscriptions
                // This will be done within the Campaign - these subscriptions will be 
                // waiting on actions published on the channel
                // Take out once set up within Campaign Validations
                CampaignSubscriptionTasksStart();

                // Broadcast to the Campaigns
                _leadPublisher.PublishLead(lead);

                // Apply Lead Resolution - will need to pass taskQueue
                _leadResolver.ResolveLead(lead);
 
            }
        }

        /// <summary>
        /// Lead Resolution Validator Task
        /// Wait for all the Campaign Taks to complete
        /// </summary>
        /// <returns></returns>
        private static async Task LeadResolverTask()
        {
            while (_taskQueue.Any())
            {
                var executingTask = _taskQueue.Dequeue();
                Console.WriteLine($"Waiting for task {executingTask.CampaignId} to complete...");
                await executingTask;
            }
        }


        ///// <summary>
        ///// Campaing Validator Task Class
        ///// </summary>
        //private class CampaignSubscriptionTask : Task
        //{
        //    public int CampaignId { get; }

        //    public CampaignSubscriptionTask(int id, Action behavior) : base(behavior)
        //    {
        //        CampaignId = id;
        //    }
        //}

        /// <summary>
        /// Campaign Validator Subscription Task Start
        /// </summary>
        void CampaignSubscriptionTasksStart()
        {
            var random = new Random();
            const int taskCount = 5;
            const int baseWait = 500;

            for (var i = 1; i <= taskCount; i++)
            {
                var closure = i;

                var task = new CampaignSubscriptionTask(closure, () =>
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
