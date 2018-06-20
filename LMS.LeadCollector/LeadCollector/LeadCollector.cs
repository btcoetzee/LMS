namespace LeadCollector
{
    using LeadEntity.Interface;
    using System;

    public class LeadCollector
    {
        /// <summary>
        /// The valid lead
        /// </summary>
        private readonly LeadValidator.LeadValidator _validLead;
        private readonly LeadDecorator.LeadDecorator _decorateLead;
        private readonly LeadPublisher.LeadPublisher _publishLead;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeadCollector"/> class.
        /// </summary>
        public LeadCollector()
        {
            _validLead = new LeadValidator.LeadValidator();
            _decorateLead = new LeadDecorator.LeadDecorator();
            _publishLead = new LeadPublisher.LeadPublisher();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeadCollector"/> class.
        /// </summary>
        /// <param name="leadValidator">The lead validator.</param>
        /// <param name="leadDecorator">The lead decorator.</param>
        /// <param name="leadPublisher">The lead publisher.</param>
        public LeadCollector(LeadValidator.LeadValidator leadValidator, LeadDecorator.LeadDecorator leadDecorator, LeadPublisher.LeadPublisher leadPublisher)
        {
            _validLead = leadValidator;
            _decorateLead = leadDecorator;
            _publishLead = leadPublisher;

        }

        /// <summary>
        /// Collects the lead.
        /// </summary>
        /// <param name="lead">The lead.</param>
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
