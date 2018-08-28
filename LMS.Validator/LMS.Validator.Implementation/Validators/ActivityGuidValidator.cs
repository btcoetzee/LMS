using System;
using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class ActivityGuidValidator : IValidator
    {
        /// <summary>
        /// Constructor - Required for Activator
        /// </summary>
        public ActivityGuidValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid ActitivityGuid
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var activityGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey)?.Value;
            if (activityGuidValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("ActivityGuid Not In Context.\n");
                return false;
            }
            
            if (!Guid.TryParse(activityGuidValue.ToString(), out Guid activityGuid))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("ActivityGuid Invalid.\n");
                return false;
            }
            return true;
        }


    }
}
