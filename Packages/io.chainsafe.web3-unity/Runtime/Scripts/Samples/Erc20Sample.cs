using System.Numerics;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3;
using Scripts.EVM.Token;

public class Erc20Sample
{
    private readonly Web3 web3;

    public Erc20Sample(Web3 web3)
    {
        this.web3 = web3;
    }

    public async Task<BigInteger> BalanceOf(string contractAddress, string account)
    {
        return await Erc20.BalanceOf(web3, contractAddress, account);
    }

    // TODO similar to previous, remove?
    public async Task<string> CustomTokenBalance(string contractAbi, string contractAddress)
    {
        var contract = web3.ContractBuilder.Build(contractAbi, contractAddress);
        var address = await web3.Signer.GetAddress();
        var response = await contract.Call("balanceOf", new object[] { address });
        var tokenBalance = response[0].ToString();
        return tokenBalance;
    }

    public async Task<BigInteger> Decimals(string contractAddress)
    {
        return await Erc20.Decimals(web3, contractAddress);
    }

    public async Task<string> Name(string contractAddress)
    {
        return await Erc20.Name(web3, contractAddress);
    }

    public async Task<BigInteger> NativeBalanceOf(string account)
    {
        return await web3.RpcProvider.GetBalance(account);
    }

    public async Task<string> Symbol(string contractAddress)
    {
        return await Erc20.Symbol(web3, contractAddress);
    }

    public async Task<BigInteger> TotalSupply(string contractAddress)
    {
        return await Erc20.TotalSupply(web3, contractAddress);
    }
}