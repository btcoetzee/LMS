namespace LMS.DataProvider
{
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

        public List<string> LeadCollectorValidatorClassNameList()
        {
            var classNameList = new List<String>();

            // Create the className List
            using (var context = new ValidatorContext())
            {
                foreach (var leadCollectorValidator in context.LeadCollectorValidators)
                {
                    // Select the validator
                    var validator = context.Validators.FirstOrDefault(v => v.ValidatorId == leadCollectorValidator.ValidatorId);
                    // var className = context.Validators.Where(v => v.ValidatorId == leadCollectorValidator.ValidatorId).Select(v => v.ClassName).ToString();
                    //var className = context.Validators.Where(v => v.ValidatorId == leadCollectorValidator.ValidatorId).Select(v => new { ClassName = v.ClassName}).ToString();

                    if (validator != null)
                    {
                        classNameList.Add(validator.ClassName);
                    }
                }
                return classNameList;
            }
        }
    }
}
