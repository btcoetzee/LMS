using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class FullNameValidator : IValidator
    {
        public FullNameValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid FullName
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var fullNameValue = leadEntity.Context.SingleOrDefault(item => item.Id == Modules.LeadEntity.Interface.Constants.PropertyKeys.FullNameKey)?.Value;
            if (fullNameValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("FullName Not In Context.\n");
                return false;
            }

            if (fullNameValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("FullName Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
