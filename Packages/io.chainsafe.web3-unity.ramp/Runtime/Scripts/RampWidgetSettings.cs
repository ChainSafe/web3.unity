namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public abstract class RampWidgetSettings
    {
        public int SwapAmount { get; set; }
        public string DefaultAsset { get; set; }
        public string FiatCurrency { get; set; }
        public int FiatValue { get; set; }
        public string OverrideUserAddress { get; set; }
        public string OverrideHostLogoUrl { get; set; }
        public string OverrideHostAppName { get; set; }
        public string UserEmailAddress { get; set; }
        public string SelectedCountryCode { get; set; }
        public string OverrideUrl { get; set; }
    }

    public class RampBuyWidgetSettings : RampWidgetSettings
    {
        public string SwapAsset { get; set; }
        public string OverrideWebhookStatusUrl { get; set; }
    }

    public class RampSellWidgetSettings : RampWidgetSettings
    {
        public string OfframpAsset { get; set; }
        public string OverrideOfframpWebHookV3Url { get; set; }
        public bool UseSendCryptoCallback { get; set; }
    }

    public class RampBuyOrSellWidgetSettings : RampWidgetSettings
    {
        public string SwapAsset { get; set; }
        public string OverrideWebhookStatusUrl { get; set; }
        public string OfframpAsset { get; set; }
        public string OverrideOfframpWebHookV3Url { get; set; }
        public bool UseSendCryptoCallback { get; set; }
    }
}