namespace LeadCollector
{
    using LeadEntity.Interface;
    using System;

    public class LeadCollector
    { 
        public LeadCollector()
        {
            _validLead = new LeadValidator.LeadValidator();
            _decorateLead = new LeadDecorator.LeadDecorator();
            _publishLead = new LeadPublisher.LeadPublisher();
        }

        public LeadCollector(LeadValidator.LeadValidator leadValidator, LeadDecorator.LeadDecorator leadDecorator, LeadPublisher.LeadPublisher leadPublisher)
        {
            _validLead = leadValidator;
            _decorateLead = leadDecorator;
            _publishLead = leadPublisher;
            
        }
        /// <summary>
        /// calling the classes here
        /// </summary>
        private readonly LeadValidator.LeadValidator _validLead;
        private readonly LeadDecorator.LeadDecorator _decorateLead;
        private readonly LeadPublisher.LeadPublisher _publishLead;
       
        void CollectLead(ILeadEntity lead)
        {
           //This is where we send the lead through the LeadValidator,LeadDecorator, and the LeadPublisher

            
            if (_validLead.ValidLead(lead).Equals(true))
            { 
                _decorateLead.DecorateLead(lead);
                _publishLead.PublishLead(lead);
            }
        }
    }

}
