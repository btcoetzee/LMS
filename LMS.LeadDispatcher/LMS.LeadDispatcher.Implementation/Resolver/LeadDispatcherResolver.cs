using System;
using System.Collections.Generic;
using System.Text;
using Compare.Services.LMS.Common.Common.Interfaces;
using Compare.Services.LMS.Controls.Resolver.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace LMS.LeadDispatcher.Implementation.Resolver
{
    /// <summary>
    /// LeadDispatcherResolver class.  This class contains the function ValidLead that
    /// will confirm that the lead has all the initial pre-requisites to process through the
    /// the LeadDispatcher.
    /// </summary>
    public class LeadDispatcherResolver : IResolver
    {
        readonly ILoggerClient _loggerClient;
        private static string solutionContext = "LeadDispatcherResolver";
        private readonly IList<IResolver> _leadDispatcherResolvers;

        /// <summary>
        /// Constructor for the LeadDispatcherLeadDispatcherResolver. 
        /// This class functions as the wrapper for all resolvers defined. 
        /// </summary>
        /// <param name="leadDispatchResolvers"></param>
        /// <param name="loggerClient"></param>
        public LeadDispatcherResolver(IList<IResolver> leadDispatchResolvers, ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
            _leadDispatcherResolvers = leadDispatchResolvers;
        }
        /// <summary>
        /// Loop through the Resolvers and check if the lead is resolved as expected.  This function
        /// loops through all the resolvers. It returns as soon as a resolver is not satisfied.
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ResolveLead(ILeadEntity leadEntity)
        {
            string processContext = "ResolveLead";
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "Resolving the Lead", ProcessContext = processContext, SolutionContext = solutionContext });
            try
            {
                // Resolve the lead using the collection of resolvers
                foreach (var resolver in _leadDispatcherResolvers)
                {
                    var resolved = resolver.ResolveLead(leadEntity);
                    if (!resolved)
                    {
                        _loggerClient.Log(new DefaultLoggerClientErrorObject
                        {
                            OperationContext = $"Lead Dispatcher Resolver failed. {leadEntity.ErrorList}",
                            ProcessContext = processContext,
                            SolutionContext = solutionContext,
                            EventType = LoggerClientEventType.LoggerClientEventTypes.Information
                        });
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject
                {
                    OperationContext = "Exception in LeadDispatcher Resolver.",
                    ProcessContext = processContext,
                    SolutionContext = solutionContext,
                    Exception = ex,
                    ErrorContext = ex.Message,
                    EventType = LoggerClientEventType.LoggerClientEventTypes.Error
                });
                return false;
            }

            return true;
        }
    }
}
