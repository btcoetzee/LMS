using System;
using System.Collections.Generic;
using System.Linq;
using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class SessionGuidValidator : IValidator
    {
        public SessionGuidValidator() { }

        public bool ValidLead(ILeadEntity leadEntity)
        {
            var sessionGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey)?.Value;
            if (sessionGuidValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("SessionGuid Not In Context.\n");
                return false;
            }

            if (!Guid.TryParse(sessionGuidValue.ToString(), out Guid sessionGuid))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("SessionGuid Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
