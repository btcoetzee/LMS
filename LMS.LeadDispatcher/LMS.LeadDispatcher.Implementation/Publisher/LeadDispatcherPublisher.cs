using System;
using System.Collections.Generic;
using System.Text;
using Compare.Components.Notification.Contract;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;
using Newtonsoft.Json;

namespace LMS.LeadDispatcher.Implementation.Publisher
{
    public class LeadDispatcherPublisher : IPublisher
    {
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "LeadDispatcherPublisher";
        private static IPublisher<ILeadEntity> _notificationChannelPublisher;

        public LeadDispatcherPublisher(IPublisher<ILeadEntity> notificationChannelPublisher, ILoggerClient loggerClient)
        {
            _notificationChannelPublisher = notificationChannelPublisher ?? throw new ArgumentNullException(nameof(notificationChannelPublisher));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        /// <summary>
        /// Publish Lead to ...... POE? 
        /// </summary>
        /// <param name="leadEntity"></param>
        public void PublishLead(ILeadEntity leadEntity)
        {
            var processContext = "PublishLead";
            // TODO - this will publish to a component outside of LMS
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = JsonConvert.SerializeObject(leadEntity, Newtonsoft.Json.Formatting.Indented), ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
            // Pass leadEntity onto channel to be picked up by Entity sending the notification
            _notificationChannelPublisher.BroadcastMessage(leadEntity);
        }
    }
}
