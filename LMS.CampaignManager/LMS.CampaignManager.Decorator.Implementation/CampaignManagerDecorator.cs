using System.Linq;
using LMS.LeadEntity.Components;
using LMS.LeadEntity.Interface.Constants;
using LMS.LoggerClient.Interface;

namespace LMS.CampaignManager.Decorator.Implementation
{
    using System;
    using LMS.Decorator.Interface;
    using System.Collections.Generic;
    using LMS.LeadEntity.Interface;

    public class CampaignManagerDecorator: IDecorator
    {
        private static ILoggerClient _loggerClient;
        private static string solutionContext = "CampaignManagerDecorator";
        public CampaignManagerDecorator(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        public void DecorateLead(ILeadEntity leadEntity, List<IResult> campaignManagerResultList)
        {
            if (leadEntity == null) throw new ArgumentNullException(nameof(leadEntity));
            if (campaignManagerResultList?.Any() == true)
            {
                string processContext = "DecorateLead";

                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Decorating the Lead", ProcessContext = processContext, SolutionContext = solutionContext });

                // Create LeadCollector Results
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
