using System;
using System.Collections.Generic;
using System.Text;
using Compare.Components.Notification.Contract;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace LMS.LeadDispatcher.Implementation.Subscriber
{
    /// <summary>
    /// This Subscriber class for the Lead Dispatcher is set up to listen when a Lead Entity is published on the 
    /// Notification Channel by the Campaign Manager.  The Leads that were published are then processed through 
    /// the LeadDepatcher code.
    /// </summary>
    public class LeadDispatcherSubscriber : ISubscriber
    {
        private readonly ISubscriber<ILeadEntity> _notificationSubscriber;
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "LeadDispatcherSubscriber";

        public LeadDispatcherSubscriber(ISubscriber<ILeadEntity> notificationSubscriber, ILoggerClient loggerClient)
        {
            _notificationSubscriber = notificationSubscriber ?? throw new ArgumentNullException(nameof(notificationSubscriber));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }
        public void SetupAddOnReceiveActionToChannel(Action<ILeadEntity> receiveAction)
        {
            var processContext = "SetupAddOnReceiveActionToChannel";
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Setting up the function to call when LeadDispatcher subscriber receives a message.", ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

            _notificationSubscriber.AddOnReceiveActionToChannel(receiveAction);
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Finished setting up the function to call when LeadDispatcher subscriber receives a message.", ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

        }
    }
}
