using System;
using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class BuyOnlineClickValidator : IValidator
    {
        public BuyOnlineClickValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid BuyOnlineClick
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var buyOnlineClickValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.BuyOnlineClickKey)?.Value;
            if (buyOnlineClickValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("BuyOnlineClick Not In Context.\n");
                return false;
            }

            if (!DateTime.TryParse(buyOnlineClickValue.ToString(), out DateTime buyOnlineClick))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("BuyOnlineClick Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
