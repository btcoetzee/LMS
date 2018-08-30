using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class EmailAddressValidator : IValidator
    {
        public EmailAddressValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid EmailAddress
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var emailAddressValue = leadEntity.Context.SingleOrDefault(item => item.Id == Modules.LeadEntity.Interface.Constants.PropertyKeys.EmailAddressKey)?.Value;
            if (emailAddressValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("EmailAddress Not In Context.\n");
                return false;
            }

            if (emailAddressValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("EmailAddress Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
