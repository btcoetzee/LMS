using System;

namespace LMS.CLI
{
    using System.Threading;
    using System.Threading.Tasks;
    using IoC;
    using Compare.Services.LMS.Modules.Preamble.Interface;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            //var leadCollector = new ServiceCollection()
            //    .BuildUp()
            //    .BuildServiceProvider()
            //    .GetService<ILeadCollector>();

            AsyncDemo("Hello World!");

            for (var i = 0; i <= 100; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(100);
            }

            Console.ReadKey();
        }
        private static void AsyncDemo(string message)
        {
            var writer = new Writer(); //Akin to the Resolver

            var random = new Random();

            const int threadCount = 5;
            var responses = new string[threadCount];

            var task = new Task<string[]>(() =>
            {
                var campaigns = new Task<string>[threadCount]; //These are the campaign threads

                for (var i = 0; i < threadCount; i++)
                {
                    var closure = i;
                    var delay = random.Next(1500, 7500);

                    campaigns[i] = new Task<string>(() =>
                    {
                        Thread.Sleep(delay);

                        var color = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{closure}: Done sleeping!");
                        Console.ForegroundColor = color;

                        if ((closure + 1) % 3 != 0)
                            return $"Task {closure} processed message: \"{message}\" after {delay}ms";

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{closure}: Encountered an exception!");
                        Console.ForegroundColor = color;

                        return null;
                    });

                    campaigns[i].Start();
                }

                for (var i = 0; i < threadCount; i++)
                {
                    responses[i] = campaigns[i].Result;
                }

                return responses;
            });

            //This is the resolver step.
            task.ContinueWith(async messageCollection => writer.WriteOut(await messageCollection));

            task.Start();
        }
    }

    internal class Writer
    {
        //This is where the resolver would do its work.
        public void WriteOut(string[] messages)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;

            foreach (var m in messages)
            {
                Console.WriteLine(m);
            }

            Console.ForegroundColor = color;
        }
    }
}
