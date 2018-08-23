namespace LMS.LeadValidator.Implementation
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using LMS.LoggerClient.Interface;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.ValidatorCollection.Interface;

    public class LeadValidator : IValidatorCollection
    {

        public DefaultLoggerClientObject DefaultLoggerClientObject;
        readonly ILoggerClient _loggerClient;
        private readonly IValidatorCollectionHandler _validatorCollectionHandler;
        private static string solutionContext = "LeadValidator";
        private string _errorMessage;
        private List<string> _validatorClassNameList;


        public LeadValidator(/*IValidatorCollectionHandler validatorCollectionHandler, */ILoggerClient loggerClient)
        {          
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
            //_validatorCollectionHandler = validationCollectionHandler;
            _errorMessage = String.Empty;
     
            _validatorClassNameList = ValidatorCollectionClassNameList();

        }

        public string ErrorMessage => _errorMessage;


        public bool ValidLead(ILeadEntity leadEntity)
        {          
            string processContext = "ValidLead";
            _loggerClient.Log(new DefaultLoggerClientObject{OperationContext = "Validating the Lead",ProcessContext = processContext,SolutionContext = solutionContext});
            try
            {
                var valid = _validatorCollectionHandler.Execute(leadEntity, _validatorClassNameList);
                if (!valid)
                {
                    _errorMessage = _validatorCollectionHandler.ErrorMessage();
                    _loggerClient.Log(new DefaultLoggerClientErrorObject { OperationContext = "Validation failed", ProcessContext = processContext, SolutionContext = solutionContext, ErrorContext = _errorMessage, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Error });
                    return false;
                }
            }
            catch (Exception ex)
            {
                _loggerClient.Log(new DefaultLoggerClientErrorObject
                {
                    OperationContext = "Exception in Validation of Lead",ProcessContext = processContext,SolutionContext = solutionContext,Exception = ex,ErrorContext = ex.Message, EventType = LoggerClientEventTypeControl.Interface.Constants.LoggerClientEventType.LoggerClientEventTypes.Information
                });
                return false;
            }

            return true;

        }

        /// <summary>
        /// Provide the ClassNames for the LeadCollector
        /// </summary>
        /// <returns></returns>
        public List<string> ValidatorCollectionClassNameList()
        {

            var classNameList = new List<String>();

            // Create the className List
            using (var context = _validatorCollectionHandler.GetValidatorContext())
            {

                foreach (var leadCollectorValidator in context.LeadCollectorValidators)
                {
                    // Select the validator
                    var validator = context.Validators.FirstOrDefault(v => v.ValidatorId == leadCollectorValidator.ValidatorId);
                    // var className = context.Validators.Where(v => v.ValidatorId == leadCollectorValidator.ValidatorId).Select(v => v.ClassName).ToString();
                    //var className = context.Validators.Where(v => v.ValidatorId == leadCollectorValidator.ValidatorId).Select(v => new { ClassName = v.ClassName}).ToString();

                    if (validator != null)
                    {
                        //                        classNameList.Add(validator.ClassName);
                        classNameList.Add(validator.ClassName);

                    }

                }
                return classNameList;

            }

        }

    }
}
