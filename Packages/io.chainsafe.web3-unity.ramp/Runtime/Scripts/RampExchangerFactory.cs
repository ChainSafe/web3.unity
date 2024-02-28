using ChainSafe.Gaming.Evm.Signers;
using ChainSafe.Gaming.Web3;
using UnityEngine;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public static class RampExchangerFactory
    {
        public static IRampExchanger CreateRampExchanger(IRampExchangerConfig config, ISigner signer)
        {
#if UNITY_IOS
            return new RampExchangeriOS(config, signer);
#elif UNITY_WEBGL && !UNITY_EDITOR
            return new RampExchangerWebGL(config, signer);
#endif
            throw new Web3Exception(
                $"No {nameof(IRampExchanger)} implementation found for " +
                $"{Application.platform} (editor:{Application.isEditor})."
                );
        }
    }
}