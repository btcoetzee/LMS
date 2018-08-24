using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
{
    public class PNIBirthDateValidator : IValidator
    {
        public PNIBirthDateValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid PNIBirthDate
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var pniBirthDateValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PNI_Age)?.Value;
            if (pniBirthDateValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("PNIBirthDate Not In Context.\n");
                return false;
            }

            if (!Int32.TryParse(pniBirthDateValue.ToString(), out int pniBirthDate))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("PNIBirthDate Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
