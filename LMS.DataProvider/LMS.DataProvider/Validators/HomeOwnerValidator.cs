using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
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
            var homeOwnerValue = leadEntity.Segments.SingleOrDefault(item => item.type == LeadEntity.Interface.Constants.SegementKeys.HomeownerKey);
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
