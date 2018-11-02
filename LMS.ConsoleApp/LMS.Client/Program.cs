using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
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
        private static string[] _customerLeadDirectory;
        private static readonly ColorSet LogColors = new ColorSet(ConsoleColor.White, ConsoleColor.Black);
        private static readonly ColorSet ObjectLogColors = new ColorSet(ConsoleColor.White, ConsoleColor.Black);
        private static IPublisher<string> _leadPublisher;

        public static void Main(string[] args)
        {
            _leadPublisher =
                new Publisher<string>(
                    new INotificationChannel<string>[]
                        {new RedisNotificationChannel("LMS", "Redis", "LMS", new MockLogger())}, true);

            Console.WriteLine($"Redis channel status: {_leadPublisher.ChannelStatus.First()}");

            var customerToLMS = CreateCustomerLeads();

            // Ask for user to select a lead to process
            WriteToConsole($"{GetLeadDirectory()}Select a customer lead [1-{customerToLMS.Length}] to process: ", LogColors);
            int.TryParse(Console.ReadLine(), out var leadChoice);

            // Process the lead
            while (leadChoice >= 1 && leadChoice <= customerToLMS.Length)
            {
                leadChoice--; //Since array indices start at 0
                WriteToConsole("\n_______________________________________________________________________________________________________________________________\n\n", LogColors);
                var serializedEntity = JsonConvert.SerializeObject(customerToLMS[leadChoice], Formatting.Indented);
                WriteToConsole(serializedEntity, ObjectLogColors);
                _leadPublisher.BroadcastMessage(serializedEntity);

                Console.ReadLine();
                WriteToConsole($"{GetLeadDirectory()}Select a lead [1-{customerToLMS.Length}] to process: ", LogColors);
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
        static ICustomerLead[] CreateCustomerLeads()
        {
            const int quotedProduct = 101;
            const string buyOnlineStr = "BuyOnline";
            const string buyByPhoneStr = "BuyByPhone";
            int[] displayedBrands = new int[] { 22, 58, 181, 218 };
    

            var customerLeads = new ICustomerLead[4];
            _customerLeadDirectory = new string[4];

            _customerLeadDirectory[0] = "Lead - All Values";
            customerLeads[0] = new DefaultCustomerLead(new ICustomerLeadProperty[]
            {
                new DefaultCustomerLeadProperty(CustomerLeadKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.QuotedProductKey, quotedProduct.ToString()),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.BuyTypeKey, buyOnlineStr),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.BrandIdKey, displayedBrands[0].ToString()),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.DisplayedBrandsKey, displayedBrands),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.ClickTimeKey, DateTime.UtcNow)

            });
            _customerLeadDirectory[1] = "Lead - No BuyType";
            customerLeads[1] = new DefaultCustomerLead(new ICustomerLeadProperty[]
            {
                new DefaultCustomerLeadProperty(CustomerLeadKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.QuotedProductKey, quotedProduct.ToString()),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.BrandIdKey, displayedBrands[1].ToString()),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.DisplayedBrandsKey, displayedBrands),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.ClickTimeKey, DateTime.UtcNow)

            });
            _customerLeadDirectory[2] = "Lead - No BrandId";
            customerLeads[2] = new DefaultCustomerLead(new ICustomerLeadProperty[]
            {
                new DefaultCustomerLeadProperty(CustomerLeadKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.QuotedProductKey, quotedProduct.ToString()),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.BuyTypeKey, buyOnlineStr),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.DisplayedBrandsKey, displayedBrands),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.ClickTimeKey, DateTime.UtcNow)

            });
            _customerLeadDirectory[3] = "Lead - No Displayed Brands";
            customerLeads[3] = new DefaultCustomerLead(new ICustomerLeadProperty[]
            {
                new DefaultCustomerLeadProperty(CustomerLeadKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.QuotedProductKey, quotedProduct.ToString()),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.BuyTypeKey, buyOnlineStr),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.BrandIdKey, displayedBrands[3].ToString()),
                new DefaultCustomerLeadProperty(CustomerLeadKeys.ClickTimeKey, DateTime.UtcNow)

            });

            return customerLeads;
        }
        #endregion

        private static string GetLeadDirectory()
        {
            string leadDirectory = "\n";

            var ix = 1;
            foreach (var directory in _customerLeadDirectory)
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
