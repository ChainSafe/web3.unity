using System;
using System.Collections.Generic;
#nullable enable

public class Web3AuthOptions {
    public string clientId { get; set; }
    public Web3Auth.Network network { get; set; }

    public Web3Auth.BuildEnv buildEnv { get; set; } = Web3Auth.BuildEnv.PRODUCTION;
    public Uri redirectUrl { get; set; }
    public string sdkUrl {
        get {
            if (buildEnv == Web3Auth.BuildEnv.STAGING)
                return "https://staging-auth.web3auth.io/{openLoginVersion}";
            else if (buildEnv == Web3Auth.BuildEnv.TESTING)
                return "https://develop-auth.web3auth.io";
            else 
                return "https://auth.web3auth.io/{openLoginVersion}";    
        }
        set { }
    }
    public const string openLoginVersion = "v5";

    public WhiteLabelData? whiteLabel { get; set; }
    public Dictionary<string, LoginConfigItem>? loginConfig { get; set; }
    public bool? useCoreKitKey { get; set; } = false;
    public Web3Auth.ChainNamespace? chainNamespace { get; set; } = Web3Auth.ChainNamespace.EIP155;
    public MfaSettings? mfaSettings { get; set; } = null;
}