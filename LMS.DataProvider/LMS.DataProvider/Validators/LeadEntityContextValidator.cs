using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.DataProvider.Validators
{
    public class LeadEntityContextValidator : IValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeadEntityContextValidator"/> class.
        /// </summary>
        public LeadEntityContextValidator() { }

        public bool ValidLead(ILeadEntity leadEntity)
        {
            if (leadEntity.Context == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("LeadEntity Context is null.\n");

                return false;
            }

            return true;
        }
    }
}
