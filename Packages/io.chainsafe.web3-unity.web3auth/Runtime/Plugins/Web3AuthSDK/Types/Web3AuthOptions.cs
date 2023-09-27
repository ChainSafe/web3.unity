using System;
using System.Collections.Generic;

public class Web3AuthOptions
{
    public string clientId { get; set; }
    public Web3Auth.Network network { get; set; }
    public Uri redirectUrl { get; set; }
    public string sdkUrl
    {
        get
        {
            if (network == Web3Auth.Network.TESTNET)
                return "https://dev-sdk.openlogin.com";
            else
                return "https://sdk.openlogin.com";
        }
        set { }
    }

    public WhiteLabelData whiteLabel { get; set; }
    public Dictionary<string, LoginConfigItem> loginConfig { get; set; }
    public bool? useCoreKitKey { get; set; } = false;
    public Web3Auth.ChainNamespace? chainNamespace { get; set; } = Web3Auth.ChainNamespace.EIP155;
}