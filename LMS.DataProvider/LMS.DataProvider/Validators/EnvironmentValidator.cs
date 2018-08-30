using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
{
    public class EnvironmentValidator : IValidator
    {
        public EnvironmentValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid Environment
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var environmentValue = leadEntity.Context.SingleOrDefault(item => item.Id ==Modules.LeadEntity.Interface.Constants.ContextKeys.EnvironmentKey)?.Value;
            if (environmentValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("Environment Not In Context.\n");
                return false;
            }

            if (environmentValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("Environment Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
