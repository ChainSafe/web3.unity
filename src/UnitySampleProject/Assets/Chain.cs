using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Chain
{
    public const string EvmNamespace = "eip155";
    
    public static readonly Chain Ethereum = new Chain(EvmNamespace, "1", nameof(Ethereum));
    
    public static readonly Chain Goerli = new Chain(EvmNamespace, "5", "Ethereum Goerli");

    public string ChainNamespace { get; private set; }
    
    public string ChainId { get; private set; }
    
    public string Name { get; private set; }
    
    public string FullChainId => $"{ChainNamespace}:{ChainId}";
    
    public Chain(string chainNamespace, string chainId, string name)
    {
        ChainNamespace = chainNamespace;

        ChainId = chainId;

        Name = name;
    }
}
