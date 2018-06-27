namespace LMS.CampaignManager.Interface
{
    public interface ICampaignManagerSubscriber
    {
        void ReceiveLead(string message);
    }
}
