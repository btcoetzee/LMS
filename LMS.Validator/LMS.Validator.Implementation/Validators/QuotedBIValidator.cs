using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class QuotedBIValidator : IValidator
    {
        public QuotedBIValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid QuotedBI
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var quotedBIValue = leadEntity.Context.SingleOrDefault(item => item.Id == Modules.LeadEntity.Interface.Constants.PropertyKeys.QuotedBIKey)?.Value;
            if (quotedBIValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("QuotedBI Not In Context.\n");
                return false;
            }

            if (quotedBIValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("QuotedBI Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
