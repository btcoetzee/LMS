﻿namespace LMS.DataProvider
{
    using LMS.LeadEntity.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public class ValidatorCollection
    {

        public static bool HasValidActivityGuid(ILeadEntity leadEntity)
        {
            var activityGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey)?.Value;
            if ((activityGuidValue == null) || (!Guid.TryParse(activityGuidValue.ToString(), out Guid activityGuid)))
            {
                return false;
            }
            // Implementation...
            return true;
        }

        public static bool HasValidIdentityGuid(ILeadEntity leadEntity)
        {
            var identityGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey)?.Value;
            if ((identityGuidValue == null) || (!Guid.TryParse(identityGuidValue.ToString(), out Guid identityGuid)))
            {
                return false;
            }
            // Implementation...
            return false;
        }
    }
}
