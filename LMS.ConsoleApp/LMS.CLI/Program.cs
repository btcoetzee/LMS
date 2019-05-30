using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Admiral.Components.Instrumentation.Contract;
using Compare.Components.Notification.Channels.Redis;
using Compare.Components.Notification.Contract;
using Compare.Components.Notification.Publishers;
using CompareNow.Components.Constants.US.Motor;
using CompareNow.Schemas._20120701;
using LMS.ClientObject.Implementation;
using LMS.ClientObject.Interface;
using LMS.ClientObject.Interface.Constants;

namespace LMS.CLI
{
    public class Program
    {
        const int RandomMaxCount = 50;  // Number of leads to send through when random is selected
        private const string DuplicateGuidStr = "BF526BAF-F860-4530-BAA5-A205E285881A";
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
            _leadPublisher =
                new Publisher<string>(
                    new INotificationChannel<string>[]
                        {new RedisNotificationChannel("LMS", "Redis", "LMS")}, true);
          //  { new RedisNotificationChannel("LMS", "Redis", "LMS", new MockLogger())}, true);

            Console.WriteLine($"Redis channel status: {_leadPublisher.ChannelStatus.First()}");

            var cjLeads = CreateCJLeads();
            var cjLeadCount = cjLeads.Count;
            var randomNbr = new Random();
            var randomRunningFlag = false;
            DefaultClientObject newLead; // Used for creating a new lead object when the array of possible leads have been created.
        
            var randomCounter = 1;
            // Ask for user to select a lead to process
            WriteToConsole($"{GetCJLeadDirectory()}Select a lead [1-{cjLeads.Count}] to process: ", LogColors);
            int.TryParse(Console.ReadLine(), out var leadChoice);

            // Process the lead
            while ((leadChoice >= 1 && leadChoice <= cjLeadCount) || (randomRunningFlag))
            {
                leadChoice--; //Since array indices start at 0

                // Run Leads through at Random
                if ((leadChoice == (cjLeadCount - 1)) || (randomRunningFlag))
                {
                    leadChoice = 5;
                    randomRunningFlag = true;

                    // Now select a random Lead from List
                    leadChoice = randomNbr.Next(0, (cjLeadCount - 1));
                    _CJLeadDirectory[leadChoice].CJLeadCnt++;
                    if (_CJLeadDirectory[leadChoice].guidList == null)
                    {
                        _CJLeadDirectory[leadChoice].guidList = new List<Guid>();
                    }
                    
                    if (randomCounter == RandomMaxCount)
                    {
                        randomRunningFlag = false;
                        randomCounter = 0;
                    }
                    else
                    {
                        randomCounter++;
                    }
                    // Unless it is the duplicate Guid - Assign a new CustomerActivity
                    if (!String.Equals(cjLeads[leadChoice].ClientObject
                            .FirstOrDefault(item => item.Key == ClientObjectKeys.ActivityGuidKey).Value.ToString().ToUpper(),
                            DuplicateGuidStr))
                    {
                        var newActivityGuid = Guid.NewGuid();
                        _CJLeadDirectory[leadChoice].guidList.Add(newActivityGuid);

                        // update the ActivityGuid
                        cjLeads[leadChoice].ClientObject.RemoveAll(item => item.Key == ClientObjectKeys.ActivityGuidKey);
                        cjLeads[leadChoice].ClientObject.Add(new KeyValuePair<string, object>(ClientObjectKeys.ActivityGuidKey, newActivityGuid));
                    }
                    else
                    {
                        _CJLeadDirectory[leadChoice].guidList.Add(new Guid(DuplicateGuidStr));
                    }
                }

                newLead = CreateNewLead(cjLeads[leadChoice]);

                //WriteToConsole($"Processing Activity ID {leadEntities[leadChoice].Context.First(ctx => ctx.Id == ContextKeys.ActivityGuidKey).Value}", LogColors);
                //                var serializedEntity = JsonConvert.SerializeObject(cjLeads[leadChoice], Formatting.Indented);
                var serializedEntity = JsonConvert.SerializeObject(newLead, Formatting.Indented);

                if (!randomRunningFlag)
                {
                    WriteToConsole("\n_______________________________________________________________________________________________________________________________\n\n", LogColors);
                    WriteToConsole(serializedEntity, ObjectLogColors);
                }
                WriteToConsole($"Pulishing: {newLead.ClientObject.FirstOrDefault(item => item.Key == ClientObjectKeys.ActivityGuidKey).Value.ToString()}: {leadChoice+1}: {_CJLeadDirectory[leadChoice].CJLeadType}", LogColors);
                //WriteToConsole(_CJLeadDirectory[leadChoice], LogColors);
                // Ok send it on
                _leadPublisher.BroadcastMessage(serializedEntity);

                // If running through leads - do not stop - else show menu of leads
                if (!randomRunningFlag)
                {
                    // At the end write a 
                    if (randomCounter == 0)
                    {
                        var summaryStr = Environment.NewLine + ("").PadRight(180, '_') + Environment.NewLine +
                                         $"SENT {RandomMaxCount} LEADS THROUGH...." +Environment.NewLine + "SUMMARY:" + Environment.NewLine;
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
                    int.TryParse(Console.ReadLine(), out leadChoice);

                }

                Thread.Sleep(200);

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
            int[] displayedBrands = new int[] { (int) brandId1, (int) brandId2, (int) brandId3, (int) brandId4 };
            

            var customerLeads = new List<IClientObject>();

            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = "Lead - All Values", CJLeadCnt = 0 });  
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.BuyTypeKey, BuyClickType.BuyOnPhone),
                new KeyValuePair<string, object>(ClientObjectKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(ClientObjectKeys.BrandIdKey, brandId4),
                new KeyValuePair<string, object>(ClientObjectKeys.ClickTimeKey, DateTime.UtcNow)

            }));
            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = "Lead - No BrandId - Failure LeadCollector Validator Tests", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.BuyTypeKey, BuyClickType.BuyOnPhone),
                new KeyValuePair<string, object>(ClientObjectKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(ClientObjectKeys.ClickTimeKey, DateTime.UtcNow)
            }));
            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = "Lead - No Quoted Product - Failure in Campaign Manager Validator Tests", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.BuyTypeKey, BuyClickType.BuyOnLine),
                new KeyValuePair<string, object>(ClientObjectKeys.BrandIdKey, brandId2),
                new KeyValuePair<string, object>(ClientObjectKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(ClientObjectKeys.ClickTimeKey, DateTime.UtcNow)
            }));
            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = "Lead - No BuyType - Failure in Campaign Validator Tests", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(ClientObjectKeys.BrandIdKey, brandId4),
                new KeyValuePair<string, object>(ClientObjectKeys.ClickTimeKey, DateTime.UtcNow)

            }));
            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = "Lead - Less than 2 Brands Displayed - Failure in Campaign Rule Tests", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.BuyTypeKey, BuyClickType.BuyOnLine),
                new KeyValuePair<string, object>(ClientObjectKeys.BrandIdKey, brandId1),
                new KeyValuePair<string, object>(ClientObjectKeys.ClickTimeKey, DateTime.UtcNow)
            }));
            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = "Lead - Duplicate Activity ID - Failure in Campaign Filter Tests", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.ActivityGuidKey, new Guid(DuplicateGuidStr).ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.QuotedProductKey, quotedProduct.ToString()),
                new KeyValuePair<string, object>(ClientObjectKeys.BuyTypeKey, BuyClickType.BuyOnLine),
                new KeyValuePair<string, object>(ClientObjectKeys.BrandIdKey, brandId2),
                new KeyValuePair<string, object>(ClientObjectKeys.DisplayedBrandsKey, displayedBrands),
                new KeyValuePair<string, object>(ClientObjectKeys.ClickTimeKey, DateTime.UtcNow)
            }));
            _CJLeadDirectory.Add(new CJDirectoryItem() { CJLeadType = $"Lead - Run {RandomMaxCount} Leads through Randomly", CJLeadCnt = 0 });
            customerLeads.Add(new DefaultClientObject(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(ClientObjectKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
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