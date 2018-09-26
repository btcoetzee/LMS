using System;
using System.Collections.Generic;
using System.Linq;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Modules.LeadEntity.Components;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace Compare.Services.LMS.CampaignManager.Implementation.Decorator
{
    public class CampaignManagerDecorator: IDecorator
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

                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Decorating the Lead", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

                // The Results Collection was not created yet - shouldn't happen
                if (leadEntity.ResultCollection == null)
                    leadEntity.ResultCollection = new DefaultResultCollection();

                // Add the decorator result time & status
                campaignManagerResultList.Add(new DefaultResult(ResultKeys.DiagnosticKeys.TimeStampEndKey, DateTime.Now));
                campaignManagerResultList.Add(new DefaultResult(ResultKeys.DecoratorStatusKey, ResultKeys.ResultKeysStatusEnum.Processed.ToString()));

                // If there has been previous CampaignManager results - add them to the list and then assign as array
                if (leadEntity.ResultCollection.CampaignManagerCollection != null)
                    campaignManagerResultList.AddRange(leadEntity.ResultCollection.CampaignManagerCollection.ToList());

                // Assign all Campaign Manager Results that were Recorded
                leadEntity.ResultCollection.CampaignManagerCollection = campaignManagerResultList.ToArray();

            }

         
        }
        
    }
}
