namespace LeadCollector
{
    using LeadEntity.Interface;
    using System;

    public class LeadCollector
    { 
        /// <summary>
        /// calling the classes here
        /// </summary>
        private readonly LeadValidator.LeadValidator _validLead;
        private readonly LeadDecorator.LeadDecorator _decorateLead;
        private readonly LeadPublisher.LeadPublisher _publishLead;

        void CollectLead(ILeadEntity lead)
        {
           //This is where we send the lead through the LeadValidator,LeadDecorator, and the LeadPublisher
            _validLead.ValidLead(lead);
            _decorateLead.DecorateLead(lead);
            _publishLead.PublishLead(lead);
        }
    }

}
