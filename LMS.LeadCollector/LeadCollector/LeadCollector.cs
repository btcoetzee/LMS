namespace LeadCollector
{
    using System;
    using Decorator.Interface;
    using LeadEntity.Interface;
    using Publisher.Interface;
    using Validator.Interface;
    using Campaign.Interface;
    using Resolution.Interface;


    public class LeadCollector
    {
        /// <summary>
        /// Defining the Members
        /// </summary>
        private readonly IValidator _leadValidator;
        private readonly IDecorator _leadDecorator;
        private readonly IPublisher _leadPublisher;
        private readonly IResolution _leadResolver;

        /// <summary>
        /// Constructor for LeadCollector
        /// </summary>
        /// <param name="leadValidator"></param>
        /// <param name="leadDecorator"></param>
        /// <param name="leadPublisher"></param>
        /// <param name="leadResolver"></param>
        public LeadCollector(IValidator leadValidator, IDecorator leadDecorator, IPublisher leadPublisher,
            IResolution leadResolver)
        {

            _leadValidator = leadValidator ?? throw new ArgumentNullException(nameof(leadValidator));
            _leadDecorator = leadDecorator ?? throw new ArgumentNullException(nameof(leadDecorator));
            _leadPublisher = leadPublisher ?? throw new ArgumentNullException(nameof(leadPublisher));
            _leadResolver = leadResolver ?? throw new ArgumentNullException(nameof(leadResolver));
        }

        /// <summary>
        /// Process the Lead Collected
        /// </summary>
        /// <param name="lead"></param>
        void CollectLead(ILeadEntity lead)
        {
            //If the lead is valid, decorate and publish 
            if (_leadValidator.ValidLead(lead).Equals(true))
            {
                // Decorate
                _leadDecorator.DecorateLead(lead);

                // Broadcast to the Campaigns
                _leadPublisher.PublishLead(lead);

                // Apply Lead Resolution - will need to pass taskQueue
                _leadResolver.ResolveLead(lead);

            }
        }

    }

}
