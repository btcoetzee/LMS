using LMS.CampaignManager.Interface;

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
    using LMS.CampaignManager.Interface;

    public class Program
    {
        private const string SolutionContext = "LMS.ConsoleApp.Exe";

        public static void Main(string[] args)
        {

            const string processContext = "Main";

            // Create the logger cliet for this program
            var loggerClient = new ConsoleLoggerClient();
            loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "ConsoleApp.Main Start",
                ProcessContext = processContext,
                SolutionContext = SolutionContext
            });

            var provider = new ServiceCollection()
                .BuildUp()
                .BuildServiceProvider();

            // Set up the Lead Collector
            var leadCollector = provider.GetService<ILeadCollector>();
  
            // Set up the Campaign Manager
            var campaignManager = provider.GetService<ICampaignManager>();

            // Mock leads to be sent through
            var leadEntities = CreateLeads();

            // Ask for user to select a lead to process
            Console.WriteLine($"Select lead [1-{leadEntities.Length}]: ");
            int.TryParse(Console.ReadLine(), out var leadChoice);

            // Process the lead
            while (leadChoice >= 1 && leadChoice <= leadEntities.Length)
            {
                leadChoice--; //Since array indices start at 0
                Console.WriteLine($"Processing Activity ID {leadEntities[leadChoice].Context.First(ctx => ctx.Id == ContextKeys.ActivityGuidKey).Value}");
                leadCollector.CollectLead(leadEntities[leadChoice]);
                Console.WriteLine("Processing complete.\n");

                Console.WriteLine($"Select lead [1-{leadEntities.Length}]: ");
                int.TryParse(Console.ReadLine(), out leadChoice);
            }

            loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "ConsoleApp.Main End",
                ProcessContext = processContext,
                SolutionContext = SolutionContext
            });

           loggerClient.Log(new DefaultLoggerClientErrorObject
            {
                OperationContext = "Ending. Press any key to continue...",
                ProcessContext = processContext,
                SolutionContext = SolutionContext
            });

            Console.ReadKey();
        }

        static ILeadEntity[] CreateLeads()
        {
            const int quotedProduct = 101;
            const string additonalProducts = "None";
            const string priorBi = "100/300";
            const bool priorInsurance = true;
            const int vehicleCount = 2;
            const string quotedBi = "100/300";
            int[] displayedBrands = new int[] { 22, 58, 181, 218 };
            const long phoneNumber = 88855654569;
            const int pni_Age = 28;

            var leadEntities = new ILeadEntity[6];

            leadEntities[0] = new MyLeads
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
                    new MyContext(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
                    new MyContext(ContextKeys.AdditionalProductKey,additonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(PropertyKeys.PriorBIKey,priorBi),
                    new MyProperty(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new MyProperty(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new MyProperty(PropertyKeys.QuotedBIKey,quotedBi),
                    new MyProperty(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString())
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
