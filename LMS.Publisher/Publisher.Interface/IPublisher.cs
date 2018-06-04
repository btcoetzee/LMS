namespace Publisher.Interface
{
    using LeadEntity.Interface;

    public interface IPublisher
    {
        // Publish the lead for further processing
        void PublishLead(ILeadEntity lead);

    }
}
