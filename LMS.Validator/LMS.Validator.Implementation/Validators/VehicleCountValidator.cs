using System;
using System.Collections.Generic;
using System.Linq;
using LMS.Modules.LeadEntity.Interface;
using LMS.Validator.Interface;

namespace LMS.Validator.Implementation.Validators
{
    public class VehicleCountValidator : IValidator
    {
        public VehicleCountValidator() { }

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid VehicleCount
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            var vehicleCountValue = leadEntity.Context.SingleOrDefault(item => item.Id == Modules.LeadEntity.Interface.Constants.PropertyKeys.VehicleCountKey)?.Value;
            if (vehicleCountValue == null)
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("VehicleCount Not In Context.\n");
                return false;
            }

            if (!Int32.TryParse(vehicleCountValue.ToString(), out int vehicleCount))
            {
                if (leadEntity.ErrorList == null)
                    leadEntity.ErrorList = new List<string>();
                leadEntity.ErrorList.Add("VehicleCount Invalid.\n");
                return false;
            }
            return true;
        }
    }
}
