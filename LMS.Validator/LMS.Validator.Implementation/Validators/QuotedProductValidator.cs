using System;
using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class QuotedProductValidator : IValidator
    {
        public QuotedProductValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid QuotedProduct
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var quotedProductValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey)?.Value;
            if (quotedProductValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("QuotedProduct Not In Context.\n");
                return false;
            }

            if (!Int32.TryParse(quotedProductValue.ToString(), out int quotedProduct))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("QuotedProduct Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
