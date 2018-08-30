namespace LMS.LeadValidator.Implementation
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using LMS.LoggerClient.Interface;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.ValidatorFactory.Interface;
    using LMS.Validator.Interface;

    public class LeadValidator : IValidator
    {

        public DefaultLoggerClientObject DefaultLoggerClientObject;
        readonly ILoggerClient _loggerClient;
        private readonly IValidatorFactory _validatorFactory;
        private static string solutionContext = "LeadValidator";
        private readonly List<IValidator> _leadCollectorValidators;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validatorFactory"></param>
        /// <param name="loggerClient"></param>
        public LeadValidator(IValidatorFactory validatorFactory, ILoggerClient loggerClient)
        {          
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
            _validatorFactory = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
            _leadCollectorValidators = _validatorFactory.BuildLeadCollectorValidators();

        }

        /// <summary>
        /// Validate the lead using the Collection of Validators
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool ValidLead(ILeadEntity leadEntity)
        {          
            string processContext = "ValidLead";
            bool allValid = true;
            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Validating the Lead",ProcessContext = processContext,SolutionContext = solutionContext});
            try
            {
                // Validate the lead using the collection of validators
                // Process all validators before returning.
                foreach (var validator in _leadCollectorValidators)
                {
                    var valid = validator.ValidLead(leadEntity);
                    if (!valid)
                    {
                        allValid = false;
                    }
                }
                if (!allValid)
                {
                    _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = "Validation failed", ProcessContext = processContext, SolutionContext = solutionContext, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Error });
                    return false;
                }
            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject
                {
                    OperationContext = "Exception in Validation of Lead",ProcessContext = processContext,SolutionContext = solutionContext,Exception = ex,ErrorContext = ex.Message, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Error
                });
                return false;
            }

            return true;

        }
    }
}
