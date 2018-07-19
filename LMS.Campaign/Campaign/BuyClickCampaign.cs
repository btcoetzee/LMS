namespace LMS.Campaign.Implementation.BuyClick
{
    using LMS.Campaign.Implementation.BuyClick.Validator;
    using LMS.Campaign.Implementation.BuyClickCampaign.Decorator;
    using LMS.Campaign.Interface;
    using LMS.Decorator.Interface;
    using LMS.LeadEntity.Interface;
    using LMS.LoggerClient.Interface;
    using LMS.Validator.Interface;
    using System;

    public class BuyClickCampaign : ICampaign
    {

        private readonly IValidator _buyClickValidator;
        private readonly IDecorator _buyClickDecorator;
        private readonly ILoggerClient _loggerClient;
        private static ICampaign _notificationChannelPublisher;
        private static string solutionContext = "BuyClickCampaign";
        const string ThisCampaignName = "BuyClick Campaign";
        public BuyClickCampaign(IValidator buyClickValidator, IDecorator buyClickDecorator, ILoggerClient loggerClient)
        {
            _buyClickValidator = buyClickValidator ?? throw new ArgumentNullException(nameof(buyClickValidator));
            _buyClickDecorator = buyClickDecorator ?? throw new ArgumentNullException(nameof(buyClickDecorator));
            _loggerClient = loggerClient ?? throw new ArgumentNullException(nameof(loggerClient));
        }

        
        public string CampaignName
        {
            get { return ThisCampaignName; }

        }

        public ILeadEntity ProcessLead(ILeadEntity leadEntity)
        {
            string processContext = "ProcessLead";

            _loggerClient.Log(new DefaultLoggerClientObject
            {
                OperationContext = "Processing the Lead",
                ProcessContext = processContext,
                SolutionContext = solutionContext
            });

            if (_buyClickValidator.ValidLead(leadEntity).Equals(true))
            {
                _loggerClient.Log(new DefaultLoggerClientObject
                {
                    OperationContext = "Validating the Buy Click",
                    ProcessContext = processContext,
                    SolutionContext = solutionContext
                });

                _buyClickDecorator.DecorateLead(leadEntity);

                _loggerClient.Log(new DefaultLoggerClientObject
                {
                    OperationContext = "Decorating the Buy Click",
                    ProcessContext = processContext,
                    SolutionContext = solutionContext
                });

            }

            return leadEntity;
        }
    }
}
