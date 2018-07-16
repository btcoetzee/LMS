namespace LMS.CampaignManager.Subscriber.Interface
{
    public interface ICampaignManagerSubscriber
    {
        void ReceiveLead(string message);
    }
}
