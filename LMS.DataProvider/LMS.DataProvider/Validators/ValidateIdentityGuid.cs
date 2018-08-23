namespace LMS.DataProvider.ValidatorCollection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LMS.LeadEntity.Interface;
    using LMS.Validator.Interface;

    public class ValidateIdentityGuid : IValidator
    {
        private string _errorMessage;

        /// <summary>
        /// Constructor
        /// </summary>
        public ValidateIdentityGuid() => _errorMessage = String.Empty;

        /// <summary>
        /// Validate the Lead by checking that the Lead has a non empty/null and valid ActitivityGuid
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            if ((leadEntity.Context == null) || (leadEntity.Context.Length < 0))
            {
                _errorMessage = "Context is null or empty. \n";
                return false;
            }
            var identityGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey)?.Value;
            if ((identityGuidValue == null) || (!Guid.TryParse(identityGuidValue.ToString(), out Guid identityGuid)))
            {
                _errorMessage = "IdentityGuid Invalid or Not In Context \n";
                return false;
            }
            return true;
        }

        public string ErrorMessage => _errorMessage;
    }
}
