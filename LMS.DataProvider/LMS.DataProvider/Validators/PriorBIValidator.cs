using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
{
    public class PriorBIValidator : IValidator
    {
        public PriorBIValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid PriorBI
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var priorBIValue = leadEntity.Context.SingleOrDefault(item => item.Id ==Modules.LeadEntity.Interface.Constants.PropertyKeys.PriorBIKey)?.Value;
            if (priorBIValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("PriorBI Not In Context.\n");
                return false;
            }

            if (priorBIValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("PriorBI Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
