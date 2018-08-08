namespace LMS.CampaignManager.Decorator.Implementation
{
    using System;
    using System.Collections.Generic;
    using LMS.CampaignManager.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using System.Linq;
    using LMS.LeadEntity.Components;
    using LMS.LeadEntity.Interface.Constants;
    using LMS.LoggerClient.Interface;

    public class CampaignManagerDecorator: ICampaignManagerDecorator
    {
        private static ILoggerClient _loggerClient;
        private static string solutionContext = "CampaignManagerDecorator";
        public CampaignManagerDecorator(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }
        /// <summary>
        /// Decorate the Lead by adding the Campaign Manager Results List to the Results Collection
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <param name="campaignManagerResultList"></param>
        public void DecorateLead(ILeadEntity leadEntity, List<IResult> campaignManagerResultList)
        {
            if (leadEntity == null) throw new ArgumentNullException(nameof(leadEntity));
            if (campaignManagerResultList?.Any() == true)
            {
                string processContext = "DecorateLead";

                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Decorating the Lead", ProcessContext = processContext, SolutionContext = solutionContext });

                // The Results Collection was not created yet - shouldn't happen
                if (leadEntity.ResultCollection == null)
                    leadEntity.ResultCollection = new DefaultResultCollection();


                campaignManagerResultList.Add(new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampEndKey, DateTime.Now));

                campaignManagerResultList.Add(new DefaultResult(ResultKeys.DecoratorStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

                // If there has been CampaignManager Decoration
                if (leadEntity.ResultCollection.CampaignManagerCollection != null)
                    campaignManagerResultList.AddRange(leadEntity.ResultCollection.CampaignManagerCollection.ToList());

                // Assign all Campaign Manager Results that were Recorded
                leadEntity.ResultCollection.CampaignManagerCollection = campaignManagerResultList.ToArray();

            }

         
        }
        
    }
}
