namespace LMS.LeadCollector.Implementation
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LMS.LeadCollector.Interface;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.Validator.Interface;
    using LMS.Publisher.Interface;
    using LMS.LoggerClient.Interface;
    using LMS.LeadEntity.Components;
    using LMS.LeadEntity.Interface.Constants;

    public class LeadCollector : ILeadCollector
    {
        /// <summary>
        /// Defining the Members
        /// </summary>
        private readonly IValidator _leadValidator;
        private readonly IDecorator _leadDecorator;
        private readonly IPublisher _leadPublisher;
        private readonly ILoggerClient _loggerClient;
        private static string solutionContext = "LeadCollector";

        /// <summary>
        /// Constructor for LeadCollector
        /// </summary>
        /// <param name="leadValidator"></param>
        /// <param name="leadDecorator"></param>
        /// <param name="leadPublisher"></param>
        /// <param name="loggerClient"></param>
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
        /// <param name="leadEntity"></param>
        public void CollectLead(ILeadEntity leadEntity)
        {

            if (leadEntity == null) throw new ArgumentNullException(nameof(leadEntity));
            string processContext = "CollectLead";
            _loggerClient.Log(new DefaultLoggerClientObject {OperationContext = "Collected the Lead",ProcessContext = processContext,SolutionContext = solutionContext});

            // Create the results list

            var leadCollectorResultCollectionList = new List<IResult> { new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampStartKey, DateTime.Now) };

            _loggerClient.Log(new DefaultLoggerClientObject {OperationContext = "Validating the Lead",ProcessContext = processContext,SolutionContext = solutionContext});

            try
            {
                //If the lead is valid, decorate and publish 
                if (_leadValidator.ValidLead(leadEntity).Equals(true))
                {
                    leadCollectorResultCollectionList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

                    // Broadcast to the Campaigns
                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Publishing the Lead", ProcessContext = processContext, SolutionContext = solutionContext });
                    _leadPublisher.PublishLead(leadEntity);
                    leadCollectorResultCollectionList.Add(new DefaultResult(ResultKeys.LeadCollectorKeys.PublisherStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));
                }
                else
                {
                    leadCollectorResultCollectionList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));
                }

                // Decorate
                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Decorating the Lead", ProcessContext = processContext, SolutionContext = solutionContext });
                leadCollectorResultCollectionList.Add(new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampEndKey, DateTime.Now));
                _leadDecorator.DecorateLead(leadEntity, leadCollectorResultCollectionList);
            }
            catch (Exception exception)
            {
                // Add results to Lead Entity
                _leadDecorator.DecorateLead(leadEntity, leadCollectorResultCollectionList);
                // Log an Error
                _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = "Collected the Lead", ProcessContext = processContext, SolutionContext = solutionContext, ErrorContext = exception.Message, Exception = exception});
                throw;
            }
  
     
        }

    }

}
