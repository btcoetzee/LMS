using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
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
            var highPOPValue = leadEntity.Segments.SingleOrDefault(item => item.type == LeadEntity.Interface.Constants.SegementKeys.HighPOPKey);
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
