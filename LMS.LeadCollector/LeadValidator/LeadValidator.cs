namespace LMS.LeadValidator.Implementation
{
    using System.ComponentModel;
    using System.Linq;
    using System;
    using System.IO;
    using LMS.Validator.Interface;
    using LMS.LeadEntity.Interface;

    public class LeadValidator : IValidator
    {
        //public bool ValidLead(LMS.LeadEntity.Interface.ILeadEntity lead)
        public bool ValidLead(LMS.LeadEntity.Interface.ILeadEntity lead)
        {
            if (lead.Context.Length < 0)
                return false;

            if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey) == null)
            {
                return false;
            }

            if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey) == null)
            {
                return false;
            }

            if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey) == null)
            {
                return false;
            }

            if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey) == null)
            {
                return false;
            }

            // Check the value here.
            Guid activityGuid;
            Guid identityGuid;
            Guid sessionGuid;
            int quotedProduct;

            if (!Guid.TryParse(lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey)
                .Select(i => i.Value).ToString(), out activityGuid))
            {
                return false;
            }

             if (!Guid.TryParse(lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey)
                .Select(i => i.Value).ToString(), out identityGuid))
            {
                return false;
            }

             if (!Guid.TryParse(lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey)
                .Select(i => i.Value).ToString(), out sessionGuid))
            {
                return false;
            }

             if (!Int32.TryParse(lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey)
                .Select(i => i.Value).ToString(), out quotedProduct))
            {
                return false;


            }
            // try tryParse on guid to see if valid guid.

            return true;

        }
    
    }
}
