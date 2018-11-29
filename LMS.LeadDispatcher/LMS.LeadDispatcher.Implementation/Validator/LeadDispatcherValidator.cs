using System;
using System.Collections.Generic;
using System.Linq;
using Compare.Services.LMS.Controls.Validator.Interface;
using Compare.Services.LMS.Modules.LeadEntity.Interface;
using Compare.Services.LMS.Modules.LoggerClient.Interface;

namespace LMS.LeadDispatcher.Implementation.Validator
{
    /// <summary>
    /// LeadDispatcherValidator class.  This class contains the function ValidLead that
    /// will confirm that the lead has all the initial pre-requisites to process through the
    /// the LeadDispatcher.
    /// </summary>
    public class LeadDispatcherValidator : IValidator
    {
        private string _validatorClassName;
        readonly ILoggerClient _loggerClient;
        private static string solutionContext = "LeadDispatcherValidator";
        private readonly IList<IValidator> _leadDispatcherValidators;

        /// <summary>
        /// Return the Resovler Name
        /// </summary>
        public string Name
        {
            get => _validatorClassName;
            set => _validatorClassName = value;
        }
        /// <summary>
        /// Constructor for the LeadDispatcherValidator.  
        /// </summary>
        /// <param name="leadDispatcherValidators"></param>
        /// <param name="loggerClient"></param>
        public LeadDispatcherValidator(IList<IValidator> leadDispatcherValidators, ILoggerClient loggerClient)
        {
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
            _leadDispatcherValidators = leadDispatcherValidators;
            _validatorClassName = "LeadDispatcherValidator";
        }

        /// <summary>
        /// Loop through the Validators and check if the lead is valid.  This function
        /// loops through all the validators, even if it finds one that is not valid and
        /// collects all the reasons why a lead does not qualify for being processed by the campaign manager.
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {
            string processContext = "ValidLead";
            bool allValid = true;
            string validationStr = String.Empty;
            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Validating the Lead",
                ProcessContext = processContext,
                SolutionContext = solutionContext
            });
            try
            {
                // Validate the lead using the collection of validators
                // Process all validators before returning.

                foreach (var validator in _leadDispatcherValidators)
                {
                    var valid = validator.ValidLead(leadEntity);
                    if (!valid)
                    {
                        validationStr += Environment.NewLine + $"Lead Validation Failed in ValidatorClassName:{validator.Name}";
                        allValid = false;
                    }
                }

                if (!allValid)
                {
                    validationStr += Environment.NewLine + String.Join("|", leadEntity.ErrorList.ToArray()).Replace("\n", String.Empty);
                    _loggerClient.Log(new DefaultLoggerClientErrorObject
                    {
                        OperationContext = validationStr,
                        ProcessContext = processContext,
                        SolutionContext = solutionContext,
                        EventType = LoggerClientEventType.LoggerClientEventTypes.Information
                    });
                    return false;
                }
            }
            catch (Exception ex)
            {
                validationStr += "Exception in Lead Dispatcher Validator:";
                _loggerClient.Log(new DefaultLoggerClientErrorObject
                {
                    OperationContext = validationStr,
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


