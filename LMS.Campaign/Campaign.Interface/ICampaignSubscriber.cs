namespace Campaign.Interface
{
    using System;
    public interface ICampaignSubscriber
    {
        string ReceiveLead(string message);
    }
}
