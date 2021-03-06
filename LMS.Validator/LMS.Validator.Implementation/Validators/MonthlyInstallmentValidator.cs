﻿using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class MonthlyInstallmentValidator : IValidator
    {
        public MonthlyInstallmentValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid MonthlyInstallment
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var monthlyInstallmentValue = leadEntity.Context.SingleOrDefault(item => item.Id == Modules.LeadEntity.Interface.Constants.ActivityKeys.MonthlyInstallmentKey)?.Value;
            if (monthlyInstallmentValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("MonthlyInstallment Not In Context.\n");
                return false;
            }

            if (monthlyInstallmentValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("MonthlyInstallment Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
