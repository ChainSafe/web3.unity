namespace ChainSafe.GamingSdk.Gelato.Types
{
    public static class GelatoAbi
    {
        public static readonly string UserNonce = "[{\"inputs\": [" +
                                                  "{\"internalType\": \"address\",\"name\": \"account\",\"type\": \"address\"}" +
                                                  "]," +
                                                  "\"name\": \"userNonce\"," +
                                                  "\"outputs\": [" +
                                                  "{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}" +
                                                  "]," +
                                                  "\"stateMutability\": \"view\"," +
                                                  "\"type\": \"function\"" +
                                                  "}]";
    }
}