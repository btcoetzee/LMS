using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
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
            var renterValue = leadEntity.Segments.SingleOrDefault(item => item.type == LeadEntity.Interface.Constants.SegementKeys.RenterKey);
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
