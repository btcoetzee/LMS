using LMS.LeadEntity.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LeadEntity.Components
{
    public struct DefaultResultCollection : IResultCollection
    {
        public IResult[] LeadCollectorCollection { get; set; }
        public IResult[] CampaignManagerCollection { get; set; }
        public IResult[][] CampaignCollection { get; set; }
        public IResult[] PreferredCampaignCollection { get; set; }
    }
}
