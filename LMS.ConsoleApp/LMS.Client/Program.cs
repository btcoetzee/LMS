using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using LMS.Client.Constants;
using LMS.Client.Entities;

namespace LMS.Client
{
    
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Admiral.Components.Instrumentation.Contract;
    using Compare.Components.Notification.Channels.Redis;
    using Compare.Components.Notification.Contract;
    using Compare.Components.Notification.Publishers;

    using Newtonsoft.Json;

    public class Program
    {
        public static List<string> CustomerLeadDirectory = new List<string>();
        private static readonly ColorSet LogColors = new ColorSet(ConsoleColor.White, ConsoleColor.Black);
        private static readonly ColorSet ObjectLogColors = new ColorSet(ConsoleColor.White, ConsoleColor.Black);
        private static IPublisher<string> _leadPublisher;

        public static void Main(string[] args)
        {
            _leadPublisher =
                new Publisher<string>(
                    new INotificationChannel<string>[]
                        {new RedisNotificationChannel("LMS", "Redis", "LMS")}, true);
            //{ new RedisNotificationChannel("LMS", "Redis", "LMS", new MockLogger())}, true);

            Console.WriteLine($"Redis channel status: {_leadPublisher.ChannelStatus.First()}");

            var customerToLMS = CreateCustomerLeads();

            // Ask for user to select a lead to process
            WriteToConsole($"{GetLeadDirectory()}Select a customer lead [1-{customerToLMS.Count}] to process: ", LogColors);
            int.TryParse(Console.ReadLine(), out var leadChoice);

            // Process the lead
            while (leadChoice >= 1 && leadChoice <= customerToLMS.Count)
            {
                leadChoice--; //Since array indices start at 0
                WriteToConsole("\n_______________________________________________________________________________________________________________________________\n\n", LogColors);
                var serializedEntity = JsonConvert.SerializeObject(customerToLMS[leadChoice], Formatting.Indented);
                WriteToConsole(serializedEntity, ObjectLogColors);
                _leadPublisher.BroadcastMessage(serializedEntity);

                Console.ReadLine();
                WriteToConsole($"{GetLeadDirectory()}Select a lead [1-{customerToLMS.Count}] to process: ", LogColors);
                int.TryParse(Console.ReadLine(), out leadChoice);

            }


            WriteToConsole("The End.  Press any key to continue...", LogColors);
            Console.ReadKey();
        }

        public static void WriteToConsole(string stringToWrite, ColorSet colorSet)
        {
            Console.ForegroundColor = colorSet.ForegroundColor;
            Console.BackgroundColor = colorSet.BackgroundColor;
            Console.WriteLine(stringToWrite);
        }

      

        #region CreateLeads
        static List<ICustomerLead> CreateCustomerLeads()
        {
            const int quotedProduct = 101;
            int[] displayedBrands = new int[] { 22, 58, 181, 218 };

            var customerLeads = new List<ICustomerLead>();

            CustomerLeadDirectory.Add("Lead - All Values");
            customerLeads.Add(new DefaultCustomerLead(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(CustomerLeadKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new KeyValuePair<string, object>(CustomerLeadKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(CustomerLeadKeys.BuyTypeKey, BuyClickType.BuyOnLine),
                new KeyValuePair<string, object>(CustomerLeadKeys.BrandIdKey, displayedBrands[0].ToString()),
                new KeyValuePair<string, object>(CustomerLeadKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(CustomerLeadKeys.ClickTimeKey, DateTime.UtcNow)

            }));
            CustomerLeadDirectory.Add("Lead - No BuyType");
            customerLeads.Add(new DefaultCustomerLead(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(CustomerLeadKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new KeyValuePair<string, object>(CustomerLeadKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(CustomerLeadKeys.BrandIdKey, displayedBrands[1].ToString()),
                new KeyValuePair<string, object>(CustomerLeadKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(CustomerLeadKeys.ClickTimeKey, DateTime.UtcNow)

            }));
            CustomerLeadDirectory.Add("Lead - No BrandId");
            customerLeads.Add(new DefaultCustomerLead(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(CustomerLeadKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new KeyValuePair<string, object>(CustomerLeadKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(CustomerLeadKeys.BuyTypeKey, BuyClickType.BuyOnPhone),
                new KeyValuePair<string, object>(CustomerLeadKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(CustomerLeadKeys.ClickTimeKey, DateTime.UtcNow)

            }));
            CustomerLeadDirectory.Add("Lead - No Displayed Brands");
            customerLeads.Add(new DefaultCustomerLead(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(CustomerLeadKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new KeyValuePair<string, object>(CustomerLeadKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(CustomerLeadKeys.BuyTypeKey, BuyClickType.BuyOnLine),
                new KeyValuePair<string, object>(CustomerLeadKeys.BrandIdKey, displayedBrands[3].ToString()),
                new KeyValuePair<string, object>(CustomerLeadKeys.ClickTimeKey, DateTime.UtcNow)

            }));

            return customerLeads;
        }
        #endregion

        private static string GetLeadDirectory()
        {
            string leadDirectory = "\n";

            var ix = 1;
            foreach (var directory in CustomerLeadDirectory)
            {
                leadDirectory += $"\n{ix}. {directory}";
                ix++;
            }
            leadDirectory += "\n";

            return leadDirectory;
        }
    }

    #region LeadEntityClassImplementations


    #endregion

    public struct ColorSet
    {
        public static readonly ColorSet StandardLoggingColors = new ColorSet(ConsoleColor.Green, ConsoleColor.Black);
        public static readonly ColorSet ErrorLoggingColors = new ColorSet(ConsoleColor.Red, ConsoleColor.Yellow);

        public ConsoleColor ForegroundColor { get; }

        public ConsoleColor BackgroundColor { get; }

        public ColorSet(ConsoleColor fg, ConsoleColor bg)
        {
            this.ForegroundColor = fg;
            this.BackgroundColor = bg;
        }
    }

    internal class MockLogger : ILogger
    {
        public void Log(string message, TraceEventType severity = TraceEventType.Error, int faultCode = 0)
        {

        }

        public void Log(Exception exception, string message, TraceEventType severity = TraceEventType.Error, int faultCode = 0,
            Guid? correlationId = null)
        {

        }

        public string AppDomainName { get; set; }
        public ICollection<string> Categories => new string[0];
        public string MachineName { get; set; }
        public string ProcessId { get; set; }
        public string ProcessName { get; set; }
    }
}
