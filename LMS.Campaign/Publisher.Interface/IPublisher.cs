namespace Publisher.Interface
{
    using System.IO;

    public interface IPublisher
    {
        // Publish the lead for further processing
        void PublishLead(Stream lead);

    }
}
