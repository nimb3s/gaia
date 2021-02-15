using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Gaia.Constants
{
    public static partial class GaiaConstants
    {
        public static class MessageBus
        {
            public static class ExampleEndpoint
            {
                public const string ENDPOINT_NAME = "Nimb3s.Gaia.*.Endpoint";
                /// <summary>
                /// If null, then max throughput is used
                /// </summary>
                public static readonly int? MessageProcessingConcurrency = null;//1;
                /// <summary>
                /// If null, then max throughput is used
                /// </summary>
                public static readonly int? RateLimitInSeconds = null;//30;
            }          
        }
    }
}
