namespace LMS.Control.Validators
{
    using System;
    using System.Linq;
    using LMS.LeadEntity.Interface;

    public class ValidatorCollection
    {
        public bool hasActivityGuid(ILeadEntity leadEntity)
        {

            var activityGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey)?.Value;
            if ((activityGuidValue == null) || (!Guid.TryParse(activityGuidValue.ToString(), out Guid activityGuid)))
            {
                return false;
            }

                return true;
        }

        public bool hasSessionGuid(ILeadEntity leadEntity)
        {
            var sessionGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey)?.Value;
            if ((sessionGuidValue == null) || (!Guid.TryParse(sessionGuidValue.ToString(), out Guid sessionGuid)))
            {
                return false;
            }
            
            return true;
        }

        public bool hasIdentityGuid(ILeadEntity leadEntity)
        {
            var identityGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey)?.Value;
            if ((identityGuidValue == null) || (!Guid.TryParse(identityGuidValue.ToString(), out Guid identityGuid)))
            {
                return false;
            }

            return true;
        } 

        public bool hasQuotedProduct(ILeadEntity leadEntity)
        {
            var quotedProductValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey)?.Value;
            if ((quotedProductValue == null) || (!Int32.TryParse(quotedProductValue.ToString(), out int quotedProduct)))
            {
                return false;
            }

            return true;
        }

        public bool hasAdditionalProducts(ILeadEntity leadEntity)
        {
            var additionalProductValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.AdditionalProductKey)?.Value;
            if ((additionalProductValue == null) || (!Int32.TryParse(additionalProductValue.ToString(), out int additionalProduct)))
            {
                return false;
            }

            return true;
        }

        public bool hasPriorInsurance(ILeadEntity leadEntity)
        {
            var priorInsuranceValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PriorInsuranceKey)?.Value;
            if ((priorInsuranceValue == null) || (!bool.TryParse(priorInsuranceValue.ToString(), out bool priorInsurance)))
            {
                return false;
            }

            return true;
        }

        public bool hasPriorBI(ILeadEntity leadEntity)
        {
            var priorBIValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PriorBIKey)?.Value;
            if ((priorBIValue == null) || (!Int32.TryParse(priorBIValue.ToString(), out int priorBI)))
            {
                return false;
            }

            return true;
        }

        public bool hasVehicleCount(ILeadEntity leadEntity)
        {
            var vehicleCountValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.VehicleCountKey)?.Value;
            if ((vehicleCountValue == null) || (!Int32.TryParse(vehicleCountValue.ToString(), out int vehicleCount)))
            {
                return false;
            }

            return true;
        }

        public bool hasPNIAge(ILeadEntity leadEntity)
        {
            var pniAgeValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PNI_Age)?.Value;
            if ((pniAgeValue == null) || (!Int32.TryParse(pniAgeValue.ToString(), out int pniAge)))
            {
                return false;
            }

            return true;
        }

        public bool hasQuotedBI(ILeadEntity leadEntity)
        {
            var quotedBIValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.QuotedBIKey)?.Value;
            if ((quotedBIValue == null) || (!Int32.TryParse(quotedBIValue.ToString(), out int quotedBI)))
            {
                return false;
            }

            return true;
        }

        public bool hasPhoneNumber(ILeadEntity leadEntity)
        {
            var phoneNumberValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber)?.Value;
            if ((phoneNumberValue == null) || (!Int32.TryParse(phoneNumberValue.ToString(), out int phoneNumber)))
            {
                return false;
            }

            return true;
        }

        public bool hasHighPOP(ILeadEntity leadEntity)
        {
            var highPOPValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.SegementKeys.HighPOPKey)?.Value;
            if ((highPOPValue == null) || (!Int32.TryParse(highPOPValue.ToString(), out int highPOP)))
            {
                return false;
            }

            return true;
        }

        public bool hasHomeOwner(ILeadEntity leadEntity)
        {
            var homeOwnerValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.SegementKeys.HomeownerKey)?.Value;
            if ((homeOwnerValue == null) || (!Int32.TryParse(homeOwnerValue.ToString(), out int homeOwner)))
            {
                return false;
            }

            return true;
        }

        public bool hasSessionRequestSeq(ILeadEntity leadEntity)
        {
            var sessionRequestSeqValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionRequestSeqKey)?.Value;
            if ((sessionRequestSeqValue == null) || (!Int32.TryParse(sessionRequestSeqValue.ToString(), out int sessionRequestSeq)))
            {
                return false;
            }

            return true;
        }

        public bool hasSiteID(ILeadEntity leadEntity)
        {
            var siteIDValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SiteIDKey)?.Value;
            if ((siteIDValue == null) || (!Int32.TryParse(siteIDValue.ToString(), out int siteId)))
            {
                return false;
            }

            return true;
        }

        public bool hasEnvironment(ILeadEntity leadEntity)
        {
            var environmentValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.EnvironmentKey)?.Value;
            if ((environmentValue == null) || (!Int32.TryParse(environmentValue.ToString(), out int environment)))
            {
                return false;
            }

            return true;
        }

        public bool hasEmailAddress(ILeadEntity leadEntity)
        {
            var emailAddressValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.EmailAddressKey)?.Value;
            if ((emailAddressValue == null) || (!Int32.TryParse(emailAddressValue.ToString(), out int emailAddress)))
            {
                return false;
            }

            return true;
        }

        public bool hasState(ILeadEntity leadEntity)
        {
            var stateValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.StateKey)?.Value;
            if ((stateValue == null) || (!Int32.TryParse(stateValue.ToString(), out int state)))
            {
                return false;
            }

            return true;
        }

        public bool hasFullName(ILeadEntity leadEntity)
        {
            var fullNameValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.FullNameKey)?.Value;
            if ((fullNameValue == null) || (!Int32.TryParse(fullNameValue.ToString(), out int fullName)))
            {
                return false;
            }

            return true;
        }

        public bool hasAddress(ILeadEntity leadEntity)
        {
            var addressValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.AddressKey)?.Value;
            if ((addressValue == null) || (!Int32.TryParse(addressValue.ToString(), out int address)))
            {
                return false;
            }

            return true;
        }

        public bool hasLowPOP(ILeadEntity leadEntity)
        {
            var lowPOPValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.SegementKeys.LowPOPKey)?.Value;
            if ((lowPOPValue == null) || (!Int32.TryParse(lowPOPValue.ToString(), out int lowPOP)))
            {
                return false;
            }

            return true;
        }

        public bool hasRenter(ILeadEntity leadEntity)
        {
            var renterValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.SegementKeys.RenterKey)?.Value;
            if ((renterValue == null) || (!Int32.TryParse(renterValue.ToString(), out int renter)))
            {
                return false;
            }

            return true;
        }

    }
}
