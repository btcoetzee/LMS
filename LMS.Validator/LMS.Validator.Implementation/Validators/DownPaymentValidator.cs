﻿using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
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
            var downPaymentValue = leadEntity.Context.SingleOrDefault(item => item.Id == Modules.LeadEntity.Interface.Constants.ActivityKeys.DownPaymentKey)?.Value;
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
