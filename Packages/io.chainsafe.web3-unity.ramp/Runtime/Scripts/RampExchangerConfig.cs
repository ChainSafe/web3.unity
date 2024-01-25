using System;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public class RampExchangerConfig : IRampExchangerConfig
    {
        public string HostApiKey { get; set; }
        public string HostLogoUrl { get; set; }
        public string HostAppName { get; set; }
        public string Url { get; set; }
        public string WebhookStatusUrl { get; set; }
        public string OfframpWebHookV3Url { get; set; }
    }
}