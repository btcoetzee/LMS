namespace LMS.LeadEntity.Interface.Constants
{
    public static partial class ResultKeys
    {
        public static partial class CampaignKeys
        {
            /// <summary>
            /// Name of the Campaign
            /// </summary>
            public const string CampaignNameKey = "CampaignName";

            /// <summary>
            /// Name of the Campaign
            /// </summary>
            public const string CampaignPriorityKey = "CampaignPriority";

            /// <summary>
            /// Name of the Campaign
            /// </summary>
            public const string CampaignMessageHandlerKey = "CampaignMessageHandler";

            /// <summary>
            /// Indicates that the lead was processed through the Campaign Fitlers 
            /// </summary>
            /// <returns></returns>
            public const string FilterStatusKey = "FilterStatus";

            /// <summary>
            /// Indicates that the lead was processed through the Campaign Rules 
            /// </summary>
            /// <returns></returns>
            public const string RuleStatusKey = "RuleStatus";

            /// <summary>
            /// Indicates that the lead was processed through the Campaing and survived all rules/filters and
            /// should continue to the CampaignManagerResolver 
            /// </summary>
            /// <returns></returns>
            public const string LeadSuccessStatusKey = "LeadSuccessStatus";


        }
    }
}