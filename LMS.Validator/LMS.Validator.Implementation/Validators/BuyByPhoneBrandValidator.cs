using System;
using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class BuyByPhoneBrandValidator : IValidator
    {
        public BuyByPhoneBrandValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid BuyByPhoneBrand
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var buyByPhoneBrandValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.BuyByPhoneBrandKey)?.Value;
            if (buyByPhoneBrandValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("BuyByPhoneBrand Not In Context.\n");
                return false;
            }

            if (!Int32.TryParse(buyByPhoneBrandValue.ToString(), out int buyByPhoneBrand))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("BuyByPhoneBrand Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
