namespace LMS.CampaignManager.Subscriber.Implementation
{
    using System;
    using Compare.Components.Notification.Contract;
    using LMS.CampaignManager.Interface;
    using LMS.CampaignManager.Subscriber.Interface;
    using LMS.LoggerClient.Interface;
    using LMS.Modules.LeadEntity.Interface;

    public class CampaignManagerSubscriber : ICampaignManagerSubscriber
    {
        private readonly ISubscriber<ILeadEntity> _notificationSubscriber;
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "CampaignManagerSubscriber";

        public CampaignManagerSubscriber(ISubscriber<ILeadEntity> notificationSubscriber, ILoggerClient loggerClient)
        {
            _notificationSubscriber = notificationSubscriber ?? throw new ArgumentNullException(nameof(notificationSubscriber));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }
        
        /// <summary>
        /// When a lead is received on the Notification Channel, the Campaign Manager 
        /// process will be executed.  See Campaign Manager constructor.
        /// </summary>
        /// <param name="receiveAction"></param>
        public void SetupAddOnReceiveActionToChannel(Action<ILeadEntity> receiveAction)
        {
            var processContext = "SetupAddOnReceiveActionToChannel";
            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Setting up the function to call when subscriber receives a message.",ProcessContext = processContext,SolutionContext = SolutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });

            _notificationSubscriber.AddOnReceiveActionToChannel(receiveAction);
            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Finished setting up the function to call when subscriber receives a message.",ProcessContext = processContext,SolutionContext = SolutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information });
        }

}
}
