namespace LMS.CampaignManager.Subscriber.Interface
{
    using LMS.Modules.LeadEntity.Interface;
    using System;
    public interface ICampaignManagerSubscriber
    {
        void SetupAddOnReceiveActionToChannel(Action<ILeadEntity> receiveAction);
    }
}
