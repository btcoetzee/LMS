namespace LeadPublisher
{
    using LeadEntity.Interface;
    using Publisher.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Admiral.Components.Instrumentation.Contract;
    using Compare.Components.Notification.Channels.InProc;
    using Compare.Components.Notification.Contract;
    using Compare.Components.Notification.Publishers;
    using Compare.Components.Notification.Subscribers;
    
    public class LeadPublisher : IPublisher
    {
        LeadPublisher(ILogger logger);

        public void PublishLead(ILeadEntity lead)
        {
            RunTasks().Wait();
        }

        private static async Task RunTasks()
        {
            var logger = new Mock<ILogger>().Object;

            var channel = new InProcNotificationChannel<string>("taskChannel", logger);

            var publisher = new Publisher<string>(new INotificationChannel<string>[] { channel }, true);

            var random = new Random();

            var taskQueue = new Queue<IdTask>();

            const int taskCount = 5;
            const int baseWait = 500;

            for (var i = 1; i <= taskCount; i++)
            {
                var closure = i;

                var task = new IdTask(closure, () =>
                {
                    var id = closure;

                    var timing = baseWait + random.Next(1000);

                    var subscriber = new Subscriber<string>(channel, true);

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
                    { }

                    Console.WriteLine($"Task ID {id} is waiting for {timing} milliseconds...");

                    Thread.Sleep(timing);

                    Console.WriteLine($"Task ID {id} complete!");
                });

                taskQueue.Enqueue(task);
                task.Start();
            }

            Console.WriteLine("\nPushing start signal... \n");

            publisher.BroadcastMessage("Let's get this party started!");

            while (taskQueue.Any())
            {
                var executingTask = taskQueue.Dequeue();
                Console.WriteLine($"Waiting for task {executingTask.TaskId} to complete...");
                await executingTask;
            }

            Console.WriteLine("\nAll threads complete!");

            Console.ReadKey();
        }

        private class IdTask : Task
        {
            public int TaskId { get; }

            public IdTask(int id, Action behavior) : base(behavior)
            {
                TaskId = id;
            }
        }
    }
}

