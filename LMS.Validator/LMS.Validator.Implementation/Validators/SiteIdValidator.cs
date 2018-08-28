﻿using System;
using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class SiteIdValidator : IValidator
    {
        public SiteIdValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid SiteId
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var siteIdValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SiteIDKey)?.Value;
            if (siteIdValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("SiteId Not In Context.\n");
                return false;
            }

            if (!Int32.TryParse(siteIdValue.ToString(), out int siteId))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("SiteId Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
