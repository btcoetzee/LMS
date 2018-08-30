using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
{
    public class AddressValidator : IValidator
    {
        public AddressValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid Address
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var addressValue = leadEntity.Context.SingleOrDefault(item => item.Id ==Modules.LeadEntity.Interface.Constants.PropertyKeys.AddressKey)?.Value;
            if (addressValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("Address Not In Context.\n");
                return false;
            }

            if (addressValue.ToString() == String.Empty)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("Address Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
