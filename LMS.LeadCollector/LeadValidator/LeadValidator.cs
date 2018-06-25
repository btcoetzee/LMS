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
            else
            {
                // Check the value here.
                var guid = lead.Context.Where(ac => ac.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey)
                    .Select(i => i.Value).ToString();
                if (guid.ToString() == null || guid == Guid.Empty.ToString())
                {
                    return false;
                }
            }
            return true;


        }
    }
}
