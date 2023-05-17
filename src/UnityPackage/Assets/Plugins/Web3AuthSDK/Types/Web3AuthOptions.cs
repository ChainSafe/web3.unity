using System;
using System.Collections.Generic;

public class Web3AuthOptions {
    //public context: Context,
    public string clientId { get; set; }
    public Web3Auth.Network network { get; set; }
    public Uri redirectUrl { get; set; }
    public string sdkUrl { get; set; } = "https://sdk.openlogin.com";
    public WhiteLabelData whiteLabel { get; set; }
    public Dictionary<string, LoginConfigItem> loginConfig { get; set; }
}