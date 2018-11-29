using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;
using Compare.Services.LMS.Modules.LoggerClient.Interface;
using Newtonsoft.Json;

namespace LMS.LeadDispatcher.Implementation.Persistor
{
    public class LeadDispatcherPersistor : IPersistor
    {
        private readonly ILoggerClient _loggerClient;
        private const string SolutionContext = "LeadDispatcherPersistor";

        public LeadDispatcherPersistor(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        /// <summary>
        /// Persist Lead into DB TBD.
        /// [ApprovedLeadID]
        // TODO
        //,[ProcessedLead_Id] ???????? - Insert into LeadEntity??? / Lookup? Done in Persistor of CM
        //,[LeadEntityGUID]
        //,[CustomerActivityGUID]
        //,[CustomerSessionGUID]
        //,[Brand_ID]
        // TODO
        //,[Insurer_ID] ???????
        //,[Site_ID]
        //,[Product_ID]
        //,[Campaign_ID]
        //,[CampaignManager_ID]
        //,[LeadCreationTime]
        //,[CreationDate]
        /// </summary>
        /// <param name="leadEntity"></param>
        public void PersistLead(ILeadEntity leadEntity)
        {

            // TODO - Implementation & NULL CHECKING!!!! - Check for NULL for each of the collections & values that need to be persisted
            var processContext = "PersistLead..................... TBD";
            var leadEntityGuid = leadEntity.Context.SingleOrDefault(item => item.Id == ContextKeys.LeadEntityGuidKey)?.Value;
            var activityGuid = leadEntity.Context.SingleOrDefault(item => item.Id == ContextKeys.ActivityGuidKey)?.Value;
            var sessionGuid = leadEntity.Context.SingleOrDefault(item => item.Id == ContextKeys.SessionGuidKey)?.Value;
            var brandId = leadEntity.Activity.SingleOrDefault(a => a.Id == ActivityKeys.BrandIdKey)?.Value;
            var siteId = leadEntity.Context.SingleOrDefault(item => item.Id == ContextKeys.SiteIDKey)?.Value;
            var productId = leadEntity.Context.SingleOrDefault(item => item.Id == ContextKeys.QuotedProductKey)?.Value;
            // TODO - the leads wont always have these ResultCollections - CHECK!!!!!!!!
            var cmId = leadEntity.ResultCollection.CampaignManagerCollection
                .SingleOrDefault(item => item.Id == ResultKeys.CampaignManagerKeys.CampaignManagerIdKey)?.Value;
            var cId = leadEntity.ResultCollection.PreferredCampaignCollection
                .SingleOrDefault(item => item.Id == ResultKeys.CampaignKeys.CampaignIdKey)?.Value;
            // TODO Check existence of collection
            var leadCreationTime = leadEntity.ResultCollection.LeadCollectorCollection
                .SingleOrDefault(item => item.Id == ResultKeys.DiagnosticKeys.TimeStampStartKey)?.Value;


            var tmpStrToDb =
                $"ApprovedLeadTable:\nLeadEntityGuid:{leadEntityGuid}\nActivitityGuid:{activityGuid}\n";
            tmpStrToDb += $"SessionGuid:{sessionGuid}\nBrandId:{brandId}\nSiteId:{siteId}\nProductId:{productId}\n";
            tmpStrToDb += $"CampaignManagerId:{cmId}\nCampaign:{cId}\nLeadCreation:{leadCreationTime}\n";

            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = JsonConvert.SerializeObject(leadEntity, Newtonsoft.Json.Formatting.Indented), ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = tmpStrToDb, ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = JsonConvert.SerializeObject(leadEntity, Newtonsoft.Json.Formatting.Indented), ProcessContext = processContext, SolutionContext = SolutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

        }
    }
}
