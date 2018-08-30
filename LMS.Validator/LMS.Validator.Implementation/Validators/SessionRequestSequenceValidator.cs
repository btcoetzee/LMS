using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    class SessionRequestSequenceValidator : IValidator
    {
        public SessionRequestSequenceValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid SessionRequestSequence
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var sessionRequestSequenceValue = leadEntity.Context.SingleOrDefault(item => item.Id == Modules.LeadEntity.Interface.Constants.ContextKeys.SessionRequestSeqKey)?.Value;
            if (sessionRequestSequenceValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("SessionRequestSequence Not In Context.\n");
                return false;
            }

            if (!Int32.TryParse(sessionRequestSequenceValue.ToString(), out int sessionRequestSequence))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("SessionRequestSequence Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
