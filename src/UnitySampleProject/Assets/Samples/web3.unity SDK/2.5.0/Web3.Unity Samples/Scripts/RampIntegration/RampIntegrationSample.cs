using ChainSafe.GamingSdk.RampIntegration;
using ChainSafe.GamingSdk.ScriptableObjects;
using UnityEngine;

public class RampIntegrationSample : MonoBehaviour
{
    [SerializeField] private RampData rampData;
    private ChainsafeRampIntegrationBase rampIntegration;

    private void Awake()
    {
#if UNITY_IOS && !UNITY_EDITOR
        rampIntegration = new ChainsafeRampIntegrationiOS(rampData);
#endif
    }

    public void ButtonClicked()
    {
        rampIntegration.OpenRamp();
    }
}