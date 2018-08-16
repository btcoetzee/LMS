using LMS.LeadEntity.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LeadEntity.Components
{
    public struct DefaultResultCollection : IResultCollection
    {
        public DefaultResultCollection(IResult[] leadCollectorCollection, IResult[] campaignManagerCollection,
            IResult[][] campaignCollection, IResult[] preferredCampaignCollection)
        {
            LeadCollectorCollection = leadCollectorCollection;
            CampaignManagerCollection = campaignManagerCollection;
            CampaignCollection = campaignCollection;
            PreferredCampaignCollection = preferredCampaignCollection;

        }
        public IResult[] LeadCollectorCollection { get; set; }
        public IResult[] CampaignManagerCollection { get; set; }
        public IResult[][] CampaignCollection { get; set; }
        public IResult[] PreferredCampaignCollection { get; set; }
    }
}
