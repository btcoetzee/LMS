using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
{
    public class BuyOnlineBrandValidator : IValidator
    {
        public BuyOnlineBrandValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid BuyOnlineBrand
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var buyOnlineBrandValue = leadEntity.Context.SingleOrDefault(item => item.Id ==Modules.LeadEntity.Interface.Constants.ActivityKeys.BuyOnlineBrandKey)?.Value;
            if (buyOnlineBrandValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("BuyOnlineBrand Not In Context.\n");
                return false;
            }

            if (!Int32.TryParse(buyOnlineBrandValue.ToString(), out int buyOnlineBrand))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("BuyOnlineBrand Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
