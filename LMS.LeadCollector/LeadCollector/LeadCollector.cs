namespace LMS.LeadCollector.Implementation
{
    using LMS.LeadCollector.Interface;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.Validator.Interface;
    using LMS.Publisher.Interface;
    using LMS.Resolution.Interface;
    using System;
  

    public class LeadCollector : ILeadCollector
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
        public LeadCollector(IValidator leadValidator, IDecorator leadDecorator, IPublisher leadPublisher)
        {

            _leadValidator = leadValidator ?? throw new ArgumentNullException(nameof(leadValidator));
            _leadDecorator = leadDecorator ?? throw new ArgumentNullException(nameof(leadDecorator));
            _leadPublisher = leadPublisher ?? throw new ArgumentNullException(nameof(leadPublisher));
        }

        /// <summary>
        /// Process the Lead Collected
        /// </summary>
        /// <param name="lead"></param>
        public void CollectLead(ILeadEntity lead)
        {
            //If the lead is valid, decorate and publish 
            if (_leadValidator.ValidLead(lead).Equals(true))
            {
                // Decorate
                _leadDecorator.DecorateLead(lead);

                // Broadcast to the Campaigns
                _leadPublisher.PublishLead(lead);
            }
        }

    }

}
