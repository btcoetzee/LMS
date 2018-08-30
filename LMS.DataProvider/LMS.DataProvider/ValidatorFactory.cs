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
    using LMS.ValidatorFactory.Interface;
    using LMS.ValidatorDataProvider.Interface;
    using LMS.ValidatorDataProvider.Interface.ValidatorEntites;
    using LMS.Validator.Implementation;
    using LMS.LoggerClient.Interface;
    using LMS.LoggerClientEventTypeControl.Interface.Constants;

    public class ValidatorFactory : IValidatorFactory
    {

        private readonly IValidatorDataProvider _validatorDataProvider;
        readonly ILoggerClient _loggerClient;
        private static string solutionContext = "ValidatorFactory";

        /// <summary>
        /// Constructor
        /// </summary>
        public ValidatorFactory(IValidatorDataProvider validatorDataProvider, ILoggerClient loggerClient)
        {
            _validatorDataProvider = validatorDataProvider ?? throw new ArgumentNullException(nameof(validatorDataProvider));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        /// <summary>
        /// Build the list of Validators for the LeadCollector
        /// </summary>
        /// <returns></returns>
        public List<IValidator> BuildLeadCollectorValidators()
        {
            // Get the list of ClassNames to Build the Validators from
            var leadCollectorValidatorClassAndAssemblies = _validatorDataProvider.LeadCollectorValidatorClassAndAssemblyList();
            // Build Validators and Return
            return BuildValidators(leadCollectorValidatorClassAndAssemblies);
        }

        /// <summary>
        /// From the classNameList - Build a list of Validators
        /// </summary>
        /// <param name="validatorClassAndAssemblyList"></param>
        /// <returns></returns>
        private List<IValidator> BuildValidators(List<ValidatorClassAndAssemblyData> validatorClassAndAssemblyList)
        {
            string processContext = "BuildValidators";
            var validatorList = new List<IValidator>();
  
            try
            {
                foreach (var validator in validatorClassAndAssemblyList)
                {
                    var assembly = System.Reflection.Assembly.LoadFrom(validator.AssemblyName + ".dll");
                    Type classType = (from type in assembly.GetTypes()
                                      where type.IsClass && type.Name == validator.ClassName
                                      select type).Single();
                    // Create an instances of the Validator Class
                    validatorList.Add((IValidator)Activator.CreateInstance(classType));
                }

            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject{OperationContext = "Exception in Creating Validator Factory",ProcessContext = processContext,SolutionContext = solutionContext,Exception = ex,ErrorContext = ex.Message,EventType = LoggerClientEventType.LoggerClientEventTypes.Error});

            }

            return validatorList;

            //foreach (var className in validatorClassNameList)
            //{
            //    Type classType =
            //        (from assemblies in AppDomain.CurrentDomain.GetAssemblies()
            //            from type in assemblies.GetTypes()
            //            where type.IsClass && type.Name == className
            //            select type).Single();

            //    // Create an instances of the Validator Class
            //    validatorList.Add((IValidator)Activator.CreateInstance(classType));
      
            //}
            //return validatorList;
        }
    }

 
}
