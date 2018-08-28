using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class PriorInsuranceValidator : IValidator
    {
        public PriorInsuranceValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid PriorInsurance
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var priorInsuranceValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PriorInsuranceKey)?.Value;
            if (priorInsuranceValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("PriorInsurance Not In Context.\n");
                return false;
            }

            if (!bool.TryParse(priorInsuranceValue.ToString(), out bool priorInsurance))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("PriorInsurance Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
