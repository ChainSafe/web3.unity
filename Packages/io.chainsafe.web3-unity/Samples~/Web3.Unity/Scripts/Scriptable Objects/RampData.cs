using UnityEngine;

namespace ChainSafe.GamingSdk.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Ramp Data", menuName = "Create Ramp Data Container")]
    public class RampData : ScriptableObject
    {
        [field: SerializeField] public string SwapAsset { get; private set; }
        [field: SerializeField] public string OfframpAsset { get; private set; }
        [field: SerializeField] public string SwapAmount { get; private set; }
        [field: SerializeField] public string FiatCurrency { get; private set; }
        [field: SerializeField] public string FiatValue { get; private set; }
        [field: SerializeField] public string UserAddress { get; private set; }
        [field: SerializeField] public string HostLogoUrl { get; private set; }
        [field: SerializeField] public string HostAppName { get; private set; }
        [field: SerializeField] public string UserEmailAddress { get; private set; }
        [field: SerializeField] public string SelectedCountryCode { get; private set; }
        [field: SerializeField] public string DefaultAsset { get; private set; }
        [field: SerializeField] public string Url { get; private set; }
        [field: SerializeField] public string WebhookStatusUrl { get; private set; }
        [field: SerializeField] public string FinalUrl { get; private set; }
        [field: SerializeField] public string ContainerNode { get; private set; }
        [field: SerializeField] public string HostApiKey { get; private set; }
        [field: SerializeField] public string OfframpWebHookV3Url { get; private set; }
        [field: SerializeField] public bool UseSendCryptoCallbackVersion { get; private set; }

    }


}