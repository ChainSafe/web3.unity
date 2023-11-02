using ChainSafe.Gaming.Evm.Signers;

namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public static class RampExchangerFactory
    {
        public static IRampExchanger CreateRampExchanger(IRampExchangerConfig config, ISigner signer)
        {
            #if UNITY_IOS
            return new RampExchangeriOS(); // todo provide with config and signer
            #elif UNITY_WEBGL
            return new RampExchangerWebGL(config, signer);
            #endif
            return null;
        }
    }
}