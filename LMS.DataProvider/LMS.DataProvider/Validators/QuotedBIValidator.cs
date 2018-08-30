using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
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
            var quotedBIValue = leadEntity.Properties.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.QuotedBIKey)?.Value;
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
