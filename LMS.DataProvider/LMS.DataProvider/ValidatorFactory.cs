using LMS.ValidatorDataProvider.Interface;

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

    public class ValidatorFactory : IValidatorFactory
    {

        private readonly IValidatorDataProvider _validatorDataProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ValidatorFactory(IValidatorDataProvider validatorDataProvider)
        {
            _validatorDataProvider = validatorDataProvider ?? throw new ArgumentNullException(nameof(validatorDataProvider));
        }

        /// <summary>
        /// Build the list of Validators for the LeadCollector
        /// </summary>
        /// <returns></returns>
        public List<IValidator> BuildLeadCollectorValidators()
        {
            // Get the list of ClassNames to Build the Validators from
            var leadCollectorValidatorClassNames = _validatorDataProvider.LeadCollectorValidatorClassNameList();
            // Build Validators and Return
            return BuildValidators(leadCollectorValidatorClassNames);
        }

        /// <summary>
        /// From the classNameList - Build a list of Validators
        /// </summary>
        /// <param name="validatorClassNameList"></param>
        /// <returns></returns>
        private List<IValidator> BuildValidators(List<string> validatorClassNameList)
        {
            var validatorList = new List<IValidator>();
            foreach (var className in validatorClassNameList)
            {
                Type classType =
                    (from assemblies in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assemblies.GetTypes()
                        where type.IsClass && type.Name == className
                        select type).Single();

                // Create an instances of the Validator Class
                validatorList.Add((IValidator)Activator.CreateInstance(classType));
      
            }
            return validatorList;
        }
    }
}
