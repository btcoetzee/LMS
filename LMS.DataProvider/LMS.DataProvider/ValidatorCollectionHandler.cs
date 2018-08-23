namespace LMS.DataProvider
{
    using LMS.LeadEntity.Interface;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using LMS.DataProvider.ValidatorCollection;
    using LMS.Validator.Interface;
    using System.Linq;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using Remotion.Linq.Clauses;
    public class ValidatorCollectionHandler
    {
        private string _errorMessage;
        public ValidatorCollectionHandler() => _errorMessage = String.Empty;
        public string ErrorMessage => _errorMessage;
        public bool Execute(ILeadEntity leadEntity, List<string> validatorClassNameList)
        {

            foreach (var className in validatorClassNameList)
            {
                Type classType =
                    (from assemblies in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assemblies.GetTypes()
                        where type.IsClass && type.Name == className
                        select type).Single();

                // Create an instances of the Validator Class
                IValidator validator = (IValidator)Activator.CreateInstance(classType);
   
                // Validate the lead for the given class
                var valid = validator.ValidLead(leadEntity);
                if (!valid)
                {
                    // _errorMessage += validator.ErrorMsg;
                }

                // 

                Console.WriteLine($"Invoked validators in {className}.");
            }

            if (_errorMessage != String.Empty)
            {

            }


   
            Console.ReadKey();

            return true;
        }


        public DbContext GetValidatorContext()
        {

            return new ValidatorContext();


        }
    }
}
