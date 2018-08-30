using System.Collections.Generic;
using System.Linq;

namespace LMS.ControlTester
{
    using LMS.Modules.LeadEntity.Components;
    using LMS.Modules.LeadEntity.Interface;
    using LMS.Modules.LeadEntity.Interface.Constants;
    using System;
    using LMS.DataProvider;
    using LMS.Validator.Interface;
    using LMS.Validator.Implementation;

    public class Program
    {
        public static void Main(string[] args)
        {
            DefaultLeadEntity _testLeadEntity;
            const string priorBi = "50/100";
            const string priorInsurance = "true";
            const int vehicleCount = 2;
            const string quotedBi = "100/300";
            int[] displayedBrands = new int[] {22, 58, 181, 218};
            const string phoneNumber = "888-556-5456";
            const int pni_Age = 28;
            _testLeadEntity = new DefaultLeadEntity()
            {
                Context = new IContext[]
                {
                    new DefaultContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new DefaultContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new DefaultContext(ContextKeys.SessionGuidKey, Guid.NewGuid().ToString()),
                    new DefaultContext(ContextKeys.QuotedProductKey, "101"),
                    new DefaultContext(ContextKeys.AdditionalProductKey, "")
                },

                Properties = new IProperty[]
                {
                    new DefaultProperty(PropertyKeys.PriorBIKey, priorBi),
                    new DefaultProperty(PropertyKeys.PriorInsuranceKey, priorInsurance.ToString()),
                    new DefaultProperty(PropertyKeys.VehicleCountKey, vehicleCount.ToString()),
                    new DefaultProperty(PropertyKeys.QuotedBIKey, quotedBi),
                    new DefaultProperty(PropertyKeys.DisplayedBrandsKey, displayedBrands.ToString()),
                    new DefaultProperty(PropertyKeys.PhoneNumber, phoneNumber.ToString()),
                    new DefaultProperty(PropertyKeys.PNI_Age, pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new DefaultSegment(SegementKeys.HighPOPKey),
                    new DefaultSegment(SegementKeys.HomeownerKey),
                },

            };

            //var validatorDataProvider = new ValidatorDataProvider();
            //var validatorFactory = new ValidatorFactory(validatorDataProvider);
            //var validators = validatorFactory.BuildLeadCollectorValidators();
            //bool allValid = true;
            //// Process all validators before returning.
            //foreach (var validator in validators)
            //{
            //    var valid = validator.ValidLead(_testLeadEntity);
            //    if (!valid)
            //    {
            //        allValid = false;
            //    }
            //}
            //if (!allValid)
            //{
            
            //   Console.WriteLine("Not All Valid");
            //}
            //Console.ReadKey();


        }

        //public static List<string> GetLeadValidatorClassNameList()
        //{

        //    var classNameList = new List<String>();

        //    classNameList.Add("ValidateActivityGuid");
        //    classNameList.Add("ValidateIdentityGuid");

        //    return classNameList;
        //    using (var context = new ValidatorContext())
        //    {

        //        foreach (var leadCollectorValidator in context.LeadCollectorValidators)
        //        {
        //            // Select the validator
        //            var validator =
        //                context.Validators.FirstOrDefault(v => v.ValidatorId == leadCollectorValidator.ValidatorId);
        //            // var className = context.Validators.Where(v => v.ValidatorId == leadCollectorValidator.ValidatorId).Select(v => v.ClassName).ToString();
        //            //var className = context.Validators.Where(v => v.ValidatorId == leadCollectorValidator.ValidatorId).Select(v => new { ClassName = v.ClassName}).ToString();

        //            if (validator != null)
        //            {
        //                //                        classNameList.Add(validator.ClassName);
        //                classNameList.Add(validator.ClassName);

        //            }

        //        }

        //        return classNameList;

        //    }
        //}
    }
}
