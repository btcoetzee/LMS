using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
{
    public class DownPaymentValidator : IValidator
    {
        public DownPaymentValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid DownPayment
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var downPaymentValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.DownPaymentKey)?.Value;
            if (downPaymentValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("DownPayment Not In Context.\n");
                return false;
            }

            if (downPaymentValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("DownPayment Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
