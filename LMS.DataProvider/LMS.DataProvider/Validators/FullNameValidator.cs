using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
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
            var fullNameValue = leadEntity.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.FullNameKey)?.Value;
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
