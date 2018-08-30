using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class BuyByPhoneClickValidator : IValidator
    {
        public BuyByPhoneClickValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid BuyByPhoneClick
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var buyByPhoneClickValue = leadEntity.Context.SingleOrDefault(item => item.Id == Modules.LeadEntity.Interface.Constants.ActivityKeys.BuyByPhoneClickKey)?.Value;
            if (buyByPhoneClickValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("BuyByPhoneClick Not In Context.\n");
                return false;
            }

            if (!DateTime.TryParse(buyByPhoneClickValue.ToString(), out DateTime buyByPhoneClick))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("BuyByPhoneClick Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
