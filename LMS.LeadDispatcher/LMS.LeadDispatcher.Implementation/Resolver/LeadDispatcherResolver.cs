using System;
using System.Collections.Generic;
using System.Linq;
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
        private string _resolverClassName;

        /// <summary>
        /// Return the Resovler Name
        /// </summary>
        public string Name
        {
            get => _resolverClassName;
            set => _resolverClassName = value;
        }


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
            _resolverClassName = "LeadDispatcherResolver";
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
            string resolverStr = String.Empty;
            _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = "LeadDispatcherResolving the Lead", ProcessContext = processContext, SolutionContext = solutionContext });
            try
            {
                // Resolve the lead using the collection of resolvers
                foreach (var resolver in _leadDispatcherResolvers)
                {

                    var resolved = resolver.ResolveLead(leadEntity);
                    _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = $"Resolver {resolver.Name} returned {resolved}.", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

                    if (!resolved)
                    {
                        resolverStr += Environment.NewLine + $"Lead Failed for ResolverClassName:{resolver.Name}";
                        resolverStr += Environment.NewLine + String.Join("|", leadEntity.ErrorList.ToArray()).Replace("\n", String.Empty);

                        _loggerClient.Log(new DefaultLoggerClientErrorObject
                        {
                            OperationContext = resolverStr,
                            ProcessContext = processContext,
                            SolutionContext = solutionContext,
                            EventType = LoggerClientEventType.LoggerClientEventTypes.Information,
                        });
                        return false;
                    }
                    else
                    {
                        resolverStr += resolver.Name + ".resolveLead() returned true." + Environment.NewLine;
                        _loggerClient.Log(new DefaultLoggerClientObject { OperationContext = resolverStr, ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventType.LoggerClientEventTypes.Information });

                    }
                }
            }
            catch (Exception ex)
            {
                resolverStr += "Exeception Thrown in Lead Dispatcher Resolver:";
                _loggerClient.Log(new DefaultLoggerClientErrorObject
                {

                    OperationContext = resolverStr,
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
