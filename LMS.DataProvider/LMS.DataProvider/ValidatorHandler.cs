namespace LMS.DataProvider
{
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;
    using LMS.LeadEntity.Components;
    using LMS.LeadEntity.Interface;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using LMS.LeadEntity.Interface.Constants;
    using System.Linq;

    public class ValidatorHandler
    {
        // Validator Class Name
        static readonly string validatorClassName = "ValidatorCollection";

        // Get the Type for the Class
        static readonly Type classType = (from assemblies in AppDomain.CurrentDomain.GetAssemblies()
                                            from type in assemblies.GetTypes()
                                            where type.IsClass && type.Name == validatorClassName
                                            select type).Single();
        public ValidatorHandler()
        {

        }

        public bool Execute(ILeadEntity leadEntity)
        {
            // Create an array that specifies the types of the parameters
            // TODO this needs to be A LOT more if going this way!
           // Type[] validatorArgList = GetArgumentTypes("ILeadEntity");

            // leadEntity will be input parameter for the validators
            object[] argObjects = new object[] { leadEntity };

            // Select all the validators from the database
            var validatorMethodName = String.Empty;
            using (var db = new ValidatorContext())
            {
                foreach (var validator in db.Validators)
                {
                    validatorMethodName = validator.MethodName;
                    Console.WriteLine($"{validatorMethodName} | {validator.MethodName}");
                    //Invoke the Method within the Class
                    var valid = (bool)classType.InvokeMember(validatorMethodName, BindingFlags.InvokeMethod, null, null, argObjects);
                    Console.WriteLine($"Returned {valid}");

                }
            }


            Console.WriteLine($"Invoked validators in {validatorClassName}.");
            Console.ReadKey();

            return true;
        }


        // Declare a delegate type that can be used to execute the completed dynamic method. 
        public delegate bool Validator(ILeadEntity leadEntityObject);


        private static Type[] GetArgumentTypes(string argumentTypeInputStr)
        {

            string tmp = "";
            var listOfTypes = new List<Type>();
            int j = 0;

            for (int i = 0; i < argumentTypeInputStr.Length; i++)
            {
                if (argumentTypeInputStr[i] != ';')
                {
                    tmp = tmp + argumentTypeInputStr[i];
                    continue;
                }
               
                listOfTypes.Add(GetTypeOf(tmp));
                tmp = "";
            }
            // Add last 1 if more
            if (!tmp.Equals(","))
                listOfTypes.Add(GetTypeOf(tmp));

            return listOfTypes.ToArray();

        }

        static Type GetTypeOf(string typeStr)
        {
            switch (typeStr)
            {
                case "string":
                    return typeof(string);
                case "ILeadIdentity":
                    return typeof(ILeadEntity);
                default:
                    // log big error
                    return typeof(Exception);
                

            }

        }
    }
}

//public bool Execute(ILeadEntity leadEntity)
//{
//// Create an array that specifies the types of the parameters
//// TODO this needs to be A LOT more if going this way!
//Type[] validatorArgList = GetArgumentTypes("ILeadEntity");
//object[] argObjects = new object[] { leadEntity };
//string[] argNames = new string[] { "leadEntity" };

//// Validator Method Name
//var validatorMethodName = "HasValidActivityGuid";

////Invoke the Method within the Class
//var valid = (bool)classType.InvokeMember(validatorMethodName, BindingFlags.InvokeMethod, null, null, argObjects);


//Console.WriteLine($"Inoked {validatorClassName}.{validatorMethodName} and it returned {valid}");
//Console.ReadKey();
//return valid;

//}