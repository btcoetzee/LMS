namespace LMS.ConsoleApp
{
    using LeadEntity.Interface;
    using System;
    using LeadCollector.Implementation;

    class Program
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

        static void Main(string[] args)
        {
            //var serviceProvider = Bootstrapper.BuildUp(new ServiceCollection()).BuildServiceProvider();
            //var leadCollector = (new ServiceCollection())
            //    .BuildUp()
            //    .BuildServiceProvider()
            //    .GetService<ILeadCollector>();

            var leadCollector = new LeadCollector(null, null, null);

            var leadEntities = CreateLeads();
            var leadChoice = 1;
            Console.WriteLine($"Select lead [1-{leadEntities.Length}]: ");
            int.TryParse(Console.ReadLine(), out leadChoice);
            while (leadChoice >= 1 && leadChoice <= leadEntities.Length)
            {

                Console.WriteLine($"Select lead [1-{leadEntities.Length}]: ");
                int.TryParse(Console.ReadLine(), out leadChoice);
            }
        }

        static ILeadEntity[] CreateLeads()
        {
            var leadEntities = new ILeadEntity[5];

            leadEntities[0] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey,QuotedProduct.ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.AdditionalProductKey,AdditonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorBIKey,PriorBI),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorInsuranceKey,PriorInsurance.ToString()),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.VehicleCountKey,VehicleCount.ToString()),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.QuotedBIKey,QuotedBI),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.DisplayedBrandsKey,DisplayedBrands.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(LeadEntity.Interface.Constants.SegementKeys.HighPOPKey),
                    new MySegment(LeadEntity.Interface.Constants.SegementKeys.HomeownerKey)
                }

            };

            leadEntities[1] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey,QuotedProduct.ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.AdditionalProductKey,AdditonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorBIKey,PriorBI),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorInsuranceKey,PriorInsurance.ToString()),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.VehicleCountKey,VehicleCount.ToString()),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.QuotedBIKey,QuotedBI),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.DisplayedBrandsKey,DisplayedBrands.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(LeadEntity.Interface.Constants.SegementKeys.HighPOPKey),
                    new MySegment(LeadEntity.Interface.Constants.SegementKeys.HomeownerKey)
                }

            };

            leadEntities[2] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey,QuotedProduct.ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.AdditionalProductKey,AdditonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorBIKey,PriorBI),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorInsuranceKey,PriorInsurance.ToString()),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.VehicleCountKey,VehicleCount.ToString()),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.QuotedBIKey,QuotedBI),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.DisplayedBrandsKey,DisplayedBrands.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(LeadEntity.Interface.Constants.SegementKeys.HighPOPKey),
                    new MySegment(LeadEntity.Interface.Constants.SegementKeys.HomeownerKey)
                }

            };

            leadEntities[3] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey,QuotedProduct.ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.AdditionalProductKey,AdditonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorBIKey,PriorBI),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorInsuranceKey,PriorInsurance.ToString()),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.VehicleCountKey,VehicleCount.ToString()),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.QuotedBIKey,QuotedBI),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.DisplayedBrandsKey,DisplayedBrands.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(LeadEntity.Interface.Constants.SegementKeys.HighPOPKey),
                    new MySegment(LeadEntity.Interface.Constants.SegementKeys.HomeownerKey)
                }

            };

            leadEntities[4] = new MyLeads
            {

                Context = new IContext[]
                {
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey,QuotedProduct.ToString()),
                    new MyContext(LeadEntity.Interface.Constants.ContextKeys.AdditionalProductKey,AdditonalProducts)
                },

                Properties = new IProperty[]
                {
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorBIKey,PriorBI),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.PriorInsuranceKey,PriorInsurance.ToString()),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.VehicleCountKey,VehicleCount.ToString()),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.QuotedBIKey,QuotedBI),
                    new MyProperty(LeadEntity.Interface.Constants.PropertyKeys.DisplayedBrandsKey,DisplayedBrands.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(LeadEntity.Interface.Constants.SegementKeys.HighPOPKey),
                    new MySegment(LeadEntity.Interface.Constants.SegementKeys.HomeownerKey)
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

}
