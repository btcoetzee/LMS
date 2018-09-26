using System;
using System.Collections.Generic;
using System.Linq;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace Compare.Services.LMS.Campaign.Implementation.Controller
{
    /// <summary>
    /// Controller Class that will continue to invoke the campaign controllers until all are
    /// complete and successful.  If any controller constraint is not met, the ConstraintMet function will
    /// return a false.
    /// </summary>
    public class CampaignController : IController
    {
        readonly ILoggerClient _loggerClient;
        private static string solutionContext = "CampaignControl";
        private readonly IList<IController> _campaignControllers;

        /// <summary>
        /// Constructor for CampaignControl
        /// </summary>
        /// <param name="campaignControllers"></param>
        /// <param name="loggerClient"></param>
        public CampaignController(IList<IController> campaignControllers, ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
            _campaignControllers = campaignControllers;

        }
        /// <summary>
        /// ConstraintMet function to loop through all the Filters and Rules in the order as instantiated
        /// in the constructor.  If any rule/filter constraint is not met, the function returns false,
        /// else the function returns true.
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ConstraintMet(ILeadEntity leadEntity)
        {
            string processContext = "ConstraintMet";

            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Check Controller (Filter and Rule) Constraints", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information});
            
            try
            {
                // Check to see if the lead meets all the constraints set up for by campaign by looping through
                // the collection of controllers. 
                // Return as soon as a constraint is not met for one of the rules/filters.
                foreach (var controller in _campaignControllers)
                {
                    if (!controller.ConstraintMet(leadEntity))
                    {
                        var errorStr = String.Join(",", leadEntity.ErrorList.ToArray());
                        _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = $"Controller constraint not met: {errorStr}.", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = ex.Message, ProcessContext = processContext, SolutionContext = solutionContext, Exception = ex, ErrorContext = ex.Message, EventType = LoggerClientEventType.LoggerClientEventTypes.Error });
                return false;
            }

    
            return true;
        }

       
    }
}
