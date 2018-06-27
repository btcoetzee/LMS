namespace Campaign.Subscriber
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Campaign.Interface;
    public class CampaignSubscriber : ICampaignSubscriber
    {
        private readonly ICampaign _campaign;

        public CampaignSubscriber(ICampaign campaign)
        {
            _campaign = campaign;
        }

        /// <summary>
        /// Receive the lead from the Channel Subscibed to and let the Campaign process it.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string ReceiveLead(string message)
        {
            var result = _campaign.ProcessLead(message);

            return result;
  
        }
    }
}
