using LMS.LeadEntity.Interface;
using LMS.Validator.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.DataProvider.Validators
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
            var vehicleCountValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.VehicleCountKey)?.Value;
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
