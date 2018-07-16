namespace LMS.LeadCollector.Implementation
{
    using LMS.LeadCollector.Interface;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.Validator.Interface;
    using LMS.Publisher.Interface;
    using System;
    using LMS.LoggerClient.Interface;

    public class LeadCollector : ILeadCollector
    {
        /// <summary>
        /// Defining the Members
        /// </summary>
        private readonly IValidator _leadValidator;
        private readonly IDecorator _leadDecorator;
        private readonly IPublisher _leadPublisher;
        private readonly ILoggerClient _loggerClient;
        private static ILeadCollector _notificationChannelPublisher;
        private static string solutionContext = "LeadCollector";

        /// <summary>
        /// Constructor for LeadCollector
        /// </summary>
        /// <param name="leadValidator"></param>
        /// <param name="leadDecorator"></param>
        /// <param name="leadPublisher"></param>
        /// <param name="leadResolver"></param>
        public LeadCollector(IValidator leadValidator, IDecorator leadDecorator, IPublisher leadPublisher, ILoggerClient loggerClient)
        {

            _leadValidator = leadValidator ?? throw new ArgumentNullException(nameof(leadValidator));
            _leadDecorator = leadDecorator ?? throw new ArgumentNullException(nameof(leadDecorator));
            _leadPublisher = leadPublisher ?? throw new ArgumentNullException(nameof(leadPublisher));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

        }

        /// <summary>
        /// Process the Lead Collected
        /// </summary>
        /// <param name="lead"></param>
        public void CollectLead(ILeadEntity lead)
        {

            string processContext = "CollectLead";

            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Validating the Lead",
                ProcessContext = processContext,
                SolutionContext = solutionContext
            });
            //If the lead is valid, decorate and publish 
            if (_leadValidator.ValidLead(lead).Equals(true))
            {
                _loggerClient.Log(new DefaultLoggerClientObject
                {
                    OperationContext = "Decorating the Lead",
                    ProcessContext = processContext,
                    SolutionContext = solutionContext
                });
                // Decorate
                _leadDecorator.DecorateLead(lead);

                // Broadcast to the Campaigns
                _leadPublisher.PublishLead(lead);

               
            }
        }

    }

}
