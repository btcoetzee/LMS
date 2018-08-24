using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
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
            var emailAddressValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.EmailAddressKey)?.Value;
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
