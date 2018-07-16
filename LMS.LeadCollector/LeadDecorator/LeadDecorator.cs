namespace LMS.LeadDecorator.Implementation
{
    using LMS.LeadEntity.Interface;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Components;
    using System;
    using System.Linq;
    using LMS.LeadEntity.Interface.Constants;
    using LMS.LoggerClient.Interface;


    public class LeadDecorator : IDecorator
    {

        private static ILoggerClient _loggerClient;
        ILeadEntity _leadEntity;
        private static IDecorator _notificationChannelPublisher;
        private static string solutionContext = "LeadDecorator";

        public LeadDecorator(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));            
        }

        
        public void DecorateLead(ILeadEntity lead)
        {
            string processContext = "DecorateLead";

            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Decorating the Lead",
                ProcessContext = processContext,
                SolutionContext = solutionContext
            });

            _leadEntity = lead;

            if (lead.Results == null)
                lead.Results = new IResults[0];

          
            var resultsList = lead.Results.ToList();
           
            
            resultsList.Add(new DefaultResult(ResultKeys.ResultTimeStampKey, DateTime.Now));
        }
    }
}
