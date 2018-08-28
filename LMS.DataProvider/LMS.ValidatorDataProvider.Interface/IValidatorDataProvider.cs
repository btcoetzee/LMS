namespace LMS.ValidatorDataProvider.Interface
{
    using LMS.ValidatorDataProvider.Interface.ValidatorEntites;
    using System;
    using System.Collections.Generic;
    public interface IValidatorDataProvider
    {
        List<ValidatorClassAndAssemblyData> LeadCollectorValidatorClassAndAssemblyList();
    }
}
