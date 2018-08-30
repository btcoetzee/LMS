using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
{
    public class QuoteReferenceValidator : IValidator
    {
        public QuoteReferenceValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid QuoteReference
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var quoteReferenceValue = leadEntity.Context.SingleOrDefault(item => item.Id ==Modules.LeadEntity.Interface.Constants.ActivityKeys.QuoteReferenceKey)?.Value;
            if (quoteReferenceValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("QuoteReference Not In Context.\n");
                return false;
            }

            if (quoteReferenceValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("QuoteReference Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
