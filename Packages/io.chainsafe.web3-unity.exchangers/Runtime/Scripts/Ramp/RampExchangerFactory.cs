namespace ChainSafe.Gaming.Exchangers
{
    public static class RampExchangerFactory
    {
        public static RampExchanger CreateRampExchanger(RampData rampData)
        {
            #if UNITY_IOS
            return new RampExchangeriOS(rampData);
            #endif
            return null;
        }
    }
}