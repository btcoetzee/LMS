using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
{
    public class PhoneNumberValidator : IValidator
    {
        public PhoneNumberValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid PhoneNumber
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var phoneNumberValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber)?.Value;
            if (phoneNumberValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("PhoneNumber Not In Context.\n");
                return false;
            }

            if (phoneNumberValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("PhoneNumber Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
