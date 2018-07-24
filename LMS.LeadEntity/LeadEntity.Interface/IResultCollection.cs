namespace LMS.LeadEntity.Interface
{
    using System;

    public interface IResultCollection
    {
        // Results from Lead Collector
        IResult[] LeadCollectorCollection { get; set; }

        // Results posted from Campaign Manager
        IResult[] CampaignManagerCollection { get; set; }

        // Results posted from different Campaigns subsribed within CampaignManager
        IResult[][] CampaignCollection { get; set; }

        // Results posted from Campaign Manager Resover
        IResult[] CampaignManagerResolverCollection { get; set; }

        // Preferred Campaign - This is the Campaign Results with the highest priority
        IResult[] PreferredCampaignCollection { get; set; }
    }
}
