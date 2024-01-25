using UnityEngine;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    // todo use constant instead of "ChainSafe/"
    [CreateAssetMenu(menuName = "ChainSafe/Ramp Exchanger Config", fileName = "RampExchangerConfig", order = 0)]
    public class RampExchangerConfigSO : ScriptableObject, IRampExchangerConfig
    {
        [field: SerializeField] public string HostApiKey { get; private set; }
        [field: SerializeField] public string HostLogoUrl { get; private set; }
        [field: SerializeField] public string HostAppName { get; private set; }
        [field: SerializeField] public string Url { get; private set; }
        [field: SerializeField] public string WebhookStatusUrl { get; private set; }
        [field: SerializeField] public string OfframpWebHookV3Url { get; private set; }
    }
}