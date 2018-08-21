using LMS.DataProvider;

namespace LMS.ControlTester
{
    using LMS.LeadEntity.Components;
    using LMS.LeadEntity.Interface;
    using LMS.LeadEntity.Interface.Constants;
    using System;
    public class Program
    {
        public static void Main(string[] args)
        {
            DefaultLeadEntity _testLeadEntity;
            const string priorBi = "50/100";
            const string priorInsurance = "true";
            const int vehicleCount = 2;
            const string quotedBi = "100/300";
            int[] displayedBrands = new int[] { 22, 58, 181, 218 };
            const string phoneNumber = "888-556-5456";
            const int pni_Age = 28;
            _testLeadEntity = new DefaultLeadEntity()
            {
                Context = new IContext[]
                {
                    new DefaultContext(ContextKeys.ActivityGuidKey, Guid.NewGuid().ToString()),
                    new DefaultContext(ContextKeys.IdentityGuidKey, Guid.NewGuid().ToString()),
                    new DefaultContext(ContextKeys.SessionGuidKey,Guid.NewGuid().ToString()),
                    new DefaultContext(ContextKeys.QuotedProductKey,"101"),
                    new DefaultContext(ContextKeys.AdditionalProductKey,"")
                },

                Properties = new IProperty[]
                {
                    new DefaultProperty(PropertyKeys.PriorBIKey,priorBi),
                    new DefaultProperty(PropertyKeys.PriorInsuranceKey,priorInsurance.ToString()),
                    new DefaultProperty(PropertyKeys.VehicleCountKey,vehicleCount.ToString()),
                    new DefaultProperty(PropertyKeys.QuotedBIKey,quotedBi),
                    new DefaultProperty(PropertyKeys.DisplayedBrandsKey,displayedBrands.ToString()),
                    new DefaultProperty(PropertyKeys.PhoneNumber,phoneNumber.ToString()),
                    new DefaultProperty(PropertyKeys.PNI_Age,pni_Age.ToString())
                },

                Segments = new ISegment[]
                {
                    new DefaultSegment(SegementKeys.HighPOPKey),
                    new DefaultSegment(SegementKeys.HomeownerKey),
                },

            };

            var validatorHandler = new ValidatorHandler();
            validatorHandler.Execute(_testLeadEntity);

            Console.ReadKey();


        }
    }
}
