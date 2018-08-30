namespace LMS.DataProvider
{
    using LMS.ValidatorDataProvider.Interface.ValidatorEntites;
    using LMS.ValidatorDataProvider.Interface;
    using System.Collections.Generic;
    using System;
    using System.Linq;

    public class ValidatorDataProvider : IValidatorDataProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ValidatorDataProvider()
        {
        }

        public List<ValidatorClassAndAssemblyData> LeadCollectorValidatorClassAndAssemblyList()
        {
            var classAndAssemblyList = new List<ValidatorClassAndAssemblyData>();

            // Create the className List
            using (var context = new ValidatorContext())
            {
                int prevAssemblyId = 0;  //Initialize to force first lookup for ControlAssemblyId
                string assemblyName = String.Empty;
                foreach (var leadCollectorValidator in context.LeadCollectorValidators)
                {
                    // Select the validator
                    var validator = context.Validators.FirstOrDefault(v => v.ValidatorId == leadCollectorValidator.ValidatorId);

                    // Select different Assembly if different from prev - Most Validators in same assembly
                    if (validator != null && validator.ControlAssemblyId != prevAssemblyId)
                    {

                        assemblyName = context.ControlAssemblies.Where(a => a.ControlAssemblyId == validator.ControlAssemblyId).Select(a => a.AssemblyName)
                            .SingleOrDefault();
                        prevAssemblyId = validator.ControlAssemblyId;

                    }
    
                    if (validator != null)
                    {
                        classAndAssemblyList.Add(new ValidatorClassAndAssemblyData(validator.ClassName, assemblyName));
                    }
                }
                return classAndAssemblyList;
            }
        }
    }
}
