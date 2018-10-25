using Compare.Services.LMS.Modules.LeadEntity.Components;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;

namespace LMS.CLI
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
        private static string[] _leadDirectory;
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

            var leadEntities = CreateLeads();

            // Ask for user to select a lead to process
            WriteToConsole($"{GetLeadDirectory()}Select a lead [1-{leadEntities.Length}] to process: ", LogColors);
            int.TryParse(Console.ReadLine(), out var leadChoice);

            // Process the lead
            while (leadChoice >= 1 && leadChoice <= leadEntities.Length)
            {
                leadChoice--; //Since array indices start at 0
                WriteToConsole("\n_______________________________________________________________________________________________________________________________\n\n", LogColors);
                WriteToConsole($"Processing Activity ID {leadEntities[leadChoice].Context.First(ctx => ctx.Id == ContextKeys.ActivityGuidKey).Value}", LogColors);

                var serializedEntity = JsonConvert.SerializeObject(leadEntities[leadChoice], Formatting.Indented);

                WriteToConsole(serializedEntity, ObjectLogColors);
                _leadPublisher.BroadcastMessage(serializedEntity);
                
                Console.ReadLine();
                WriteToConsole($"{GetLeadDirectory()}Select a lead [1-{leadEntities.Length}] to process: ", LogColors);
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
        static ILeadEntity[] CreateLeads()
        {
            const int quotedProduct = 101;
            const string additonalProducts = "None";
            const string priorBi = "100/300";
            const bool priorInsurance = true;
            const int vehicleCount = 2;
            const string quotedBi = "100/300";
            int[] displayedBrands = new int[] { 22, 58, 181, 218 };
            const string phoneNumber = "888-556-5456";
            const int pni_Age = 28;

            var leadEntities = new ILeadEntity[7];
            _leadDirectory = new string[7];

            _leadDirectory[0] = "Lead - NO IdentityGUID";
            leadEntities[0] = new DefaultLeadEntity
            {

                Context = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new DefaultSegment(SegementKeys.HighPOPKey),
                    new DefaultSegment(SegementKeys.HomeownerKey)
                },



            };

            _leadDirectory[1] = "Lead - Phone #, PNI Age";
            leadEntities[1] = new DefaultLeadEntity
            {

                Context = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new DefaultSegment(SegementKeys.HighPOPKey),
                    new DefaultSegment(SegementKeys.HomeownerKey)
                },


            };
            _leadDirectory[2] = "Lead - NO Phone #, PNI Age";
            leadEntities[2] = new DefaultLeadEntity
            {

                Context = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new DefaultSegment(SegementKeys.HighPOPKey),
                    new DefaultSegment(SegementKeys.HomeownerKey)
                },


            };
            _leadDirectory[3] = "Lead - Phone #, NO PNI Age";
            leadEntities[3] = new DefaultLeadEntity
            {

                Context = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),

                },

                Segments = new ISegment[]
                {
                    new DefaultSegment(SegementKeys.HighPOPKey),
                    new DefaultSegment(SegementKeys.HomeownerKey)
                },


            };

            _leadDirectory[4] = "Lead - NO Phone #, NO PNI Age";
            leadEntities[4] = new DefaultLeadEntity
            {

                Context = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                },

                Segments = new ISegment[]
                {
                    new DefaultSegment(SegementKeys.HighPOPKey),
                    new DefaultSegment(SegementKeys.HomeownerKey)
                },


            };


            _leadDirectory[5] = "Lead - BF526BAF-F860-4530-BAA5-A205E285881A - Notification sent previously";
            leadEntities[5] = new DefaultLeadEntity
            {

                Context = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, new Guid("BF526BAF-F860-4530-BAA5-A205E285881A").ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new DefaultSegment(SegementKeys.HighPOPKey),
                    new DefaultSegment(SegementKeys.HomeownerKey)
                },


            };
            _leadDirectory[6] = "Lead - No POP";
            leadEntities[6] = new DefaultLeadEntity
            {

                Context = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new ILeadEntityObjectContainer[]
                {
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,"false"),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
                    new Compare.Services.LMS.Modules.LeadEntity.Components.DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new DefaultSegment(SegementKeys.HighPOPKey),
                    new DefaultSegment(SegementKeys.HomeownerKey)
                },


            };
            return leadEntities;
        }
        #endregion

        private static string GetLeadDirectory()
        {
            string leadDirectory = "\n";

            var ix = 1;
            foreach (var directory in _leadDirectory)
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
