using System;
using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class RenterValidator : IValidator
    {
        public RenterValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid Renter
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var renterValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.SegementKeys.RenterKey)?.Value;
            if (renterValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("Renter Not In Context.\n");
                return false;
            }

            if (renterValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("Renter Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
