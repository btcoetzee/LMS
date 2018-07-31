using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

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
        private static readonly ColorSet LogColors = new ColorSet(ConsoleColor.White, ConsoleColor.Black);
        private static readonly ColorSet ObjectLogColors = new ColorSet(ConsoleColor.White, ConsoleColor.Black);
        private static string[] _leadDirectory;


        public static void Main(string[] args)
        {
            const string processContext = "Main";

            // Create the logger cliet for this program
            var loggerClient = new ConsoleLoggerClient();
            //loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "ConsoleApp.Main Start",ProcessContext = processContext,SolutionContext = SolutionContext});

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
            WriteToConsole($"{GetLeadDirectory()}Select a lead [1-{leadEntities.Length}] to process: ", LogColors);
            int.TryParse(Console.ReadLine(), out var leadChoice);

            // Process the lead
            while (leadChoice >= 1 && leadChoice <= leadEntities.Length)
            {
                leadChoice--; //Since array indices start at 0
                WriteToConsole($"Processing Activity ID {leadEntities[leadChoice].Context.First(ctx => ctx.Id == ContextKeys.ActivityGuidKey).Value}", LogColors);
                WriteToConsole(JsonConvert.SerializeObject(leadEntities[leadChoice], Formatting.Indented), ObjectLogColors);
                leadCollector.CollectLead(leadEntities[leadChoice]);
                WriteToConsole("Lead was Handed Off .\n", LogColors);
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
            _leadDirectory[0] = "Valid Lead - Phone #, PNI Age";
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

 
            };
            _leadDirectory[1] = "Valid Lead - NO Phone #, PNI Age";
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
                    new MyProperty(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },


            };
            _leadDirectory[2] = "Valid Lead - Phone #, NO PNI Age";
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
  
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },

 
            };

            _leadDirectory[3] = "Valid Lead - NO Phone #, NO PNI Age";
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
                },

                Segments = new ISegment[]
                {
                    new MySegment(SegementKeys.HighPOPKey),
                    new MySegment(SegementKeys.HomeownerKey)
                },

  
            };

            _leadDirectory[4] = "Valid Lead - NO IdentityGUID";
            leadEntities[4] = new MyLeads
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
            _leadDirectory[5] = "Valid Lead - BF526BAF-F860-4530-BAA5-A205E285881A";
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
            _leadDirectory[6] = "Valid Lead - No POP";
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
    }
    #endregion

    #region LeadEntityClassImplementations
    class MyLeads : ILeadEntity
    {
        public IContext[] Context { get; set; }
        public IProperty[] Properties { get; set; }
        public ISegment[] Segments { get; set; }
        public IResultCollection ResultCollection { get; set; }
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
