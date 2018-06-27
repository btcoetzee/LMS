namespace LMS.Publisher.Interface
{
    using LMS.LeadEntity.Interface;

    public interface IPublisher
    {
        // Publish the lead for further processing
        void PublishLead(ILeadEntity lead);

    }
}
