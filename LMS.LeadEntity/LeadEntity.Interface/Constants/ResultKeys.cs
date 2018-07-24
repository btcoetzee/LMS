using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.LeadEntity.Interface.Constants
{
    public static partial class ResultKeys
    {

        // Keys for Collector, CampaignManager and Campaigns

        // Enums for Status Keys etc.

        public static class DiagnosticKeys
        {
            /// <summary>
            /// Time Stamp when Processing Started
            /// </summary>
            public const string TimeStampStartKey = "TimeStampStart";

            /// <summary>
            /// Time Stamp when  Processing Ended
            /// </summary>
            public const string TimeStampEndKey = "TimeStampEnd";
        }

        /// <summary>
        /// Validator Status - ResultKeysStatusEnum
        /// </summary>
        /// <returns></returns>
        public const string ValidatorStatusKey = "ValidatorStatus";

        /// <summary>
        /// Decorator Status - ResultKeysStatusEnum
        /// </summary>
        /// <returns></returns>
        public const string DecoratorStatusKey = "DecoratorStatus";

        ///<summary>
        /// Resolver Status - ResultKeysStatusEnum
        /// </summary>
        /// <returns></returns>
        public const string ResolverStatusKey = "ResolverStatus";

        /// <summary>
        /// Number of Leads that reached Resolver
        /// </summary>
        /// <returns></returns>
        public const string ResolverResultCountKey = "ResolverResultCount";

 
    }
}