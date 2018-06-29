using System;

namespace LMS.CLI
{
    using IoC;
    using LeadCollector.Interface;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var leadCollector = new ServiceCollection()
                .BuildUp()
                .BuildServiceProvider()
                .GetService<ILeadCollector>();

            Console.ReadKey();
        }
    }
}
