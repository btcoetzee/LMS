namespace LMS.LeadPublisher.Implementation
{
    using System;
    using Compare.Components.Notification.Contract;
    using LMS.Modules.LeadEntity.Interface;
    using LMS.LoggerClient.Interface;
    using Newtonsoft.Json;
    using Publisher.Interface;

    public class LeadPublisher : IPublisher
    {
       private static ILoggerClient  _loggerClient;
        ILeadEntity _leadEntity;
        private static IPublisher<ILeadEntity> _notificationChannelPublisher;
        private static string solutionContext = "LeadPublisher";

        public LeadPublisher(IPublisher<ILeadEntity> notificationChannelPublisher,  ILoggerClient loggerClient)
        {
            _notificationChannelPublisher = notificationChannelPublisher ?? throw new ArgumentNullException(nameof(notificationChannelPublisher));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        public void PublishLead(ILeadEntity leadEntity)
        {
            string processContext = "PublishLead";

            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Publishing the Lead",
                ProcessContext = processContext,
                SolutionContext = solutionContext,
                EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information
            });

            _leadEntity = leadEntity;

            // Pass leadEntity onto channel to be picked up by Subscribed Campaign Managers
            //notificationChannelPublisher.BroadcastMessage(JsonConvert.SerializeObject(leadEntity));
            _notificationChannelPublisher.BroadcastMessage(leadEntity);
        }

    }
}




