using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class AnnualPremiumValidator : IValidator
    {
        public AnnualPremiumValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid AnnualPremium
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var annualPremiumValue = leadEntity.Context.SingleOrDefault(item => item.Id == Modules.LeadEntity.Interface.Constants.ActivityKeys.AnnualPremiumKey)?.Value;
            if (annualPremiumValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("AnnualPremium Not In Context.\n");
                return false;
            }

            if (annualPremiumValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("AnnualPremium Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
