namespace LeadPublisher
{
    using LeadEntity.Interface;
    using Publisher.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Admiral.Components.Instrumentation.Contract;
    using Compare.Components.Notification.Contract;
    using Compare.Components.Notification.Publishers;
    using LMS.LoggerClient.Interface;
    using Moq;

    public class LeadPublisher : IPublisher
    {
        ILoggerClient  _loggerClient;
        ILeadEntity _leadEntity;
     
        private static IPublisher<string> _notificationPublisher;

        LeadPublisher(IPublisher<string> notificationPublisher, ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient;
            _notificationPublisher = notificationPublisher;
        }

        public void PublishLead(ILeadEntity leadEntity)
        {
            _leadEntity = leadEntity;
   
            _notificationPublisher.BroadcastMessage("Start Campaigns");

        }




 

 
    }
}




