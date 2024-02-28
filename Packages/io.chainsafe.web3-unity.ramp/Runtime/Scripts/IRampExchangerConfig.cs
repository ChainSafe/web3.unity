namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public interface IRampExchangerConfig
    {
        /// <summary>
        /// A required string parameter that allows our system to properly recognize and count purchases made through
        /// your API integration.
        /// </summary>
        string HostApiKey { get; }

        /// <summary>
        /// A required string parameter that allows you to brand your Ramp integration with your app's logo.
        /// </summary>
        string HostLogoUrl { get; }

        /// <summary>
        /// A required string parameter that allows you to brand your Ramp integration with your app's name.
        /// </summary>
        string HostAppName { get; }

        /// <summary>
        /// An optional string parameter that allows you to use a non-production version of our widget.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// An optional string parameter that allows you to subscribe to purchase events via webhooks.
        /// </summary>
        string WebhookStatusUrl { get; }

        /// <summary>
        /// An optional string parameter that allows you to subscribe to sale events via webhooks.
        /// </summary>
        string OfframpWebHookV3Url { get; }
    }
}