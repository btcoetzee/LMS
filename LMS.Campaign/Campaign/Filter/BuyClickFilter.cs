using System;
using System.Linq;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface.Constants;

using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace Compare.Services.LMS.Campaign.BuyClick.Filter
{
    public class BuyClickFilter : IController
    {
        readonly ILoggerClient _loggerClient;

        private static string solutionContext = "BuyClickFilter";

        // Mock an array of ActivityGuids that could have come from the database - for these activity guids, emails have been sent out in 
        // the last 3 days - If the incomiing leadEntity has a Guid in this array the Filter should return false;
        readonly Guid[] _mockedActivityGuidArray = {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new Guid("BF526BAF-F860-4530-BAA5-A205E285881A"), Guid.NewGuid() };
        
        public BuyClickFilter(ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));

        }
        public bool ConstraintMet(ILeadEntity leadEntity)
        {
            string processContext = "ClearedFilter";

            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Filter the Lead", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information});
            var errorStr = string.Empty;
            try
            {
                //retrieve activityGuid from leadEntity
                var activityGuidValue = leadEntity.Context.SingleOrDefault(item => item.Id == ContextKeys.ActivityGuidKey)?.Value;
                if (activityGuidValue == null)
                {
                    errorStr += "No activityGuid Found In Context of LeadEntityObject\n";
                }
                else
                {
                    Guid.TryParse(activityGuidValue.ToString(), out Guid activityGuid);
                    //Check to see if leadEntity activity Guid exists in array
                    if (_mockedActivityGuidArray.Count(ag => ag == activityGuid) != 0)
                    {
                        errorStr += $"Activity Guid Found in Duplicate Check Filter. Guid: {activityGuid}\n";
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = ex.Message, ProcessContext = processContext, SolutionContext = solutionContext, Exception = ex, ErrorContext = ex.Message, EventType = LoggerClientEventType.LoggerClientEventTypes.Error });
                return false;
            }

            if (errorStr != String.Empty)
            {
                _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = errorStr, ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                return false;
            }
            return true;
        }

      
    }
}
