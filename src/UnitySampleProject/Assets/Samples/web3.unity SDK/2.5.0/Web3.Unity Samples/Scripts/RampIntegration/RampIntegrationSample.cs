using ChainSafe.GamingSdk.RampIntegration;
using ChainSafe.GamingSdk.ScriptableObjects;
using UnityEngine;

public class RampIntegrationSample : MonoBehaviour
{
    [SerializeField] private RampData rampData;
    private RampChainsafeIntegrationBase _integration;

    private void Awake()
    {
        Debug.LogError("AWAKE");
#if UNITY_IOS
        _integration = new RampChainsafeIntegrationiOS(rampData);
#endif
    }

    public void ButtonClicked()
    {
        _integration.OpenRamp();
    }
}