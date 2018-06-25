using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace LeadValidator
{
    using LeadEntity.Interface;
    using System;
    using System.IO;
    using Validator.Interface;

    public class LeadValidator : IValidator
    {
        public bool ValidLead(ILeadEntity lead)
        {
            if (lead.Context.Length < 0)
                return false;

            if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey) == null)
            {
                return false;
            }

            else if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey) == null)
            {
                return false;
            }

            else if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey) == null)
            {
                return false;
            }

            else if (lead.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey) == null)
            {
                return false;
            }

            else
            {
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

                else if (!Guid.TryParse(lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey)
                    .Select(i => i.Value).ToString(), out identityGuid))
                {
                    return false;
                }

                else if (!Guid.TryParse(lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey)
                    .Select(i => i.Value).ToString(), out sessionGuid))
                {
                    return false;
                }

                else if (!Int32.TryParse(lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey)
                    .Select(i => i.Value).ToString(), out quotedProduct))
                {
                    return false;
                }
            }
            return true;


        }
    }
}
