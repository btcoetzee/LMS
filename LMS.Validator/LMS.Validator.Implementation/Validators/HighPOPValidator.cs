﻿using System;
using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class HighPOPValidator : IValidator
    {
        public HighPOPValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid HighPOP
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var highPOPValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.SegementKeys.HighPOPKey)?.Value;
            if (highPOPValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("HighPOP Not In Context.\n");
                return false;
            }

            if (highPOPValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("HighPOP Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
