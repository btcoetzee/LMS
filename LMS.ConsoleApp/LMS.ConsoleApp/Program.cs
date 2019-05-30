using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Compare.Services.LMS.Modules.LeadDispatcher.Interface;
using LMS.ConsoleApp.Constants;

namespace LMS.ConsoleApp
{

    using System;
    using System.Linq;
    using IoC;
    using Microsoft.Extensions.DependencyInjection;
    using Compare.Services.LMS.Modules.CampaignManager.Interface;
    using Compare.Services.LMS.Modules.Campaign.Interface;
    using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;
    using Compare.Services.LMS.Modules.LeadEntity.Interface;
    using Compare.Services.LMS.Modules.LeadEntity.Components;
    using Compare.Services.LMS.Modules.LoggerClient.Interface;
    using Compare.Services.LMS.Modules.LoggerClient.Implementation;
    using Compare.Services.LMS.Common.Common.Interfaces;
    using Compare.Services.LMS.Modules.DataProvider.Interface;
    using Compare.Services.LMS.Modules.DataProvider.Implementation;
    using Compare.Services.LMS.Modules.Preamble.Interface;
    using Compare.Services.LMS.Modules.Preamble.Implementation;
    using Compare.Services.LMS.Controls.Factory.Interface;
    using Compare.Services.LMS.Controls.Factory.Implementation;
    using Compare.Services.LMS.Controls.Rule.Implementation;
    using Compare.Services.LMS.Controls.Filter.Implementation;
    using Compare.Services.LMS.Controls.Validator.Interface;
    using Compare.Services.LMS.Controls.Validator.Implementation;
    using Compare.Services.LMS.Controls.Resolver.Interface;
    using Compare.Services.LMS.Controls.Resolver.Implementation;


    public class Program
    {
        private const string SolutionContext = "LMS.ConsoleApp.Exe";
        private static readonly ColorSet LogColors = new ColorSet(ConsoleColor.White, ConsoleColor.Black);
        private static readonly ColorSet ObjectLogColors = new ColorSet(ConsoleColor.White, ConsoleColor.Black);
        private static string[] _leadDirectory;


        public static void Main(string[] args)
        {
            const string processContext = "Main";

            // Create the logger cliet for this program
            var loggerClient = new LoggerClientEventTypeControl(); // LoggerClientEventTypeControl.Implementation.LoggerClientEventTypeControl();
            //loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "ConsoleApp.Main Start",ProcessContext = processContext,SolutionContext = SolutionContext});

            var provider = new ServiceCollection()
                .BuildUp()
                .BuildServiceProvider();

            // Set up different components
          //  var validatorFactory = provider.GetService<IValidatorFactory>();
          //  var controllerFactory = provider.GetService<IControllerFactory>();
         //   var resolverFactory = provider.GetService<IResolverFactory>();
            var leadCollector = provider.GetService<ILeadCollector>();
            var campaignManager = provider.GetService<ICampaignManager>();
            var leadDispatcher = provider.GetService<ILeadDispatcher>();
         //   var loggerClientEventTypeControl = provider.GetService<ILoggerClientEventTypeControl>();
         //   var campaign = provider.GetService<ICampaign>();

            // Mock leads to be sent through
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
                WriteToConsole(JsonConvert.SerializeObject(leadEntities[leadChoice], Formatting.Indented), ObjectLogColors);
                leadCollector.CollectLead(leadEntities[leadChoice]);
                Console.ReadLine();
                WriteToConsole($"{GetLeadDirectory()}Select a lead [1-{leadEntities.Length}] to process: ", LogColors);
                int.TryParse(Console.ReadLine(), out leadChoice);

            }


            //loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "ConsoleApp.Main End",ProcessContext = processContext,SolutionContext = SolutionContext});

            //loggerClient.Log(new DefaultLoggerClientErrorObject{OperationContext = "Ending. Press any key to continue...",ProcessContext = processContext,SolutionContext = SolutionContext});
            WriteToConsole("The End.  Press any key to continue...", LogColors);
            Console.ReadKey();
        }

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

        public static void WriteToConsole(string stringToWrite, ColorSet colorSet)
        {
            Console.ForegroundColor = colorSet.ForegroundColor;
            Console.BackgroundColor = colorSet.BackgroundColor;
            Console.WriteLine(stringToWrite);
        }

        public static void PrintProperties(ILeadEntity leadEntity)
        {
            Console.WriteLine("Properties:");
            foreach (var prop in leadEntity.GetType().GetProperties())
            {
                Console.WriteLine(prop.Name + ": " + prop.GetValue(leadEntity, null));
                Console.WriteLine();
            }

            Console.WriteLine("Fields:");
            foreach (var field in leadEntity.GetType().GetFields())
            {
                Console.WriteLine(field.Name + ": " + field.GetValue(leadEntity));
            }
        }

     #region CreateLeads
        static ILeadEntity[] CreateLeads()
        {
            //const int quotedProduct = 101;
            //const string additonalProducts = "None";
            //const string priorBi = "100/300";
            //const bool priorInsurance = true;
            //const int vehicleCount = 2;
            //const string quotedBi = "100/300";
            //int[] displayedBrands = new int[] { 22, 58, 181, 218 };
            //const string phoneNumber = "888-556-5456";
            //const int pni_Age = 28;
            //const string emailAddress = "SomeEmail@compare.com";
            //const string stateStr = "VA";
            //const int brandID = 44;

            var leadEntities = new ILeadEntity[7];
            //_leadDirectory = new string[7];

            //_leadDirectory[0] = "Lead - NO IdentityGUID - LeadCollector Validator";
            //leadEntities[0] = new DefaultLeadEntity
            //{

            //    Context = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(ContextKeys.LeadEntityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
            //    },

            //    Properties = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString())
            //    },

            //    Segments = new ISegment[]
            //    {
            //        new DefaultSegment(SegementKeys.HighPOPKey),
            //        new DefaultSegment(SegementKeys.HomeownerKey)
            //    },

            //};

            //_leadDirectory[1] = "Lead - Good ";
            //leadEntities[1] = new DefaultLeadEntity
            //{

            //    Context = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(ContextKeys.LeadEntityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionRequestSeqKey,"1"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SiteIDKey,"26328"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
            //    },

            //    Properties = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.EmailAddressKey,emailAddress),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.StateKey,stateStr),
            //    },
            //    Activity = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(ActivityKeys.BrandIdKey, brandID),
            //        new DefaultLeadEntityObjectContainer(ActivityKeys.BuyType, BuyClickType.BuyOnLine),
            //    },

            //    Segments = new ISegment[]
            //    {
            //        new DefaultSegment(SegementKeys.HighPOPKey),
            //        new DefaultSegment(SegementKeys.HomeownerKey)
            //    },

 
            //};
            //_leadDirectory[2] = "Lead - NO Email, No State - Campaign Manager Validator";
            //leadEntities[2] = new DefaultLeadEntity
            //{

            //    Context = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(ContextKeys.LeadEntityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionRequestSeqKey,"1"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SiteIDKey,"26328"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
            //    },

            //    Properties = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString()),
            //    },
            //    Activity = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(ActivityKeys.BrandIdKey, brandID),
            //        new DefaultLeadEntityObjectContainer(ActivityKeys.BuyType, BuyClickType.BuyOnLine),
            //    },

            //    Segments = new ISegment[]
            //    {
            //        new DefaultSegment(SegementKeys.HighPOPKey),
            //        new DefaultSegment(SegementKeys.HomeownerKey)
            //    },


            //};
            //_leadDirectory[3] = "Lead - QuotedBIValidator - Campaign Validator";
            //leadEntities[3] = new DefaultLeadEntity
            //{

            //    Context = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(ContextKeys.LeadEntityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionRequestSeqKey,"1"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SiteIDKey,"26328"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
            //    },

            //    Properties = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.EmailAddressKey,emailAddress),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.StateKey,stateStr),
            //    },
            //    Activity = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(ActivityKeys.BrandIdKey, brandID),
            //        new DefaultLeadEntityObjectContainer(ActivityKeys.BuyType, BuyClickType.BuyOnLine),
            //    },

            //    Segments = new ISegment[]
            //    {
            //        new DefaultSegment(SegementKeys.HighPOPKey),
            //        new DefaultSegment(SegementKeys.HomeownerKey)
            //    },


            //};

            //_leadDirectory[4] = "Lead - Less than 2 Brands Displayed - Campaign Rule";



            //leadEntities[4] = new DefaultLeadEntity
            //{

            //    Context = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(ContextKeys.LeadEntityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionRequestSeqKey,"1"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SiteIDKey,"26328"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
            //    },

            //    Properties = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,1),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.EmailAddressKey,emailAddress),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.StateKey,stateStr),
            //    },
            //    Activity = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(ActivityKeys.BrandIdKey, brandID),
            //        new DefaultLeadEntityObjectContainer(ActivityKeys.BuyType, BuyClickType.BuyOnLine),
            //    },

            //    Segments = new ISegment[]
            //    {
            //        new DefaultSegment(SegementKeys.HighPOPKey),
            //        new DefaultSegment(SegementKeys.HomeownerKey)
            //    },
            //};

 
            //_leadDirectory[5] = "Lead - BF526BAF-F860-4530-BAA5-A205E285881A - Campaign Filter";
            //leadEntities[5] = new DefaultLeadEntity
            //{

            //    Context = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(ContextKeys.LeadEntityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, new Guid("BF526BAF-F860-4530-BAA5-A205E285881A").ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionRequestSeqKey,"1"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SiteIDKey,"26328"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
            //    },

            //    Properties = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,1),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.EmailAddressKey,emailAddress),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.StateKey,stateStr),
            //    },

            //    Segments = new ISegment[]
            //    {
            //        new DefaultSegment(SegementKeys.HighPOPKey),
            //        new DefaultSegment(SegementKeys.HomeownerKey)
            //    },

   
            //};
            //_leadDirectory[6] = "Lead - No POP - Good";
            //leadEntities[6] = new DefaultLeadEntity
            //{

            //    Context = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(ContextKeys.LeadEntityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.QuotedProductKey,quotedProduct.ToString()),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SessionRequestSeqKey,"1"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.SiteIDKey,"26328"),
            //        new DefaultLeadEntityObjectContainer(ContextKeys.AdditionalProductKey,additonalProducts)
            //    },

            //    Properties = new ILeadEntityObjectContainer[]
            //    {
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorBIKey,priorBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PriorInsuranceKey,"false"),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.QuotedBIKey,quotedBi),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.DisplayedBrandsKey,displayedBrands),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.PNI_Age,pni_Age.ToString()),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.EmailAddressKey,emailAddress),
            //        new DefaultLeadEntityObjectContainer(PropertyKeys.StateKey,stateStr),
            //    },
            //    Activity = new ILeadEntityObjectContainer[]
            //    {
            //       // new DefaultLeadEntityObjectContainer(ActivityKeys.BrandIdKey, brandID),
            //        new DefaultLeadEntityObjectContainer(ActivityKeys.BuyType, BuyClickType.BuyOnLine),
            //    },
            //    Segments = new ISegment[]
            //    {
            //        new DefaultSegment(SegementKeys.HighPOPKey),
            //        new DefaultSegment(SegementKeys.HomeownerKey)
            //    },

  
            //};
            return leadEntities;
        }
    }
    #endregion

    #region LeadEntityClassImplementations
    //class MyLeads : ILeadEntity
    //{
    //    public ILeadEntityObjectContainer[] Context { get; set; }
    //    public IProperty[] Properties { get; set; }
    //    public ISegment[] Segments { get; set; }
    //    public IResultCollection ResultCollection { get; set; }
    //    public IList<string> ErrorList { get; set; }
    //}

    //struct DefaultLeadEntityObjectContainer : ILeadEntityObjectContainer
    //{
    //    public DefaultLeadEntityObjectContainer(string id, object value)
    //    {
    //        Id = id;
    //        Value = value;
    //    }

    //    public string Id { get; private set; }

    //    public object Value { get; private set; }

    //    private string ToXmlString()
    //    {
    //        return string.Format("<Context id='{0}' value='{1}'/>", Id, Value);
    //    }

    //    private string ToJsonString()
    //    {
    //        return string.Format("{{\"Id\":\"{0}\", \"Value\":\"{1}\"}}", Id, Value);
    //    }

    //    public string ToString(FormatSpecifier format)
    //    {
    //        switch (format)
    //        {
    //            case FormatSpecifier.Xml:
    //                return ToXmlString();

    //            case FormatSpecifier.Json:
    //                return ToJsonString();

    //            default:
    //                return string.Empty;
    //        }
    //    }

    //    public enum FormatSpecifier
    //    {
    //        Xml,
    //        Json
    //    }
    //}

    //struct DefaultLeadEntityObjectContainer : IProperty, ILeadEntityObjectContainer
    //{
    //    public DefaultLeadEntityObjectContainer(string id, object value)
    //    {
    //        Id = id;
    //        Value = value;
    //    }
    //    public string Id { get; private set; }

    //    public object Value { get; private set; }
    //}

    //struct DefaultSegment : ISegment
    //{
    //    public DefaultSegment(string Type)
    //    {
    //        type = Type;
    //    }
    //    public string type { get; private set; }
    //}
    #endregion

    #region ObjectDumper

    public class ObjectDumper
    {

        public static void Write(object element)
        {
            Write(element, 0);
        }

        public static void Write(object element, int depth)
        {
            Write(element, depth, Console.Out);
        }

        public static void Write(object element, int depth, TextWriter log)
        {
            ObjectDumper dumper = new ObjectDumper(depth)
            {
                writer = log
            };
            dumper.WriteObject(null, element);
        }

        TextWriter writer;
        int pos;
        int level;
        int depth;

        private ObjectDumper(int depth)
        {
            this.depth = depth;
        }

        private void Write(string s)
        {
            if (s != null)
            {
                writer.Write(s);
                pos += s.Length;
            }
        }

        private void WriteIndent()
        {
            for (int i = 0; i < level; i++) writer.Write("  ");
        }

        private void WriteLine()
        {
            writer.WriteLine();
            pos = 0;
        }

        private void WriteTab()
        {
            Write("  ");
            while (pos % 8 != 0) Write(" ");
        }

        private void WriteObject(string prefix, object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                WriteIndent();
                Write(prefix);
                WriteValue(element);
                WriteLine();
            }
            else
            {
                IEnumerable enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (object item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            WriteIndent();
                            Write(prefix);
                            Write("...");
                            WriteLine();
                            if (level < depth)
                            {
                                level++;
                                WriteObject(prefix, item);
                                level--;
                            }
                        }
                        else
                        {
                            WriteObject(prefix, item);
                        }
                    }
                }
                else
                {
                    MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    WriteIndent();
                    Write(prefix);
                    bool propWritten = false;
                    foreach (MemberInfo m in members)
                    {
                        FieldInfo f = m as FieldInfo;
                        PropertyInfo p = m as PropertyInfo;
                        if (f != null || p != null)
                        {
                            if (propWritten)
                            {
                                WriteTab();
                            }
                            else
                            {
                                propWritten = true;
                            }
                            Write(m.Name);
                            Write("=");
                            Type t = f != null ? f.FieldType : p.PropertyType;
                            if (t.IsValueType || t == typeof(string))
                            {
                                WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
                            }
                            else
                            {
                                if (typeof(IEnumerable).IsAssignableFrom(t))
                                {
                                    Write("...");
                                }
                                else
                                {
                                    Write("{ }");
                                }
                            }
                        }
                    }
                    if (propWritten) WriteLine();
                    if (level < depth)
                    {
                        foreach (MemberInfo m in members)
                        {
                            FieldInfo f = m as FieldInfo;
                            PropertyInfo p = m as PropertyInfo;
                            if (f != null || p != null)
                            {
                                Type t = f != null ? f.FieldType : p.PropertyType;
                                if (!(t.IsValueType || t == typeof(string)))
                                {
                                    object value = f != null ? f.GetValue(element) : p.GetValue(element, null);
                                    if (value != null)
                                    {
                                        level++;
                                        WriteObject(m.Name + ": ", value);
                                        level--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteValue(object o)
        {
            if (o == null)
            {
                Write("null");
            }
            else if (o is DateTime)
            {
                Write(((DateTime)o).ToShortDateString());
            }
            else if (o is ValueType || o is string)
            {
                Write(o.ToString());
            }
            else if (o is IEnumerable)
            {
                Write("...");
            }
            else
            {
                Write("{ }");
            }
        }
    }
    #endregion
}
