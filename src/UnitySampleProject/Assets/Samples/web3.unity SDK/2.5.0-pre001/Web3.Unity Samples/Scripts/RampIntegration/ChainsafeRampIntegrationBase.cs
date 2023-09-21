using ChainSafe.GamingSdk.ScriptableObjects;

namespace ChainSafe.GamingSdk.RampIntegration
{
    public abstract class ChainsafeRampIntegrationBase
    {
        protected readonly RampData _rampData;

        protected ChainsafeRampIntegrationBase(RampData rampData)
        {
            _rampData = rampData;
        }
        
        public abstract void OpenRamp();
    }
}