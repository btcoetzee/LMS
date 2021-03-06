﻿using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class HomeOwnerValidator : IValidator
    {
        public HomeOwnerValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid HomeOwner
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var homeOwnerValue = leadEntity.Context.SingleOrDefault(item => item.Id == Modules.LeadEntity.Interface.Constants.SegementKeys.HomeownerKey)?.Value;
            if (homeOwnerValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("HomeOwner Not In Context.\n");
                return false;
            }

            if (homeOwnerValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("HomeOwner Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
