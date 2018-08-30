using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
{
    public class LowPOPValidator : IValidator
    {
        public LowPOPValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid LowPOP
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var lowPOPValue = leadEntity.Context.SingleOrDefault(item => item.Id ==Modules.LeadEntity.Interface.Constants.SegementKeys.LowPOPKey)?.Value;
            if (lowPOPValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("LowPOP Not In Context.\n");
                return false;
            }

            if (lowPOPValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("LowPOP Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
