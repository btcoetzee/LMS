using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class IdentityGuidValidator : IValidator
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public IdentityGuidValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid IdentityGuid
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var identityGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == Modules.LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey)?.Value;
            if (identityGuidValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("IdentityGuid Not In Context.\n");
                return false;
            }

            if (!Guid.TryParse(identityGuidValue.ToString(), out Guid identityGuid))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("IdentityGuid Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
