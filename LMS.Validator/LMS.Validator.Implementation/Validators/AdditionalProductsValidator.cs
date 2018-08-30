using System;
using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class AdditionalProductsValidator : IValidator
    {
        public AdditionalProductsValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid AdditionalProduct
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var additionalProductValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.AdditionalProductKey)?.Value;
            if (additionalProductValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("AdditionalProduct Not In Context.\n");
                return false;
            }

            if (!Int32.TryParse(additionalProductValue.ToString(), out int quotedProduct))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("AdditionalProduct Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
