﻿namespace LMS.LeadCollector.Implementation
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LMS.LeadCollector.Interface;
    using LMS.Decorator.Interface;
    using LMS.Validator.Interface;
    using LMS.Publisher.Interface;
    using LMS.LoggerClient.Interface;
    using Newtonsoft.Json;
    using LMS.Modules.LeadEntity.Interface;
    using LMS.Modules.LeadEntity.Interface.Constants;
    using LMS.Modules.LeadEntity.Components;

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
            _loggerClient.Log(new DefaultLoggerClientObject {OperationContext = "Collected the Lead",ProcessContext = processContext,SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });

            // Create the results list

            var leadCollectorResultCollectionList = new List<IResult> { new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampStartKey, DateTime.Now) };

            _loggerClient.Log(new DefaultLoggerClientObject {OperationContext = "Validating the Lead",ProcessContext = processContext,SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });

            try
            {
                //If the lead is valid, decorate and publish 
                if (_leadValidator.ValidLead(leadEntity).Equals(true))
                {
                    leadCollectorResultCollectionList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

                    // Broadcast to the Campaigns
                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Publishing the Lead", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
                    _leadPublisher.PublishLead(leadEntity);
                    leadCollectorResultCollectionList.Add(new DefaultResult(ResultKeys.LeadCollectorKeys.PublisherStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

                    // Decorate
                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Decorating the Lead", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
                    leadCollectorResultCollectionList.Add(new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampEndKey, DateTime.Now));
                    _leadDecorator.DecorateLead(leadEntity, leadCollectorResultCollectionList);
                }
                else
                {
                    leadCollectorResultCollectionList.Add(new DefaultResult(ResultKeys.ValidatorStatusKey, ResultKeys.ResultKeysStatusEnum.Failed.ToString()));

                    // Decorate
                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Decorating the Lead", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
                    leadCollectorResultCollectionList.Add(new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampEndKey, DateTime.Now));
                    _leadDecorator.DecorateLead(leadEntity, leadCollectorResultCollectionList);

                    // TODO - Show the lead for demo Remove afterwards and move "//Decorate part below if stmnt
                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = JsonConvert.SerializeObject(leadEntity, Newtonsoft.Json.Formatting.Indented), ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });

                }


            }
            catch (Exception exception)
            {
                // Add results to Lead Entity
                _leadDecorator.DecorateLead(leadEntity, leadCollectorResultCollectionList);
                // Log an Error
                _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = "Collected the Lead", ProcessContext = processContext, SolutionContext = solutionContext, ErrorContext = exception.Message, Exception = exception, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Error});
                throw;
            }
  
     
        }

    }

}
