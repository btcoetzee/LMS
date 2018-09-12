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
    using LeadCollector.Interface;
    using Modules.LeadEntity.Interface;
    using Modules.LeadEntity.Interface.Constants;
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
            leadEntities[0] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,priorBi),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,quotedBi),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new MyProperty(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
                    new MyProperty(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },



            };

            _leadDirectory[1] = "Lead - Phone #, PNI Age";
            leadEntities[1] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,priorBi),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,quotedBi),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new MyProperty(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
                    new MyProperty(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },


            };
            _leadDirectory[2] = "Lead - NO Phone #, PNI Age";
            leadEntities[2] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,priorBi),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,quotedBi),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new MyProperty(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },


            };
            _leadDirectory[3] = "Lead - Phone #, NO PNI Age";
            leadEntities[3] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,priorBi),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,quotedBi),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new MyProperty(PropertyKeys.PhoneNumber,phoneNumber.ToString()),

                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },


            };

            _leadDirectory[4] = "Lead - NO Phone #, NO PNI Age";
            leadEntities[4] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,priorBi),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,quotedBi),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },


            };


            _leadDirectory[5] = "Lead - BF526BAF-F860-4530-BAA5-A205E285881A - Notification sent previously";
            leadEntities[5] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, new Guid("BF526BAF-F860-4530-BAA5-A205E285881A").ToString()),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,priorBi),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,quotedBi),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new MyProperty(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
                    new MyProperty(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },


            };
            _leadDirectory[6] = "Lead - No POP";
            leadEntities[6] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,priorBi),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,"false"),
                    new MyProperty(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,quotedBi),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new MyProperty(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
                    new MyProperty(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
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
    class MyLeads : ILeadEntity
    {
        public IContext[] Context { get; set; }
        public IProperty[] Properties { get; set; }
        public ISegment[] Segments { get; set; }
        public IResultCollection ResultCollection { get; set; }
        public List<string> ErrorList { get; set; }
    }

    struct MyContext : IContext
    {
        public MyContext(string id, object value)
        {
            Id = id;
            Value = value;
        }

        public string Id { get; private set; }

        public object Value { get; private set; }

        private string ToXmlString()
        {
            return string.Format("<Context id='{0}' value='{1}'/>", Id, Value);
        }

        private string ToJsonString()
        {
            return string.Format("{{\"Id\":\"{0}\", \"Value\":\"{1}\"}}", Id, Value);
        }

        public string ToString(FormatSpecifier format)
        {
            switch (format)
            {
                case FormatSpecifier.Xml:
                    return ToXmlString();

                case FormatSpecifier.Json:
                    return ToJsonString();

                default:
                    return string.Empty;
            }
        }

        public enum FormatSpecifier
        {
            Xml,
            Json
        }
    }

    struct MyProperty : IProperty
    {
        public MyProperty(string id, object value)
        {
            Id = id;
            Value = value;
        }
        public string Id { get; private set; }

        public object Value { get; private set; }
    }

    struct MySegment : ISegment
    {
        public MySegment(string Type)
        {
            type = Type;
        }
        public string type { get; private set; }
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
