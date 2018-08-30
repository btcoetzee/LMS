using System;
using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class RequoteValidator : IValidator
    {
        public RequoteValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid Requote
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var requoteValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.RequoteKey)?.Value;
            if (requoteValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("Requote Not In Context.\n");
                return false;
            }

            if (!DateTime.TryParse(requoteValue.ToString(), out DateTime requote))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("Requote Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
