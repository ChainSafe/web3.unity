using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.MultiCall;
using ChainSafe.Gaming.MultiCall.Dto;
using UnityEngine;

public class MultiCallSample
{
    private readonly Web3 _web3;

    public MultiCallSample(Web3 web3)
    {
        _web3 = web3;
    }

    public async Task<IMultiCallRequest[]> BlockStateExample()
    {
        var currentDifficulty = _web3.MultiCall().GetCurrentBlockDifficulty();
        return await _web3.MultiCall().MultiCallV3(new IMultiCallRequest[]
        {
            currentDifficulty
        });
    }
}
