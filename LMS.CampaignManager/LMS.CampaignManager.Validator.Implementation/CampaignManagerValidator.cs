namespace LMS.CampaignManager.Validator.Implementation
{
    using LMS.Validator.Interface;
    using LMS.LeadEntity.Interface;
 
    public class CampaignManagerValidator:IValidator
    {
        public bool ValidLead(ILeadEntity lead)
        {
            return true;

        }
    }
}
