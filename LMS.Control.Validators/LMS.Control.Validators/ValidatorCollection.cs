namespace LMS.Control.Validators
{
    using System;
    using System.Linq;
    using LMS.LeadEntity.Interface;

    public class ValidatorCollection
    {
        public bool HasValidActivityGuid(ILeadEntity leadEntity)
        {

            var activityGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.ActivityGuidKey)?.Value;
            if ((activityGuidValue == null) || (!Guid.TryParse(activityGuidValue.ToString(), out Guid activityGuid)))
            {
                return false;
            }

                return true;
        }

        public bool HasValidSessionGuid(ILeadEntity leadEntity)
        {
            var sessionGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionGuidKey)?.Value;
            if ((sessionGuidValue == null) || (!Guid.TryParse(sessionGuidValue.ToString(), out Guid sessionGuid)))
            {
                return false;
            }
            
            return true;
        }

        public bool HasValidIdentityGuid(ILeadEntity leadEntity)
        {
            var identityGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.IdentityGuidKey)?.Value;
            if ((identityGuidValue == null) || (!Guid.TryParse(identityGuidValue.ToString(), out Guid identityGuid)))
            {
                return false;
            }

            return true;
        } 

        public bool HasValidQuotedProduct(ILeadEntity leadEntity)
        {
            var quotedProductValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.QuotedProductKey)?.Value;
            if ((quotedProductValue == null) || (!Int32.TryParse(quotedProductValue.ToString(), out int quotedProduct)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidAdditionalProducts(ILeadEntity leadEntity)
        {
            var additionalProductValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.AdditionalProductKey)?.Value;
            if ((additionalProductValue == null) || (!Int32.TryParse(additionalProductValue.ToString(), out int additionalProduct)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidPriorInsurance(ILeadEntity leadEntity)
        {
            var priorInsuranceValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PriorInsuranceKey)?.Value;
            if ((priorInsuranceValue == null) || (!bool.TryParse(priorInsuranceValue.ToString(), out bool priorInsurance)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidPriorBI(ILeadEntity leadEntity)
        {
            var priorBIValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PriorBIKey)?.Value;
            if ((priorBIValue == null) || (!Int32.TryParse(priorBIValue.ToString(), out int priorBI)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidVehicleCount(ILeadEntity leadEntity)
        {
            var vehicleCountValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.VehicleCountKey)?.Value;
            if ((vehicleCountValue == null) || (!Int32.TryParse(vehicleCountValue.ToString(), out int vehicleCount)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidPNIAge(ILeadEntity leadEntity)
        {
            var pniAgeValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PNI_Age)?.Value;
            if ((pniAgeValue == null) || (!Int32.TryParse(pniAgeValue.ToString(), out int pniAge)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidQuotedBI(ILeadEntity leadEntity)
        {
            var quotedBIValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.QuotedBIKey)?.Value;
            if ((quotedBIValue == null) || (!Int32.TryParse(quotedBIValue.ToString(), out int quotedBI)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidPhoneNumber(ILeadEntity leadEntity)
        {
            var phoneNumberValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.PhoneNumber)?.Value;
            if ((phoneNumberValue == null) || (!Int32.TryParse(phoneNumberValue.ToString(), out int phoneNumber)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidHighPOP(ILeadEntity leadEntity)
        {
            var highPOPValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.SegementKeys.HighPOPKey)?.Value;
            if ((highPOPValue == null) || (!Int32.TryParse(highPOPValue.ToString(), out int highPOP)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidHomeOwner(ILeadEntity leadEntity)
        {
            var homeOwnerValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.SegementKeys.HomeownerKey)?.Value;
            if ((homeOwnerValue == null) || (!Int32.TryParse(homeOwnerValue.ToString(), out int homeOwner)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidSessionRequestSeq(ILeadEntity leadEntity)
        {
            var sessionRequestSeqValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SessionRequestSeqKey)?.Value;
            if ((sessionRequestSeqValue == null) || (!Int32.TryParse(sessionRequestSeqValue.ToString(), out int sessionRequestSeq)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidSiteID(ILeadEntity leadEntity)
        {
            var siteIDValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.SiteIDKey)?.Value;
            if ((siteIDValue == null) || (!Int32.TryParse(siteIDValue.ToString(), out int siteId)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidEnvironment(ILeadEntity leadEntity)
        {
            var environmentValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ContextKeys.EnvironmentKey)?.Value;
            if ((environmentValue == null) || (!Int32.TryParse(environmentValue.ToString(), out int environment)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidEmailAddress(ILeadEntity leadEntity)
        {
            var emailAddressValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.EmailAddressKey)?.Value;
            if ((emailAddressValue == null) || (!Int32.TryParse(emailAddressValue.ToString(), out int emailAddress)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidState(ILeadEntity leadEntity)
        {
            var stateValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.StateKey)?.Value;
            if ((stateValue == null) || (!Int32.TryParse(stateValue.ToString(), out int state)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidFullName(ILeadEntity leadEntity)
        {
            var fullNameValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.FullNameKey)?.Value;
            if ((fullNameValue == null) || (!Int32.TryParse(fullNameValue.ToString(), out int fullName)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidAddress(ILeadEntity leadEntity)
        {
            var addressValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.PropertyKeys.AddressKey)?.Value;
            if ((addressValue == null) || (!Int32.TryParse(addressValue.ToString(), out int address)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidLowPOP(ILeadEntity leadEntity)
        {
            var lowPOPValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.SegementKeys.LowPOPKey)?.Value;
            if ((lowPOPValue == null) || (!Int32.TryParse(lowPOPValue.ToString(), out int lowPOP)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidRenter(ILeadEntity leadEntity)
        {
            var renterValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.SegementKeys.RenterKey)?.Value;
            if ((renterValue == null) || (!Int32.TryParse(renterValue.ToString(), out int renter)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidQuoteRef(ILeadEntity leadEntity)
        {
            var quoteRefValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.QuoteReferenceKey)?.Value;
            if ((quoteRefValue == null) || (!Int32.TryParse(quoteRefValue.ToString(), out int quoteRef)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidAnnualPremium(ILeadEntity leadEntity)
        {
            var annualPremiumValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.AnnualPremiumKey)?.Value;
            if ((annualPremiumValue == null) || (!Int32.TryParse(annualPremiumValue.ToString(), out int annualPremium)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidDownPayment(ILeadEntity leadEntity)
        {
            var downPaymentValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.DownPaymentKey)?.Value;
            if ((downPaymentValue == null) || (!Int32.TryParse(downPaymentValue.ToString(), out int downPayment)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidMonthlyInstallment(ILeadEntity leadEntity)
        {
            var monthlyInstallmentValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.MonthlyInstallmentKey)?.Value;
            if ((monthlyInstallmentValue == null) || (!Int32.TryParse(monthlyInstallmentValue.ToString(), out int monthlyInstallment)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidRequote(ILeadEntity leadEntity)
        {
            var requoteValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.RequoteKey)?.Value;
            if ((requoteValue == null) || (!Int32.TryParse(requoteValue.ToString(), out int requote)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidBuyOnlineClick(ILeadEntity leadEntity)
        {
            var buyOnlineClickValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.BuyOnlineClickKey)?.Value;
            if ((buyOnlineClickValue == null) || (!Int32.TryParse(buyOnlineClickValue.ToString(), out int buyOnlineClick)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidBuyOnlineBrand(ILeadEntity leadEntity)
        {
            var buyOnlineBrandValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.BuyOnlineBrandKey)?.Value;
            if ((buyOnlineBrandValue == null) || (!Int32.TryParse(buyOnlineBrandValue.ToString(), out int buyOnlineBrand)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidBuyByPhoneClick(ILeadEntity leadEntity)
        {
            var buyByPhoneClickValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.BuyByPhoneClickKey)?.Value;
            if ((buyByPhoneClickValue == null) || (!Int32.TryParse(buyByPhoneClickValue.ToString(), out int buyByPhoneClick)))
            {
                return false;
            }

            return true;
        }

        public bool HasValidBuyByPhoneBrand(ILeadEntity leadEntity)
        {
            var buyByPhoneBrandValue = leadEntity.Context.SingleOrDefault(item => item.Id == LeadEntity.Interface.Constants.ActivityKeys.BuyByPhoneBrandKey)?.Value;
            if ((buyByPhoneBrandValue == null) || (!Int32.TryParse(buyByPhoneBrandValue.ToString(), out int buyByPhoneBrand)))
            {
                return false;
            }

            return true;
        }

    }
}
