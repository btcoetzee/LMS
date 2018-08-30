using System;
using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class StateValidator : IValidator
    {
        public StateValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid State
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var stateValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.StateKey)?.Value;
            if (stateValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("State Not In Context.\n");
                return false;
            }

            if (stateValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("State Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
