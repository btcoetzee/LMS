namespace LMS.ConsoleApp
{
    using LeadEntity.Interface;
    using System;
    using System.Linq;
    using IoC;
    using LeadCollector.Interface;
    using LeadEntity.Interface.Constants;
    using Microsoft.Extensions.DependencyInjection;
    using LMS.LoggerClient.Console;
    using LMS.LoggerClient.Interface;

    public class Program
    {
        public static int QuotedProduct = 101;
        public static string AdditonalProducts = "None";
        public static string PriorBI = "100/300";
        public static bool PriorInsurance = true;
        public static int VehicleCount = 2;
        public static string QuotedBI = "100/300";
        public static int[] DisplayedBrands = new int[] { 22, 58, 181, 218 };
        public string HighPOP = "yes";
        public string Homeowner = "yes";

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            new ConsoleLoggerClient().Log(new DefaultLoggerClientObject
            {
                LogDateTime = DateTime.Now,
                OperationContext = "ConsoleApp.Main Start",
                ProcessContext = "LMS.ConsoleApp.Exe",
                SolutionContext = string.Empty
            });

            var leadCollector = new ServiceCollection()
                .BuildUp()
                .BuildServiceProvider()
                .GetService<ILeadCollector>();

            var leadEntities = CreateLeads();
            var leadChoice = 1;
            Console.WriteLine($"Select lead [1-{leadEntities.Length}]: ");
            int.TryParse(Console.ReadLine(), out leadChoice);
            while (leadChoice >= 1 && leadChoice <= leadEntities.Length)
            {
                leadChoice--; //Since array indices start at 0
                Console.WriteLine($"Processing Activity ID {leadEntities[leadChoice].Context.First(ctx => ctx.Id == ContextKeys.ActivityGuidKey).Value}");
                leadCollector.CollectLead(leadEntities[leadChoice]);
                Console.WriteLine("Processing complete.\n");

                Console.WriteLine($"Select lead [1-{leadEntities.Length}]: ");
                int.TryParse(Console.ReadLine(), out leadChoice);
            }

            new ConsoleLoggerClient().Log(new DefaultLoggerClientObject
            {
                LogDateTime = DateTime.Now,
                OperationContext = "ConsoleApp.Main End",
                ProcessContext = "LMS.ConsoleApp.Exe",
                SolutionContext = string.Empty
            });

            new ConsoleLoggerClient().Log(new DefaultLoggerClientErrorObject
            {
                LogDateTime = DateTime.Now,
                OperationContext = "Had an error occurred...",
                ProcessContext = "LMS.ConsoleApp.Exe",
                SolutionContext = string.Empty
            });

            Console.ReadKey();
        }

        static ILeadEntity[] CreateLeads()
        {
            var leadEntities = new ILeadEntity[6];

            leadEntities[0] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,QuotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,AdditonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,PriorBI),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,PriorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,VehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,QuotedBI),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,DisplayedBrands.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },

                Results = new IResults[]
                {
                    new MyResult(ResultKeys.ResultTimeStampKey, DateTime.Now)
                }

            };

            leadEntities[1] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,QuotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,AdditonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,PriorBI),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,PriorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,VehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,QuotedBI),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,DisplayedBrands.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },

                Results = new IResults[]
                {
                    new MyResult(ResultKeys.ResultTimeStampKey, DateTime.Now)
                }

            };

            leadEntities[2] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,QuotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,AdditonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,PriorBI),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,PriorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,VehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,QuotedBI),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,DisplayedBrands.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },

                Results = new IResults[]
                {
                    new MyResult(ResultKeys.ResultTimeStampKey, DateTime.Now)
                }

            };

            leadEntities[3] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,QuotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,AdditonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,PriorBI),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,PriorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,VehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,QuotedBI),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,DisplayedBrands.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },

                Results = new IResults[]
                {
                    new MyResult(ResultKeys.ResultTimeStampKey, DateTime.Now)
                }

            };

            leadEntities[4] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,QuotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,AdditonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,PriorBI),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,PriorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,VehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,QuotedBI),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,DisplayedBrands.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },

                Results = new IResults[]
                {
                    new MyResult(ResultKeys.ResultTimeStampKey, DateTime.Now)
                }

            };

            leadEntities[5] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(ContextKeys.ActivityGuidKey, null),
                    new MyContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(ContextKeys.QuotedProductKey,QuotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,AdditonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,PriorBI),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,PriorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,VehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,QuotedBI),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,DisplayedBrands.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },

                Results = new IResults[]
                {
                    new MyResult(ResultKeys.ResultTimeStampKey, DateTime.Now)
                }

            };
            return leadEntities;
        }
    }


    class MyLeads : ILeadEntity
    {

        public bool isValid()
        {
            throw new NotImplementedException();
        }

        public IContext[] Context { get; set; }
        public IProperty[] Properties { get; set; }
        public ISegment[] Segments { get; set; }
        public IResults[] Results { get; set; }
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

    struct MyResult : IResults
    {
        public MyResult(string id, object value)
        {
            Id = id;
            Value = value;
        }

        public string Id { get; private set; }

        public object Value { get; private set; }
    }

}
