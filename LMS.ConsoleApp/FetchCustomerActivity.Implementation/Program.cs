using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Compare.Components.Notification.Abstractions;
using Compare.Components.Notification.Channels.Redis;
using Compare.Components.Notification.Publishers;
using CompareNow.Components.Constants.US.Motor;
using FetchCustomerActivity.Implementation.ClientObject;
using Newtonsoft.Json;

namespace FetchCustomerActivity.Implementation
{
    class Program
    {
        const int RandomMaxCount = 50;  // Number of leads to send through when random is selected
        private const string DuplicateGuidStr = "BF526BAF-F860-4530-BAA5-A205E285881A";
        private static IList<Guid> listOfGuids;  // This is the list of customerActvityGuids retrieved from the database
        public class CJDirectoryItem
        {
            public string CJLeadType { get; set; }
            public int CJLeadCnt { get; set; }
            public List<Guid> guidList { get; set; }
        }
        public static List<CJDirectoryItem> _CJLeadDirectory = new List<CJDirectoryItem>();
        private static readonly ColorSet LogColors = new ColorSet(ConsoleColor.White, ConsoleColor.Black);
        private static readonly ColorSet ObjectLogColors = new ColorSet(ConsoleColor.White, ConsoleColor.Black);
        private static IPublisher<string> _leadPublisher;

        public static void Main(string[] args)
        {
            //_leadPublisher =
            //    new Publisher<string>(
            //        new INotificationChannel<string>[]
            //            {new RedisNotificationChannel("LMS", "Redis", "LMS")}, true);
            ////  { new RedisNotificationChannel("LMS", "Redis", "LMS", new MockLogger())}, true);

            //Console.WriteLine($"Redis channel status: {_leadPublisher.ChannelStatus.First()}");
            
            // Retrieve customerActivityGuids from the database
            listOfGuids = GetCustomerActivitiesFromDb(RandomMaxCount);
            int activityGuidIx = 0;

            var cjLeads = CreateCJLeads();
            var numberOfLeadScenariosSetUp = cjLeads.Count;
            var randomNbr = new Random();
            var randomRunningFlag = false;
            DefaultClientObject newLead; // Used for creating a new lead object when the array of possible leads have been created.

            var randomCounter = 1;
            // Ask for user to select a lead to process
            WriteToConsole($"{GetCJLeadDirectory()}Select a lead [1-{numberOfLeadScenariosSetUp}] to process: ", LogColors);
            int.TryParse(Console.ReadLine(), out var choiceSelectedFromMenu);

            // Use the scenario selected - or continue if the randomRunningFlag is set
            while ((choiceSelectedFromMenu >= 1 && choiceSelectedFromMenu <= numberOfLeadScenariosSetUp) || (randomRunningFlag))
            {
                choiceSelectedFromMenu--; //Since array indices start at 0

                // Run Leads through at Random if selected - This is the last choice on the menu
                if ((choiceSelectedFromMenu == (numberOfLeadScenariosSetUp - 1)) || (randomRunningFlag))
                {
                    // if first time through random
                    if (randomRunningFlag == false)
                    {
                        randomRunningFlag = true;
                        // Create the guidlist that keeps track of the guids within the scenarios
                        for (int i = 0; i < numberOfLeadScenariosSetUp - 1; i++)
                        {
                            _CJLeadDirectory[i].guidList = new List<Guid>();
                        }
                    }
                    // Now do a random selection from the scerios instead 
                    choiceSelectedFromMenu = randomNbr.Next(0, (numberOfLeadScenariosSetUp - 1));
                    _CJLeadDirectory[choiceSelectedFromMenu].CJLeadCnt++;

                    // Keep track of what other info is being sent for the Customer.
                    _CJLeadDirectory[choiceSelectedFromMenu].guidList.Add(listOfGuids[activityGuidIx]);

                    // Remove if previously added but then add the CustomerActivity Retrieved from the database
                    cjLeads[choiceSelectedFromMenu].ClientObject.RemoveAll(item => item.Key == ClientObjectKeys.ActivityGuidKey);
                    cjLeads[choiceSelectedFromMenu].ClientObject.Add(
                        new KeyValuePair<string, object>(ClientObjectKeys.ActivityGuidKey,
                            listOfGuids[activityGuidIx]));
                    activityGuidIx++;

                    if (randomCounter == RandomMaxCount)
                    {
                        randomRunningFlag = false;  // STOP
                        randomCounter = 0;
                    }
                    else
                    {
                        randomCounter++;
                    }
                }
                else
                {
                    // Remove if previously added but then add the CustomerActivity Retrieved from the database
                    cjLeads[choiceSelectedFromMenu].ClientObject.RemoveAll(item => item.Key == ClientObjectKeys.ActivityGuidKey);
                    cjLeads[choiceSelectedFromMenu].ClientObject.Add(
                        new KeyValuePair<string, object>(ClientObjectKeys.ActivityGuidKey,
                            listOfGuids[activityGuidIx]));
                    activityGuidIx++;
                }
                newLead = new DefaultClientObject();
                foreach (var valuePair in cjLeads[choiceSelectedFromMenu].ClientObject)
                {
                    newLead.ClientObject.Add(new KeyValuePair<string, object>(valuePair.Key, valuePair.Value));
                }

                //newLead = CreateNewLead(cjLeads[choiceSelectedFromMenu]);

                //WriteToConsole($"Processing Activity ID {leadEntities[leadChoice].Context.First(ctx => ctx.Id == ContextKeys.ActivityGuidKey).Value}", LogColors);
                //                var serializedEntity = JsonConvert.SerializeObject(cjLeads[leadChoice], Formatting.Indented);
                //var serializedEntity = JsonConvert.SerializeObject(newLead, Formatting.Indented);

                //if (!randomRunningFlag)
                //{
                //    WriteToConsole("\n_______________________________________________________________________________________________________________________________\n\n", LogColors);
                //    WriteToConsole(serializedEntity, ObjectLogColors);
                //}
                WriteToConsole($"Pulishing: {newLead.ClientObject.FirstOrDefault(item => item.Key == ClientObjectKeys.ActivityGuidKey).Value.ToString()}: {choiceSelectedFromMenu + 1}: {_CJLeadDirectory[choiceSelectedFromMenu].CJLeadType}", LogColors);
                //WriteToConsole(_CJLeadDirectory[leadChoice], LogColors);
                // Ok send it on
                //_leadPublisher.BroadcastMessage(serializedEntity);

                // If running through leads - do not stop - else show menu of leads
                if (!randomRunningFlag)
                {
                    // At the end write a 
                    if (randomCounter == 0)
                    {
                        var summaryStr = Environment.NewLine + ("").PadRight(180, '_') + Environment.NewLine +
                                         $"SENT {RandomMaxCount} LEADS THROUGH...." + Environment.NewLine + "SUMMARY:" + Environment.NewLine;
                        var ix = 1;
                        foreach (var lead in _CJLeadDirectory)
                        {
                            summaryStr += $"{ix}. ({lead.CJLeadCnt}) : {lead.CJLeadType}" + Environment.NewLine;
                            if (lead.guidList != null)
                            {
                                foreach (var guid in lead.guidList)
                                {
                                    summaryStr += $"{guid}" + Environment.NewLine;
                                }
                            }
                            ix++;
                        }
                        WriteToConsole(summaryStr, LogColors);
                        randomCounter = 1;
                    }
                    Console.ReadLine();
                    WriteToConsole($"{GetCJLeadDirectory()}Select a lead [1-{cjLeads.Count}] to process: ", LogColors);
                    int.TryParse(Console.ReadLine(), out choiceSelectedFromMenu);

                }

                Thread.Sleep(200);

            }

            WriteToConsole("The End.  Press any key to continue...", LogColors);
            Console.ReadKey();
        }

        public static IList<Guid> GetCustomerActivitiesFromDb(int count)
        {
            IList<Guid> customerActivityGuidList;

            var fetchCustomerActivityFromDb = new FetchCustomerActivityFromDb();
            customerActivityGuidList = fetchCustomerActivityFromDb.getCustomerActivity(count);
            
            foreach (var customerActivityGuid in customerActivityGuidList)
            {
                Console.WriteLine(customerActivityGuid);
            }

            Console.ReadKey();
            return customerActivityGuidList;
        }
        public static void WriteToConsole(string stringToWrite, ColorSet colorSet)
        {
            Console.ForegroundColor = colorSet.ForegroundColor;
            Console.BackgroundColor = colorSet.BackgroundColor;
            Console.WriteLine(stringToWrite);
        }


        #region CreateLeads
        private static string GetCJLeadDirectory()
        {
            string leadDirectory = "\n";

            var ix = 1;
            foreach (var directory in _CJLeadDirectory)
            {
                leadDirectory += $"\n{ix}. {directory.CJLeadType}";
                ix++;
            }
            leadDirectory += "\n";

            return leadDirectory;
        }

        public static DefaultClientObject CreateNewLead(IClientObject source)
        {
            if (source == null)
                return null;

            var destination = new DefaultClientObject();


            foreach (var valuePair in source.ClientObject)
            {
                destination.ClientObject.Add(new KeyValuePair<string, object>(valuePair.Key, valuePair.Value));
            }

            return destination;

        }

        static List<IClientObject> CreateCJLeads()
        {
            const int quotedProduct = 101;
            int? brandId1 = 22;
            int? brandId2 = 58;
            int? brandId3 = 181;
            int? brandId4 = 218;
            int[] displayedBrands = new int[] { (int)brandId1, (int)brandId2, (int)brandId3, (int)brandId4 };


            var customerLeads = new List<IClientObject>();

            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = "Lead - All Values", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.BuyTypeKey, BuyClickType.BuyOnPhone),
                new KeyValuePair<string, object>(ClientObjectKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(ClientObjectKeys.BrandIdKey, brandId4),
                new KeyValuePair<string, object>(ClientObjectKeys.ClickTimeKey, DateTime.UtcNow)

            }));
            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = "Lead - No BrandId - Failure LeadCollector Validator Tests", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.BuyTypeKey, BuyClickType.BuyOnPhone),
                new KeyValuePair<string, object>(ClientObjectKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(ClientObjectKeys.ClickTimeKey, DateTime.UtcNow)
            }));
            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = "Lead - No Quoted Product - Failure in Campaign Manager Validator Tests", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.BuyTypeKey, BuyClickType.BuyOnLine),
                new KeyValuePair<string, object>(ClientObjectKeys.BrandIdKey, brandId2),
                new KeyValuePair<string, object>(ClientObjectKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(ClientObjectKeys.ClickTimeKey, DateTime.UtcNow)
            }));
            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = "Lead - No BuyType - Failure in Campaign Validator Tests", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(ClientObjectKeys.BrandIdKey, brandId4),
                new KeyValuePair<string, object>(ClientObjectKeys.ClickTimeKey, DateTime.UtcNow)

            }));
            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = "Lead - Less than 2 Brands Displayed - Failure in Campaign Rule Tests", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.BuyTypeKey, BuyClickType.BuyOnLine),
                new KeyValuePair<string, object>(ClientObjectKeys.BrandIdKey, brandId1),
                new KeyValuePair<string, object>(ClientObjectKeys.ClickTimeKey, DateTime.UtcNow)
            }));
            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = $"Lead - Run {RandomMaxCount} Leads through Randomly", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
            }));

            return customerLeads;
        }

    }
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
}
