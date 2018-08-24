namespace LMS.ValidatorFactory.Interface
{
    using LMS.Validator.Interface;
    using System;
    using System.Collections.Generic;
    public interface IValidatorFactory
    {

        List<IValidator> BuildLeadCollectorValidators();
        
    }
}
